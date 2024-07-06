using OpenTK.Mathematics;
public abstract class Primitive
{
    public Material Material { get; protected set; }

    public Texture? Texture { get; protected set; }

    public Primitive(Material material, Texture? texture = null)
    {
        Material = material;
        Texture = texture;
    }

    public abstract Intersection? Intersect(Ray ray);

    public abstract Color? MapTexture(Vector3 point);
}