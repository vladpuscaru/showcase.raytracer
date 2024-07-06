using OpenTK.Mathematics;

public class Ray
{
    public Vector3 Origin { get; set; }
    public Vector3 Direction { get; set; }

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = Vector3.Normalize(direction);
    }

    public Vector3 GetPointAt(float t)
    {
        return Origin + Direction * t;
    }
}