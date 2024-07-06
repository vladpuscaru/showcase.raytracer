using OpenTK.Mathematics;

public class Plane : Primitive
{
    public Vector3 Normal { get; set; }
    public float Distance { get; set; }

    public Plane(Vector3 normal, float distance, Material material, Texture? texture = null) : base(material, texture)
    {
        Normal = Vector3.Normalize(normal);
        Distance = distance;
    }

    public override Intersection? Intersect(Ray ray)
    {
        float denominator = Vector3.Dot(ray.Direction, Normal);
        if (denominator == 0) { return null; } // ray parallel with plane
        float numerator = Distance - Vector3.Dot(ray.Origin, Normal);
        float t = numerator / denominator;
        if (t < 0) { return null; } // intersection behind ray
        return new Intersection(ray, t, Normal, this);
    }

    public override Color? MapTexture(Vector3 point)
    {
        if (Texture == null) return null;

        Vector3 e1 = Vector3.Normalize(Vector3.Cross(Normal, new Vector3(1, 0, 0)));

        //If normal and (1,0,0) are parallel, change e1
        if (e1 == new Vector3(0, 0, 0))
        {
            e1 = Vector3.Normalize(Vector3.Cross(Normal, new Vector3(0, 0, 1)));
        }
        Vector3 e2 = Vector3.Normalize(Vector3.Cross(Normal, e1));

        float u = Vector3.Dot(e1, point);
        float v = Vector3.Dot(e2, point);


        int color = Texture.LookUp(u, v, Texture.MappingBehaviour.REPEAT);
        return Utils.IntToColor(color);
    }
}