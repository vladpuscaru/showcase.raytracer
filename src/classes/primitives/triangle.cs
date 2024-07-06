using OpenTK.Mathematics;

public class Triangle : Primitive
{
    public Vector3 VertexA { get; set; }
    public Vector3 VertexB { get; set; }
    public Vector3 VertexC { get; set; }

    public Vector3 VertexANormal { get; set; }
    public Vector3 VertexBNormal { get; set; }
    public Vector3 VertexCNormal { get; set; }

    public Vector2 uvA { get; set; }
    public Vector2 uvB { get; set; }
    public Vector2 uvC { get; set; }

    public float Area { get; set; }

    public Vector3 Normal { get; set; }


    public Triangle(Vector3 vA, Vector3 vB, Vector3 vC, Material material, Texture? texture = null) : base(material, texture)
    {
        VertexA = vA;
        VertexB = vB;
        VertexC = vC;

        Normal = Vector3.Cross(VertexB - VertexA, VertexC - VertexA).Normalized();
        Area = Utils.ComputeTriangleArea(VertexA, VertexB, VertexC);

        /**
         * Make the vertex normal different than the geometric normal
         */
        VertexANormal = new Vector3(0, 1, 0);
        VertexBNormal = new Vector3(1, 0, 0);
        VertexCNormal = new Vector3(0, 0, 1);


        uvA = new Vector2(0.0f, 0.0f);
        uvB = new Vector2(0.5f, 1.0f);
        uvC = new Vector2(1.0f, 0.0f);
    }

    private void ComputeBarycentricCoordinates(Vector3 point, out float alpha, out float beta, out float gamma)
    {
        /**
         * Check if P is inside the triangle
         */
        bool isInside = Vector3.Dot(Vector3.Cross(VertexB - VertexA, point - VertexA), Normal) >= 0 &&
                        Vector3.Dot(Vector3.Cross(VertexA - VertexC, point - VertexC), Normal) >= 0 &&
                        Vector3.Dot(Vector3.Cross(VertexC - VertexB, point - VertexB), Normal) >= 0;

        if (isInside)
        {
            alpha = Utils.ComputeTriangleArea(point, VertexB, VertexC) / Area;
            beta = Utils.ComputeTriangleArea(point, VertexC, VertexA) / Area;
            gamma = 1 - alpha - beta;
        }
        else
        {
            alpha = beta = gamma = -1;
        }
    }

    public override Intersection? Intersect(Ray ray)
    {
        /**
         * Ray-Plane intersection
         */
        float denominator = Vector3.Dot(ray.Direction, Normal);
        if (denominator == 0) { return null; } // ray parallel with plane
        float numerator = Vector3.Dot(VertexA, Normal) - Vector3.Dot(ray.Origin, Normal);
        float t = numerator / denominator;
        if (t < 0) { return null; } // intersection behind ray

        Vector3 P = ray.GetPointAt(t);

        float alpha, beta, gamma;
        ComputeBarycentricCoordinates(P, out alpha, out beta, out gamma);

        if (alpha == -1 || beta == -1 || gamma == -1)
        {
            return null;
        }

        return new Intersection(ray, t, Normal, this);
    }

    public override Color? MapTexture(Vector3 point)
    {
        if (Texture == null) return null;

        float alpha, beta, gamma;
        ComputeBarycentricCoordinates(point, out alpha, out beta, out gamma);

        if (alpha == -1 || beta == -1 || gamma == -1)
        {
            return null;
        }

        Vector2 uv = uvA * alpha + uvB * beta + uvC * gamma;

        int color = Texture.LookUp(uv.X, uv.Y);
        return Utils.IntToColor(color);
    }
}