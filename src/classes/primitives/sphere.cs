using OpenTK.Mathematics;

public class Sphere : Primitive
{
    public Vector3 Position { get; set; }
    public float Radius { get; set; }

    public Sphere(Vector3 position, float radius, Material material, Texture? texture = null) : base(material, texture)
    {
        Position = position;
        Radius = radius;
    }

    public override Intersection? Intersect(Ray ray)
    {
        Vector3 co = ray.Origin - Position;
        float a = Vector3.Dot(ray.Direction, ray.Direction);
        float b = 2.0f * Vector3.Dot(ray.Direction, co);
        float c = Vector3.Dot(co, co) - Radius * Radius;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0) return null; // No intersection

        // single intersection
        else if (discriminant == 0)
        {
            float t = -b / (2f * a); // ignore discriminant as it's 0.
            Vector3 intersectionPoint = ray.GetPointAt(t);
            Vector3 normal = intersectionPoint - Position;
            return new Intersection(ray, t, normal, this);
        }

        // double intersection
        else
        {
            float sqrtd = (float)MathF.Sqrt(discriminant);
            float t0 = (-b - sqrtd) / (2f * a);
            float t1 = (-b + sqrtd) / (2f * a);

            if (t0 < 0 || t1 < 0) return null; // intersection is behind the ray

            float t = MathHelper.Min(t0, t1);

            Vector3 intersectionPoint = ray.GetPointAt(t);
            Vector3 normal = intersectionPoint - Position;
            return new Intersection(ray, t, normal, this);
        }
    }

    public override Color? MapTexture(Vector3 point)
    {
        if (Texture == null) return null;

        /**
         * 𝜃 = (arccos((𝑧 − 𝑧c) / 𝑟)
         * 𝜑 = atan2(𝑦 − 𝑦c , 𝑥 − 𝑥c)
         *
         * u = (phi + PI) / 2 * PI
         * v = theta / PI
         */
        float theta = (float)MathHelper.Acos((point.Z - Position.Z) / Radius);
        float phi = (float)MathHelper.Atan2(point.Y - Position.Y, point.X - Position.X);

        float u = (phi + MathHelper.Pi) / MathHelper.TwoPi;
        float v = theta / MathHelper.Pi;

        int color = Texture.LookUp(u, v);
        return Utils.IntToColor(color);
    }
}