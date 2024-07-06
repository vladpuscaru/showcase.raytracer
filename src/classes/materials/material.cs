using OpenTK.Mathematics;

public class Material
{
    public Color DiffuseColor { get; private set; }
    public Color SpecularColor { get; private set; }
    public float Reflectivity { get; private set; } // the value of the exponent n
    public float SpecularFraction { get; set; }

    public Material(Color diffuseColor, Color specularColor, float reflectivity, float specularFraction)
    {
        DiffuseColor = diffuseColor;
        SpecularColor = specularColor;
        Reflectivity = reflectivity;
        SpecularFraction = specularFraction;
    }

    public Color Shade(Intersection intersection, Light light)
    {
        Color color = new(0, 0, 0);
        Vector3 pointToLight = light.Position - intersection.Point;
        float lightRadius = pointToLight.Length;
        pointToLight = Vector3.Normalize(pointToLight);
        Color lightIntensityAtPoint = light.Intensity * (1 / (lightRadius * lightRadius));
        // diffuse component
        float nDotL = Vector3.Dot(intersection.Normal, pointToLight);
        color += lightIntensityAtPoint * DiffuseColor * MathF.Max(0f, nDotL);

        // specular component if the material is reflective
        if (Reflectivity > 1)
        {
            Vector3 lightReflect = Utils.GetReflectionVector(pointToLight, intersection.Normal);
            Vector3 viewRay = intersection.Ray.Direction;
            float vDotR = Vector3.Dot(viewRay, lightReflect);
            color += lightIntensityAtPoint * SpecularColor * MathF.Pow(MathF.Max(0, vDotR), Reflectivity);
        }

        return color;
    }
}