using OpenTK.Mathematics;
using Template;

public class DebugRayInfo
{
    public Intersection? Intersection { get; set; }
    public Ray Ray { get; set; }

    public Color Color { get; set; }

    public DebugRayInfo(Intersection? intersection, Ray ray, Color color)
    {
        Intersection = intersection;
        Ray = ray;
        Color = color;
    }
}

public static class Debug
{
    private static float INFINITE_RAY_PARAMETER = 100; // Don't set this to something really big cuz it can overflow
    private static int SPHERE_POINT_COUNT = 100;

    /**
     * Calculates optimal width and height sizes such that
     * all objects (camera, primitives, lights) in the scene are present
     *
     * It checks what are the farthest coordinates in the XZ plane of the 3D world
     * where objects are present.
     *
     * The final result also accounts for the aspect ratio of the actual debug window
     * by adjusting the final viewport width.
     */
    private static Vector2 GetDebugViewportSize(Scene scene, Camera camera, float aspectRatio)
    {
        float zRange = 0.0f;
        float xRange = 0.0f;

        /* Check Primitives */
        foreach (Primitive p in scene.Primitives)
        {
            if (p is Sphere sphere)
            {
                if (MathHelper.Abs(sphere.Position.Z) >= zRange)
                {
                    zRange = MathHelper.Abs(sphere.Position.Z);
                    zRange += sphere.Radius; /* Account for the sphere radius */
                }

                if (MathHelper.Abs(sphere.Position.X) >= xRange)
                {
                    xRange = MathHelper.Abs(sphere.Position.X);
                    xRange += sphere.Radius; /* Account for the sphere radius */
                }
            }
        }

        /* Check Lights */
        foreach (Light l in scene.Lights)
        {
            if (MathHelper.Abs(l.Position.Z) >= zRange)
            {
                zRange = MathHelper.Abs(l.Position.Z);
                zRange += 0.25f; /* Some space around the light point */
            }

            if (MathHelper.Abs(l.Position.X) >= xRange)
            {
                xRange = MathHelper.Abs(l.Position.X);
                xRange += 0.25f; /* Some space around the light point */
            }
        }

        /* Check Camera */
        if (Math.Abs(camera.Position.Z) > zRange) zRange = Math.Abs(camera.Position.Z);
        if (Math.Abs(camera.Position.X) > xRange) xRange = Math.Abs(camera.Position.X);

        /* xRange and zRange are only in one direction right now, so doublen them */
        xRange *= 2;
        zRange *= 2;

        /* Increase the width of the viewport such that the aspect ratio is maintained */
        if (xRange / zRange != aspectRatio)
        {
            /* Maintain the longer dimension and adjust the smaller one to account for aspect ratio */
            if (xRange > zRange)
            {
                zRange += xRange / aspectRatio - zRange;
            }
            else
            {
                xRange += zRange * aspectRatio - xRange;
            }
        }

        return new Vector2(xRange, zRange);
    }

    private static void ProjectCameraToDebugScreen(Camera camera, Surface screen, Vector2 viewportSize)
    {
        Vector2 cameraOriginNDC = new Vector2(camera.Position.X, camera.Position.Z);
        Vector2i origin = Camera.FromNDCToPixels(cameraOriginNDC, viewportSize, screen.width, screen.height);

        /**
         * Represent the camera as a 4x4 square
         */
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                int x1 = origin.X - i; int y1 = origin.Y - j;
                int x2 = origin.X + i; int y2 = origin.Y + j;
                screen.Plot(x1, y1, 0xff0000);
                screen.Plot(x2, y2, 0xff0000);
            }
        }
    }

    private static void ProjectCameraViewportToDebugScreen(Camera camera, Surface screen, Vector2 viewportSize)
    {
        /**
         * Camera Viewport
         * Should be drawn last so it goes over the rays
         */
        Vector3 viewportLeftCenter = camera.Position + camera.LookAt * camera.FocalLength
                                    - camera.Right * camera.ViewportWidth / 2
                                    - camera.Up * camera.ViewportHeight / 2;
        Vector2 viewportLeftCenter2D = new Vector2(viewportLeftCenter.X, viewportLeftCenter.Z);
        Vector2i viewportLeft = Camera.FromNDCToPixels(viewportLeftCenter2D, viewportSize, screen.width, screen.height);

        Vector3 viewportRightCenter = camera.Position + camera.LookAt * camera.FocalLength
                        + camera.Right * camera.ViewportWidth / 2
                        - camera.Up * camera.ViewportHeight / 2;
        Vector2 viewportRightCenter2D = new Vector2(viewportRightCenter.X, viewportRightCenter.Z);
        Vector2i viewportRight = Camera.FromNDCToPixels(viewportRightCenter2D, viewportSize, screen.width, screen.height);

        // Making it ticker, by drawing multiple stacked lines
        screen.Line(viewportLeft.X, viewportLeft.Y, viewportRight.X, viewportRight.Y, 0xffffff);
        screen.Line(viewportLeft.X, viewportLeft.Y - 1, viewportRight.X, viewportRight.Y - 1, 0xffffff);
        screen.Line(viewportLeft.X, viewportLeft.Y + 1, viewportRight.X, viewportRight.Y + 1, 0xffffff);
    }

    private static void ProjectRaysToDebugScreen(List<DebugRayInfo> debugRays, Surface screen, Vector2 viewportSize, out Dictionary<Vector2i, Sphere> debugSpheres)
    {
        debugSpheres = new();

        foreach (DebugRayInfo debugRay in debugRays)
        {
            Vector2 origin2dNDC = new Vector2(debugRay.Ray.Origin.X, debugRay.Ray.Origin.Z);
            Vector2i origin2D = Camera.FromNDCToPixels(origin2dNDC, viewportSize, screen.width, screen.height);

            Vector3 endpoint = Vector3.Zero;
            if (debugRay.Intersection != null)
            {
                endpoint = debugRay.Intersection.Point;
            }
            else
            {
                endpoint = debugRay.Ray.Origin + debugRay.Ray.Direction * INFINITE_RAY_PARAMETER;
            }

            Vector2 endpoint2dNDC = new Vector2(endpoint.X, endpoint.Z);
            Vector2i endpoint2D = Camera.FromNDCToPixels(endpoint2dNDC, viewportSize, screen.width, screen.height);

            /* Draw the ray as a line in 2D world */
            screen.Line(origin2D.X, origin2D.Y, endpoint2D.X, endpoint2D.Y, Utils.ColorToInt(debugRay.Color));



            /* 
             * If the ray intersected a sphere, save the coordinated of that sphere 
             * This ensures only spheres that are visible in for the camera are being shown in debug
             */
            if (debugRay.Intersection != null && debugRay.Intersection.Primitive is Sphere)
            {
                Sphere sphere = (Sphere)debugRay.Intersection.Primitive;

                Vector2 sphereCenterNDC = new Vector2(sphere.Position.X, sphere.Position.Z);
                Vector2i sphereCenter = Camera.FromNDCToPixels(sphereCenterNDC, viewportSize, screen.width, screen.height);

                // Vector2i has overloaded Equals and ==
                // to check for the X and Y values
                if (!debugSpheres.ContainsKey(sphereCenter))
                {
                    debugSpheres.Add(sphereCenter, sphere);
                }
            }
        }
    }

    private static void ProjectSpheresToDebugScreen(Dictionary<Vector2i, Sphere> debugSpheres, Surface screen, Vector2 viewportSize)
    {
        foreach (var pair in debugSpheres)
        {
            Vector2i sphereCenter = pair.Key;
            Sphere sphere = pair.Value;

            float radiusX = sphere.Radius / viewportSize.X * screen.width;
            float radiusY = sphere.Radius / viewportSize.Y * screen.height;

            int radiusInPixelsX = (int)(radiusX);
            int radiusInPixelsY = (int)(radiusY);

            List<Vector2i> spherePoints = new List<Vector2i>();
            Vector2 direction = new Vector2(1, 0);
            float theta = 360.0f / SPHERE_POINT_COUNT;
            for (int i = 0; i < SPHERE_POINT_COUNT; i++)
            {
                float angle = MathHelper.DegreesToRadians(theta) * i;
                Vector2 rotatedDirection = new Vector2();
                rotatedDirection.X = (float)(Math.Cos(angle) * direction.X - Math.Sin(angle) * direction.Y);
                rotatedDirection.Y = (float)(Math.Sin(angle) * direction.X + Math.Cos(angle) * direction.Y);

                Vector2i spherePoint = new Vector2i();
                spherePoint.X = (int)(sphereCenter.X + rotatedDirection.X * radiusInPixelsX);
                spherePoint.Y = (int)(sphereCenter.Y + rotatedDirection.Y * radiusInPixelsY);
                spherePoints.Add(spherePoint);
            }

            for (int i = 0; i < spherePoints.Count - 1; i++)
            {
                screen.Line(spherePoints[i].X, spherePoints[i].Y, spherePoints[i + 1].X, spherePoints[i + 1].Y, Utils.ColorToInt(sphere.Material.DiffuseColor));
            }

            // Connect last point with first point, closing the circle
            screen.Line(spherePoints[spherePoints.Count - 1].X, spherePoints[spherePoints.Count - 1].Y, spherePoints[0].X, spherePoints[0].Y, Utils.ColorToInt(sphere.Material.DiffuseColor));
        }
    }

    private static void ProjectLightsToDebugScreen(Scene scene, Surface screen, Vector2 viewportSize)
    {
        foreach (Light light in scene.Lights)
        {
            Vector2 light2dNDC = new Vector2(light.Position.X, light.Position.Z);
            Vector2i light2D = Camera.FromNDCToPixels(light2dNDC, viewportSize, screen.width, screen.height);

            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    int x1 = light2D.X - i; int y1 = light2D.Y - j;
                    int x2 = light2D.X + i; int y2 = light2D.Y + j;
                    screen.Plot(x1, y1, 0xffffff);
                    screen.Plot(x2, y2, 0xffffff);
                }
            }
        }
    }

    public static Surface GenerateDebugScreen(int width, int height, Scene scene, Camera camera, List<DebugRayInfo> debugRays)
    {
        Surface screen = new Surface(width, height);
        Vector2 viewportSize = GetDebugViewportSize(scene, camera, width / height);

        Dictionary<Vector2i, Sphere> debugSpheres = new();

        ProjectCameraToDebugScreen(camera, screen, viewportSize);

        ProjectRaysToDebugScreen(debugRays, screen, viewportSize, out debugSpheres);

        ProjectSpheresToDebugScreen(debugSpheres, screen, viewportSize);

        ProjectLightsToDebugScreen(scene, screen, viewportSize);

        /* Draw camera viewport last so it is drawn over the rays */
        ProjectCameraViewportToDebugScreen(camera, screen, viewportSize);

        return screen;
    }
}