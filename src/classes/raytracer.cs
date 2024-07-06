using Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

class RayTracer
{
    public Camera Camera { get; set; }
    public Surface Screen { get; set; }
    public Scene[] Scenes { get; set; }
    private float lastRecordedTime = 0.0f;
    private float currentTime = 0.0f;
    // private uint frames = 0;
    private float FrameTime = 0;
    private List<DebugRayInfo> debugRays = new();

    public void ToggleDebug()
    {
        Settings.DEBUG_SCREEN = !Settings.DEBUG_SCREEN;
        Camera = new Camera(Screen, new Vector3(0, 0, 0), debug: Settings.DEBUG_SCREEN);
    }

    /* Prints relevant information to the console */
    private void PrintConfig(float deltaTime)
    {
        Console.Clear();

        /* Frametime */
        currentTime += deltaTime;
        FrameTime = currentTime - lastRecordedTime;
        lastRecordedTime = currentTime;

        /* Camera Settings */
        Console.WriteLine("Camera Settings");
        Console.WriteLine("* Viewport: " + Camera.ViewportWidth + " x " + Camera.ViewportHeight);
        Console.WriteLine("* Aspect Ratio: " + Camera.AspectRatio);
        Console.WriteLine("* Focal Length: " + Camera.FocalLength);
        Console.WriteLine("* FOV: " + Camera.FOV);

        /* Application Settings */
        Console.WriteLine("Application Settings");
        Console.WriteLine("* Debug: " + Settings.DEBUG_SCREEN);
        Console.WriteLine("* Active Scene: " + Settings.ACTIVE_SCENE + ": " + Scenes[Settings.ACTIVE_SCENE].Name);
        Console.WriteLine("Anti-Aliasing ray samples: " + Settings.N_RAY_SAMPLES_PER_PX_AXIS * Settings.N_RAY_SAMPLES_PER_PX_AXIS);
        Console.WriteLine("Frametime (s): " + FrameTime);
    }

    public RayTracer(Surface screenSurface)
    {
        Camera = new Camera(screenSurface, new Vector3(0, 0, 0), debug: Settings.DEBUG_SCREEN);
        Screen = screenSurface;

        Scenes = new Scene[]{
            Scene.SceneMirror(),
            Scene.ScenePrimary(),
            Scene.SceneTriangles(),
            Scene.SceneSpecular(),
            Scene.SceneGalaxy(),
            Scene.SceneTextures(),
        };
    }

    public void Render(float deltaTime)
    {
        // Clear the screen
        Screen.Clear(0);
        // Clear the debug rays
        debugRays.Clear();

        // Print the configuration
        PrintConfig(deltaTime);

        // Determine the screen width based on the debug mode
        int screenWidth = Settings.DEBUG_SCREEN ? Screen.width / 2 : Screen.width;

        /* Main Raytracing Loops */
        // Parallelize the rendering of each pixel
        Parallel.For(0, Screen.height, px_y =>
        {
            for (int px_x = 0; px_x < screenWidth; px_x++)
            {
                // Initialize the color of the pixel
                Color px_color = new(0, 0, 0);

                // Iterate over the ray samples along each axis
                for (float dy = 0; dy < 1.0f; dy += 1f / Settings.N_RAY_SAMPLES_PER_PX_AXIS)
                {
                    for (float dx = 0; dx < 1.0f; dx += 1f / Settings.N_RAY_SAMPLES_PER_PX_AXIS)
                    {
                        // Get the sampled rays if N_RAY_SAMPLES_PER_PX_AXIS > 1, else use a single ray per pixel
                        float px_xx = px_x + dx;
                        float px_yy = px_y + dy;
                        Ray primaryRay = getPrimaryRay(px_xx, px_yy);
                        // Only store a single debug ray for each pixel instead of all the samples to avoid a visual mess
                        bool storeDebugRays = Settings.DEBUG_SCREEN && px_yy == Screen.height / 2 && px_xx % 10 == 0;
                        bool storeShadowRays = Settings.DEBUG_SCREEN && px_xx % 10 == 0;
                        // Get the color of the pixel by recursively tracing the ray
                        px_color += getColorRecursive(primaryRay, storeDebugRays, storeShadowRays);
                    }
                }
                // Take the average color of the sampled rays
                px_color *= (float)1 / (Settings.N_RAY_SAMPLES_PER_PX_AXIS * Settings.N_RAY_SAMPLES_PER_PX_AXIS);

                // Clamp the color to the range [0, 1]
                px_color = px_color.Clamp(0f, 1f);
                // Plot the pixel on the screen
                Screen.Plot(px_x, px_y, Utils.ColorToInt(px_color));
            }
        });

        /* Debug Screen */
        // Generate the debug screen if debug mode is enabled
        if (Settings.DEBUG_SCREEN)
        {
            // Generate the debug screen
            Surface debugScreen = Debug.GenerateDebugScreen(Screen.width / 2, Screen.height, Scenes[Settings.ACTIVE_SCENE], Camera, debugRays);
            // Copy the debug screen pixels to the main screen pixels
            for (int x = 0; x < Screen.width / 2; x++)
            {
                for (int y = 0; y < Screen.height; y++)
                {
                    // Calculate the screen pixel index
                    int screenPixel = Settings.DEBUG_SCREEN ? y * Screen.width + x + Screen.width / 2 : y * Screen.width + x;
                    // Copy the debug screen pixel to the main screen pixel
                    Screen.pixels[screenPixel] = debugScreen.pixels[y * debugScreen.width + x];
                }
            }
        }
    }

    public Color getColorRecursive(Ray ray, bool storeDebugRays, bool storeShadowRays, int nBounces = 5)
    {
        Color color = new Color(0, 0, 0);

        if (nBounces < 0) return color;

        Intersection? closestIntersect = Scenes[Settings.ACTIVE_SCENE].GetClosestIntersection(ray);

        if (closestIntersect != null)
        {
            /**
             * Calculate texture mapping
             */
            Color? textureColor = closestIntersect.Primitive.MapTexture(closestIntersect.Point);
            if (textureColor != null)
            {
                color += textureColor;
            }

            /**
             * Calculate reflected light
             */
            if (closestIntersect.Primitive.Material.SpecularFraction > 0)
            {
                Vector3 reflectedRayDirection = Utils.GetReflectionVector(ray.Direction, closestIntersect.Normal);
                // set reflected ray origin an epsilon value away from the intersection point, to avoid floating precision errors.
                Vector3 rayOrigin = closestIntersect.Point + 0.0001f * closestIntersect.Normal;

                Ray reflectedRay = new Ray(rayOrigin, reflectedRayDirection);
                color += getColorRecursive(reflectedRay, storeDebugRays, storeShadowRays, nBounces - 1) * closestIntersect.Primitive.Material.SpecularFraction;
            }


            /**
             * Calculate shading + shadow
             */
            foreach (Light light in Scenes[Settings.ACTIVE_SCENE].Lights)
            {
                Ray shadowRay = getShadowRay(closestIntersect.Point, light);
                Intersection? closestShadowRayIntersection = null;
                if (Scenes[Settings.ACTIVE_SCENE].IsLightVisible(shadowRay, closestIntersect, light, out closestShadowRayIntersection))
                {
                    /**
                     * If light hits the surface, calculate shading
                     * Otherwise, leave the color as default (0, 0, 0)
                     */
                    color += closestIntersect.Primitive.Material.Shade(closestIntersect, light);
                }

                /* Only show shadow rays in debug if they actually contribute the a pixel that's in shadow */
                if (storeShadowRays && closestShadowRayIntersection != null)
                {
                    Color debugRayColor = Color.COLOR_SHADOOW_RAY;

                    /* Maybe nice for visualization to see that the ray is in the drection of the light */
                    // closestShadowRayIntersection.Point = light.Position;

                    debugRays.Add(new DebugRayInfo(closestShadowRayIntersection, shadowRay, debugRayColor));
                }
            }
        }

        else
        {
            // when there's no intersection, simply use the ambient color.
            color = Light.AmbientColor;
        }
        // set color of debug ray to the diffuse color of the intersection primitive material
        if (storeDebugRays)
        {
            Color debugRayColor = closestIntersect != null ? closestIntersect.Primitive.Material.DiffuseColor : Color.COLOR_RAY_NO_INTERSECTION;
            debugRays.Add(new DebugRayInfo(closestIntersect, ray, debugRayColor));
        }

        return color;
    }

    public Ray getPrimaryRay(float x, float y)
    {
        // Calculate the pixel coordinate ratios between 0 and 1.
        // If in debug mode, halve the screen width.
        float xRatio = Settings.DEBUG_SCREEN ? x / (Screen.width / 2) : x / Screen.width;
        float yRatio = y / Screen.height;

        // Calculate the pixel coordinate in UV space.
        // The UV space is centered at (0, 0). The range depends on the viewport size.
        float xUV = xRatio * Camera.ViewportWidth - Camera.ViewportWidth / 2.0f;
        float yUV = yRatio * Camera.ViewportHeight - Camera.ViewportHeight / 2.0f;

        // Calculate the point of the pixel in world space.
        Vector3 pixelSample = Camera.Position + Camera.LookAt * Camera.FocalLength +
                                xUV * Camera.Right -
                                yUV * Camera.Up;

        Vector3 direction = pixelSample - Camera.Position;

        return new Ray(
            Camera.Position,
            direction
        );
    }

    public Ray getShadowRay(Vector3 intersectionPoint, Light light)
    {
        // Calculate the direction vector from the intersection point to the light source.
        Vector3 pointToLight = light.Position - intersectionPoint;

        // Return the Ray representing the shadow ray.
        return new Ray(
            intersectionPoint,
            pointToLight
        );
    }

    public void HandleInput(KeyboardState keyboardState, double deltaTime)
    {
        Camera.HandleInput(keyboardState, deltaTime);
        Scenes[Settings.ACTIVE_SCENE].HandleInput(keyboardState, deltaTime);
        if (keyboardState.IsKeyPressed(Keys.X)) { ToggleDebug(); Console.WriteLine("Debug toggled"); }

        if (keyboardState.IsKeyPressed(Keys.Z)) { Settings.ACTIVE_SCENE++; Settings.ACTIVE_SCENE = Settings.ACTIVE_SCENE >= Scenes.Length ? 0 : Settings.ACTIVE_SCENE; }
    }
}