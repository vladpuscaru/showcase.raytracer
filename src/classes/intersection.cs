
using OpenTK.Mathematics;

public class Intersection
{
    public Vector3 Point { get; set; }
    public Vector3 Normal { get; private set; }
    public Primitive Primitive { get; set; }
    public float Distance { get; set; }
    public Ray Ray { get; set; }

    public Intersection(Ray ray, float distance, Vector3 normal, Primitive primitive)
    {
        Ray = ray;
        Point = ray.GetPointAt(distance);
        Normal = Vector3.Normalize(normal);
        Primitive = primitive;
        Distance = distance;
    }
}