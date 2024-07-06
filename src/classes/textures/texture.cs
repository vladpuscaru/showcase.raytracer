using OpenTK.Mathematics;
using Template;

public class Texture
{
    public Surface Surface;

    public enum MappingBehaviour
    {
        CLAMP,

        REPEAT,
        MIRROR_REPEAT
    }

    public Texture(string path)
    {
        Surface = new Surface(path);
    }

    /**
     * 𝐹(𝑢,𝑣)=lookuptexel (int)(𝑢×𝑤𝑖𝑑𝑡h),(int)(𝑣×h𝑒𝑖𝑔h𝑡)
     */
    public int LookUp(float u, float v, MappingBehaviour behaviour = MappingBehaviour.CLAMP)
    {
        switch (behaviour)
        {
            case MappingBehaviour.CLAMP:
                u = MathHelper.Min(MathHelper.Max(0, u), 1);
                v = MathHelper.Min(MathHelper.Max(0, v), 1);
                break;
            case MappingBehaviour.REPEAT:
                u = u - (float)MathHelper.Floor(u);
                v = v - (float)MathHelper.Floor(v);
                break;
            case MappingBehaviour.MIRROR_REPEAT:
                /* TODO: Implement this */
                u = MathHelper.Min(MathHelper.Max(0, u), 1);
                v = MathHelper.Min(MathHelper.Max(0, v), 1);
                break;
        }

        int x = (int)(u * (Surface.width - 1));
        int y = (int)(v * (Surface.height - 1));

        return this[x, y];
    }

    public int this[int x, int y]
    {
        get
        {
            return Surface.pixels[y * Surface.width + x];
        }
        set
        {
            Surface.pixels[y * Surface.width + x] = value;
        }
    }
}