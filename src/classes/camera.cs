using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Template;

public class Camera
{
    // move speed in units per second
    private const float moveSpeed = 1.5f;
    // rotate speed in degrees per second
    private const float rotateSpeedDeg = 45.0f;

    // fov update per second, in degrees
    private const float fovUpdateRate = 2.0f;

    public float FocalLength { get; set; }
    public float AspectRatio { get; set; }
    public float FOV { get; set; }

    public float ViewportHeight { get; set; }
    public float ViewportWidth { get; set; }

    public Vector3 Position { get; set; }
    public Vector3 Up { get; set; }
    public Vector3 LookAt { get; set; }
    public Vector3 Right { get; set; }

    /// <summary>
    /// Constructor for the Camera class
    /// </summary>
    /// <param name="surface">The surface object to get the dimensions from</param>
    /// <param name="position">The position of the camera</param>
    /// <param name="focalLength">The focal length of the camera (default 1.0f)</param>
    /// <param name="fovAngleDeg">The field of view angle in degrees (default 45.0f)</param>
    /// <param name="debug">Whether the camera is in debug mode (default false)</param>
    public Camera(Surface surface, Vector3 position, float focalLength = 1f, float fovAngleDeg = 45f, bool debug = false)
    {
        // Set the position of the camera
        Position = position;
        // Set the initial vectors
        Right = Vector3.UnitX;
        Up = Vector3.UnitY;
        LookAt = -Vector3.UnitZ;

        // Set the field of view angle
        FOV = fovAngleDeg;

        // Set the focal length
        FocalLength = focalLength;
        float fovRads = MathHelper.DegreesToRadians(fovAngleDeg);

        // Get the width and height of the surface
        int px_width = surface.width;
        int px_height = surface.height;

        // If in debug mode, halve the width
        if (debug) { px_width /= 2; }
        // Calculate the aspect ratio
        AspectRatio = (float)px_width / px_height;

        // Calculate the viewport height and width
        ViewportHeight = 2 * (float)MathHelper.Tan(fovRads) * FocalLength;
        ViewportWidth = (float)ViewportHeight * AspectRatio;
    }

    public static Vector2i FromNDCToPixels(Vector2 screenCoords, Vector2 vwSize, int pxWidth, int pxHeight)
    {
        // Get the width and height of the viewport in NDC (normalized device coordinates)
        float vwWidth = vwSize.X;
        float vwHeight = vwSize.Y;

        // Calculate the x,y coordinate ratio's between 0 and 1
        float xRatio = (screenCoords.X + vwWidth - vwWidth / 2) / vwWidth;
        float yRatio = (screenCoords.Y + vwHeight - vwHeight / 2) / vwHeight;

        // Calculate the x,y coordinates in pixels
        int x = (int)(xRatio * pxWidth);
        int y = (int)(yRatio * pxHeight);

        return new Vector2i(x, y);
    }

    public void StrafeLeft(double deltaTime) { Position += -Right * moveSpeed * (float)deltaTime; }
    public void StrafeRight(double deltaTime) { Position += Right * moveSpeed * (float)deltaTime; }
    public void GoForward(double deltaTime) { Position += LookAt * moveSpeed * (float)deltaTime; }
    public void GoBackwards(double deltaTime) { Position += -LookAt * moveSpeed * (float)deltaTime; }
    public void MoveUp(double deltaTime) { Position += Up * moveSpeed * (float)deltaTime; }
    public void MoveDown(double deltaTime) { Position -= Up * moveSpeed * (float)deltaTime; }

    public void UpdateFOV(double deltaTime, bool increase = true)
    {
        float fovUpdateRateDirection = increase ? fovUpdateRate : -fovUpdateRate;
        FOV += fovUpdateRateDirection * (float)deltaTime;
        float fovRads = MathHelper.DegreesToRadians(FOV);
        ViewportHeight = 2 * (float)MathHelper.Tan(fovRads) * FocalLength;
        ViewportWidth = (float)ViewportHeight * AspectRatio;
    }

    /**
     * Rotate about X-axis
     * Rotate LookAt and Up vectors
     * The Right vector doesn't need modification as it's still gonna be orthogonal
     */
    public void Pitch(float angleDeg)
    {
        float angleRad = MathHelper.DegreesToRadians(angleDeg);
        float cos = (float)MathHelper.Cos(angleRad);
        float sin = (float)MathHelper.Sin(angleRad);

        // Rotate LookAt
        float newZ = cos * LookAt.Z - sin * LookAt.Y;
        float newY = sin * LookAt.Z + cos * LookAt.Y;

        LookAt = new Vector3(LookAt.X, newY, newZ);

        // Rotate Up
        newZ = cos * Up.Z - sin * Up.Y;
        newY = sin * Up.Z + cos * Up.Y;

        Up = new Vector3(Up.X, newY, newZ);
    }

    /**
     * Rotate about Y-axis
     * Rotate LookAt and Right vectors
     * The Up vector doesn't need modification as it's still gonna be orthogonal
     */
    public void Yaw(float angleDeg)
    {
        float angleRad = MathHelper.DegreesToRadians(angleDeg);
        float cos = (float)MathHelper.Cos(angleRad);
        float sin = (float)MathHelper.Sin(angleRad);

        // Rotate LookAt
        float newX = cos * LookAt.X - sin * LookAt.Z;
        float newZ = sin * LookAt.X + cos * LookAt.Z;

        LookAt = new Vector3(newX, LookAt.Y, newZ);

        // Rotate Up
        newX = cos * Right.X - sin * Right.Z;
        newZ = sin * Right.X + cos * Right.Z;

        Right = new Vector3(newX, Right.Y, newZ);
    }

    public void HandleInput(KeyboardState keyboardState, double deltaTime)
    {
        // Movement
        if (keyboardState[Keys.W]) GoForward(deltaTime);
        if (keyboardState[Keys.S]) GoBackwards(deltaTime);
        if (keyboardState[Keys.A]) StrafeLeft(deltaTime);
        if (keyboardState[Keys.D]) StrafeRight(deltaTime);
        if (keyboardState[Keys.E]) MoveUp(deltaTime);
        if (keyboardState[Keys.C]) MoveDown(deltaTime);

        // rotation about Y
        if (keyboardState[Keys.Right]) Yaw(rotateSpeedDeg * (float)deltaTime);
        if (keyboardState[Keys.Left]) Yaw(-rotateSpeedDeg * (float)deltaTime);

        // pitch buggy
        if (keyboardState[Keys.Down]) Pitch(rotateSpeedDeg * (float)deltaTime);
        if (keyboardState[Keys.Up]) Pitch(-rotateSpeedDeg * (float)deltaTime);

        // FOV
        if (keyboardState[Keys.B]) UpdateFOV(deltaTime, true);
        if (keyboardState[Keys.V]) UpdateFOV(deltaTime, false);

        // Focal length
        if (keyboardState[Keys.F]) FocalLength -= 0.5f * (float)deltaTime;
        if (keyboardState[Keys.G]) FocalLength += 0.5f * (float)deltaTime;
    }
}