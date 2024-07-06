using OpenTK.Mathematics;
public class Utils
{

    public static Color IntToColor(int color)
    {
        float r = ((0x00ff0000 & color) >> 16) / (float)255;
        float g = ((0x0000ff00 & color) >> 8) / (float)255;
        float b = (0x000000ff & color) / (float)255;

        return new Color(r, g, b);
    }

    public static int ColorToInt(Color color)
    {
        int c = 0, temp;

        temp = (int)(color.r * 255.999);
        temp <<= 16;
        c += temp;

        temp = (int)(color.g * 255.999);
        temp <<= 8;
        c += temp;

        temp = (int)(color.b * 255.999f);
        c += temp;

        return c;
    }

    public static Vector3 GetReflectionVector(Vector3 incident, Vector3 normal)
    {
        return incident - 2 * Vector3.Dot(incident, normal) * normal;
    }

    public static float ComputeTriangleArea(Vector3 a, Vector3 b, Vector3 c)
    {
        float hypothenuse = (b - a).Length;
        float side = (c - a).Length / 2.0f;

        float min = MathHelper.Min(side, hypothenuse);
        float max = MathHelper.Max(side, hypothenuse);
        hypothenuse = max;
        side = min;

        float h = (float)MathHelper.Sqrt(hypothenuse * hypothenuse - side * side);
        return 1.0f / 2.0f * side * h;
    }

    public static string GetPathFromSrc(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            path = Path.Combine(new string[] { "../../../", path });
        }
        return path;
    }
}