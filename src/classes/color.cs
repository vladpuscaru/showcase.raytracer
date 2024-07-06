public class Color
{
    public float r, g, b;

    public static Color COLOR_RAY_NO_INTERSECTION = new Color(1, 1, 0);
    public static Color COLOR_SHADOOW_RAY = new Color(1, 1, 1);

    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
    public Color Clamp(float min, float max)
    {
        r = Math.Clamp(r, min, max);
        g = Math.Clamp(g, min, max);
        b = Math.Clamp(b, min, max);
        return this;
    }
    public static Color operator *(Color c1, Color c2)
    {
        return new Color(c1.r * c2.r, c1.g * c2.g, c1.b * c2.b);
    }

    public static Color operator +(Color c1, Color c2)
    {

        return new Color(c1.r + c2.r, c1.g + c2.g, c1.b + c2.b);
    }

    public static Color operator *(Color c, float s)
    {
        return new Color(c.r * s, c.g * s, c.b * s);
    }

    public static Color operator -(Color c1, Color c2)
    {
        return new Color(c1.r - c2.r, c1.g - c2.g, c1.b - c2.b);
    }
}