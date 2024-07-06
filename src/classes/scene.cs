using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
public class Scene
{
    public List<Primitive> Primitives { get; private set; }
    public List<Light> Lights { get; private set; }
    public string Name { get; set; } = "Scene";

    public static Scene SceneSingleSphere()
    {
        Scene scene = new Scene();
        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -4f), 1f,
                new Material(new Color(1.0f, 1f, 1.0f), new Color(0.5f, 0.5f, 0.5f), 10f, 0f)
            )
        );

        scene.AddLight(new Light(new Vector3(0f, 5f, -4f), new Color(200, 200, 200)));

        return scene;
    }

    public static Scene SceneSingleTriangle()
    {
        Scene scene = new Scene();

        Vector3[] BaseTriangle1 = {
            new Vector3(1.0f, 5.0f, -5.0f),
            new Vector3(1.5f, 0.0f, -4.0f),
            new Vector3(-1.5f, 0.0f, -4.0f)
        };

        scene.AddTriangle(new Triangle(
            BaseTriangle1[0], BaseTriangle1[1], BaseTriangle1[2],
            new Material(new Color(1f, 0.1f, 0.1f), new Color(1f, 0.1f, 0.1f), 10f, 0f)
        ));

        scene.AddLight(new Light(new Vector3(0f, 5f, -4f), new Color(200, 200, 200)));

        return scene;
    }

    public static Scene SceneMirror()
    {
        Scene scene = new();
        scene.Name = "Mirror Scene";
        scene.AddPlane(
            new Plane(
                new Vector3(0, 1, 0), -1,
                new Material(new Color(.6f, .6f, .8f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        Vector3[] BaseTriangle1 = {
            new Vector3(-1f, 5.0f, -16.0f),
            new Vector3(-2f, 0.0f, -16.0f),
            new Vector3(-3f, 0.0f, 0.0f),
        };

        Vector3[] BaseTriangle2 = {
            new Vector3(1f, 5.0f, -16.0f),
            new Vector3(2f, 0.0f, -16.0f),
            new Vector3(3f, 0.0f, 0.0f),
        };

        scene.AddTriangle(new Triangle(
            BaseTriangle1[2], BaseTriangle1[1], BaseTriangle1[0],
            new Material(new Color(1f, 1f, 1f), new Color(1f, 1f, 1f), 0f, 1f)
        ));

        scene.AddTriangle(new Triangle(
            BaseTriangle2[0], BaseTriangle2[1], BaseTriangle2[2],
            new Material(new Color(1f, 1f, 1f), new Color(1f, 1f, 1f), 0f, 1f)
        ));

        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -10f), 1f,
                new Material(new Color(1f, 0f, 0f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -8f), .6f,
                new Material(new Color(0f, 1f, 0f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -6f), .3f,
                new Material(new Color(0f, 0f, 1f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -4f), .1f,
                new Material(new Color(1f, 1f, 1f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        scene.AddLight(new Light(new Vector3(0f, 10.7f, -0.8f), new Color(200, 200, 200)));

        return scene;
    }

    public static Scene SceneTriangles()
    {
        Scene scene = new();
        scene.Name = "Triangles Scene";
        scene.AddPlane(
            new Plane(
                new Vector3(0, 1, 0), -1,
                new Material(new Color(.6f, .6f, .8f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        Vector3[] BaseTriangle1 = {
            new Vector3(-3.5f, 5.0f, -6.0f),
            new Vector3(-4.5f, 0.0f, -8.0f),
            new Vector3(1.5f, 0.0f, -8.0f)
        };

        Vector3[] BaseTriangle2 = {
            new Vector3(5.0f, 5.0f, -5.0f),
            new Vector3(5.5f, 0.0f, -4.0f),
            new Vector3(-1.5f, 0.0f, -4.0f)
        };

        scene.AddTriangle(new Triangle(
            BaseTriangle1[0], BaseTriangle1[1], BaseTriangle1[2],
            new Material(new Color(1f, 0.1f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, .9f)
        ));

        scene.AddTriangle(new Triangle(
            BaseTriangle2[0], BaseTriangle2[1], BaseTriangle2[2],
            new Material(new Color(.5f, 0.3f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f),
            new Texture(Utils.GetPathFromSrc("assets/stone_floor.png"))
        ));

        scene.AddSphere(
            new Sphere(
                new Vector3(-3f, 0f, -4f), 1f,
                new Material(new Color(1f, 1f, 1f), new Color(1f, 1f, 1f), 0f, 0f)
            )
        );

        scene.AddLight(new Light(new Vector3(-5f, 9f, 9f), new Color(200, 200, 200)));


        return scene;
    }

    public static Scene SceneReflection()
    {
        Scene scene = new();
        scene.Name = "Reflection Scene";
        scene.AddPlane(
            new Plane(
                new Vector3(0, 1, 0), -1,
                new Material(new Color(1f, .5f, 0f), new Color(0f, 0f, 0f), 0f, 0f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(-3f, 0f, -4f), 1f,
                new Material(new Color(1f, 1f, 1f), new Color(1f, 1f, 1f), 10f, 0f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -6f), .78f,
                new Material(new Color(.3f, .7f, .1f), new Color(.1f, .6f, 1f), 2f, .8f)
            )
        );


        scene.AddSphere(
            new Sphere(
                new Vector3(-1f, 0f, -3f), .5f,
                new Material(new Color(0f, 0f, 1f), new Color(1f, 1f, 1f), 10f, 0f)
            )
        );

        scene.AddLight(new Light(new Vector3(11f, 5.5f, 3.5f), new Color(200, 200, 200)));

        return scene;
    }

    public static Scene SceneSpecular()
    {
        Scene scene = new();
        scene.Name = "Specular Scene";
        scene.AddPlane(
            new Plane(
                new Vector3(0, 1, 0), -1,
                new Material(new Color(.5f, .5f, .5f), new Color(0f, 0f, 0f), 0f, 1.0f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(-3f, 0f, -4f), 1f,
                new Material(new Color(.1f, .5f, 1f), new Color(1f, 0f, 0f), 0f, 1f)
            )
        );

        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -6f), 1f,
                new Material(new Color(1f, .3f, .5f), new Color(0f, 0f, 0f), 0f, 0.5f)
            )
        );

        scene.AddLight(new Light(new Vector3(4f, 5.5f, -3f), new Color(20, 20, 15)));
        scene.AddLight(new Light(new Vector3(-4f, 5.5f, -3f), new Color(20, 20, 50)));
        scene.AddLight(new Light(new Vector3(-4f, 5.5f, -25f), new Color(10, 55, 0)));

        return scene;
    }

    public static Scene SceneGalaxy()
    {
        Scene scene = new();
        scene.Name = "Galaxy Scene";
        scene.AddPlane(
            new Plane(
                new Vector3(0, 0, 1), -25,
                new Material(new Color(.5f, .5f, .5f), new Color(0f, 0f, 0f), 0f, 0f),
                new Texture(Utils.GetPathFromSrc("assets/scene_galaxy_bg.jpg"))
            )
        );

        // Mercury - .38 of Earth's radius
        scene.AddSphere(
            new Sphere(
                new Vector3(-6f, 0f, -6f), .38f,
                new Material(new Color(1f, 1f, 1f), new Color(0f, 0f, 0f), 0f, 0f),
                new Texture(Utils.GetPathFromSrc("assets/scene_galaxy_mercury.jpeg"))
            )
        );

        // Venus - .94 of Earth's radius
        scene.AddSphere(
            new Sphere(
                new Vector3(-3f, 0f, -6f), .94f,
                new Material(new Color(1f, 1f, 1f), new Color(0f, 0f, 0f), 0f, 0f),
                new Texture(Utils.GetPathFromSrc("assets/scene_galaxy_venus.jpeg"))
            )
        );

        // Earth
        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -6f), 1f,
                new Material(new Color(1f, 1f, 1f), new Color(0f, 0f, 0f), 0f, 0f),
                new Texture(Utils.GetPathFromSrc("assets/scene_galaxy_earth.jpeg"))
            )
        );

        // Mars - .53 of Earth's radius
        scene.AddSphere(
            new Sphere(
                new Vector3(3f, 0f, -6f), .53f,
                new Material(new Color(1f, 1f, 1f), new Color(0f, 0f, 0f), 0f, 0f),
                new Texture(Utils.GetPathFromSrc("assets/scene_galaxy_mars.jpeg"))
            )
        );

        scene.AddLight(new Light(new Vector3(-25f, 0f, -8f), new Color(100, 100, 100)));

        return scene;
    }

    public static Scene ScenePyramid()
    {
        Scene scene = new Scene();
        scene.Name = "Pyramid Scene";

        Vector3[] BaseTriangle1 = {
            new Vector3(-1.0f, 0.0f, -5.0f),
            new Vector3(1.0f, 0.0f, -5.0f),
            new Vector3(-1.0f, 0.0f, -4.0f)
        };

        Vector3[] BaseTriangle2 = {
            new Vector3(1.0f, 0.0f, -5.0f),
            new Vector3(1.0f, 0.0f, -4.0f),
            new Vector3(-1.0f, 0.0f, -4.0f)
        };

        scene.AddTriangle(new Triangle(
            BaseTriangle1[0], BaseTriangle1[1], BaseTriangle1[2],
            new Material(new Color(1f, 0.1f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f)
        ));

        scene.AddTriangle(new Triangle(
            BaseTriangle2[0], BaseTriangle2[1], BaseTriangle2[2],
            new Material(new Color(1f, 1.0f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f)
        ));


        Vector3[] FaceTriangle1 = {
            new Vector3(0.0f, 1.0f, -4.5f), // Vertex of pyramid
            new Vector3(-1.0f, 0.0f, -4.0f),
            new Vector3(1.0f, 0.0f, -4.0f),
        };
        scene.AddTriangle(new Triangle(
            FaceTriangle1[0], FaceTriangle1[1], FaceTriangle1[2],
            new Material(new Color(1f, 1.0f, 1.0f), new Color(1f, 0.1f, 0.1f), 0f, 1.0f)
        ));

        Vector3[] FaceTriangle2 = {
            new Vector3(0.0f, 1.0f, -4.5f), // Vertex of pyramid
            new Vector3(1.0f, 0.0f, -4.0f),
            new Vector3(1.0f, 0.0f, -5.0f)
        };
        scene.AddTriangle(new Triangle(
            FaceTriangle2[0], FaceTriangle2[1], FaceTriangle2[2],
            new Material(new Color(1f, 1.0f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f)
        ));

        Vector3[] FaceTriangle3 = {
            new Vector3(0.0f, 1.0f, -4.5f), // Vertex of pyramid
            new Vector3(1.0f, 0.0f, -5.0f),
            new Vector3(-1.0f, 0.0f, -5.0f)
        };
        scene.AddTriangle(new Triangle(
            FaceTriangle3[0], FaceTriangle3[1], FaceTriangle3[2],
            new Material(new Color(1f, 1.0f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f)
        ));

        Vector3[] FaceTriangle4 = {
            new Vector3(0.0f, 1.0f, -4.5f), // Vertex of pyramid
            new Vector3(-1.0f, 0.0f, -5.0f),
            new Vector3(-1.0f, 0.0f, -4.0f)
        };
        scene.AddTriangle(new Triangle(
            FaceTriangle4[0], FaceTriangle4[1], FaceTriangle4[2],
            new Material(new Color(1f, 1.0f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f)
        ));

        scene.AddLight(new Light(new Vector3(5f, 5f, 0f), new Color(200, 200, 200)));
        scene.AddLight(new Light(new Vector3(-5f, -5f, 0f), new Color(200, 200, 200)));
        scene.AddLight(new Light(new Vector3(5f, 15f, -5f), new Color(200, 200, 200)));

        return scene;
    }

    public static Scene ScenePrimary()
    {
        Scene scene = new Scene();
        scene.Name = "Primary Scene";

        scene.AddLight(new Light(new Vector3(-5f, 10f, 0f), new Color(100, 100, 100)));
        scene.AddLight(new Light(new Vector3(5f, 10f, 0f), new Color(100, 100, 100)));

        // metallic red sphere -> specular equal to diffuse color
        scene.AddSphere(
            new Sphere(
                new Vector3(-3f, 0f, -4f), 1f,
                new Material(new Color(1f, 0.1f, 0.1f), new Color(1f, 0.1f, 0.1f), 10f, 1.0f),
            new Texture(Utils.GetPathFromSrc("assets/wood.jpg"))
            )
        );
        // 'plastic' green sphere -> white-ish specular color
        scene.AddSphere(
            new Sphere(
                new Vector3(0f, 0f, -4f), 1f,
                new Material(new Color(0.1f, 1f, 0.1f), new Color(0.5f, 0.5f, 0.5f), 10f, 0.5f),
                new Texture(Utils.GetPathFromSrc("assets/rocks.jpeg"))
            )
        );

        // diffuse blue sphere
        scene.AddSphere(
             new Sphere(
                 new Vector3(3f, 0f, -4f), 1f,
                 new Material(new Color(0f, 0.1f, 0.5f), new Color(0f, 0f, 0f), 0f, 0f),
                 new Texture(Utils.GetPathFromSrc("assets/worldmap.jpeg"))
             )
         );

        // diffuse grey ground plane
        scene.AddPlane(
            new Plane(
                new Vector3(0, 1, 0), -1,
                new Material(new Color(.5f, .5f, .5f), new Color(0f, 0f, 0f), 0f, 1.0f)
            )
        );
        // metallic golden back wall (very slow) 
        // scene.AddPlane(
        //     new Plane(new Vector3(0, 0, 1), -15,
        //     new Material(new Color(1f, 1f, 0.1f), new Color(1f, 1f, 0.1f), 0f, 0.9f))
        // );

        return scene;
    }

    public static Scene SceneTextures()
    {
        Scene scene = new Scene();
        scene.Name = "Textures Scene";

        // AddLight(new Light(new Vector3(-5f, 10f, -8f), new Color(200, 200, 200)));
        scene.AddLight(new Light(new Vector3(5f, 15f, 6f), new Color(200, 200, 200)));

        // diffuse blue sphere
        scene.AddSphere(
             new Sphere(
                 new Vector3(3f, 0f, -4f), 1f,
                 new Material(new Color(0.1f, 0.1f, 1f), new Color(0f, 0f, 0f), 0f, 0f),
                 new Texture(Utils.GetPathFromSrc("assets/rocks.jpeg"))
             )
         );

        Vector3[] BaseTriangle1 = {
            new Vector3(-1.0f, 5.0f, -5.0f),
            new Vector3(1.0f, 2.5f, -3.0f),
            new Vector3(-3.0f, 1.0f, -4.0f)
        };
        scene.AddTriangle(new Triangle(
            BaseTriangle1[0], BaseTriangle1[1], BaseTriangle1[2],
            new Material(new Color(1f, 0.1f, 0.1f), new Color(1f, 0.1f, 0.1f), 0f, 0f),
            new Texture(Utils.GetPathFromSrc("assets/wood.jpg"))
        ));

        // diffuse grey ground plane
        scene.AddPlane(
            new Plane(
                new Vector3(0, 1, 0), -1,
                new Material(new Color(.5f, .5f, .5f), new Color(0f, 0f, 0f), 0f, 0.5f),
                new Texture(Utils.GetPathFromSrc("assets/stone_floor.png"))
            )
        );

        return scene;
    }

    public Scene()
    {
        Primitives = new List<Primitive>();
        Lights = new List<Light>();
    }

    public void AddSphere(Sphere sphere) { Primitives.Add(sphere); }
    public void AddPlane(Plane plane) { Primitives.Add(plane); }
    public void AddTriangle(Triangle triangle) { Primitives.Add(triangle); }
    public void AddLight(Light light) { Lights.Add(light); }

    /// <summary>
    /// Finds the closest intersection of the given ray with any primitive in the scene.
    /// </summary>
    /// <param name="ray">The ray to trace.</param>
    /// <returns>The closest intersection, or null if no intersection was found.</returns>
    public Intersection? GetClosestIntersection(Ray ray)
    {
        // The closest intersection found so far.
        Intersection? closestIntersect = null;

        // Check for intersection with each primitive in the scene.
        foreach (Primitive primitive in Primitives)
        {
            Intersection? newIntersect = primitive.Intersect(ray);

            // If an intersection was found and it is closer than the previous closest intersection,
            // update the closest intersection.
            if (newIntersect != null)
            {
                if (closestIntersect == null || newIntersect.Distance < closestIntersect.Distance)
                {
                    closestIntersect = newIntersect;
                }
            }
        }
        // Return the closest intersection found, or null if no intersection was found.
        return closestIntersect;
    }

    public bool IsLightVisible(Ray shadowray, Intersection primaryIntersect, Light light, out Intersection? closestIntersection)
    {
        // Move the shadow ray's origin slightly away from the intersection along the normal,
        // so that the ray doesn't intersect the primitive it originated from.
        shadowray.Origin += (float)0.0001 * primaryIntersect.Normal;

        // Find the closest intersection of the shadow ray with any primitive in the scene.
        closestIntersection = GetClosestIntersection(shadowray);

        float lightDistance = (light.Position - shadowray.Origin).Length;

        bool isLightVisible =
            closestIntersection == null || // No intersection in direction of light
            closestIntersection.Distance > lightDistance; // Intersection is past the light

        return isLightVisible;
    }

    public void HandleInput(KeyboardState keyboardState, double deltaTime)
    {
        foreach (Light light in Lights)
        {
            light.HandleInput(keyboardState, (float)deltaTime);
        }
    }
}
