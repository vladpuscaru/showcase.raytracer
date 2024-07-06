
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

public class Light
{
    public Vector3 Position { get; set; }
    public Color Intensity { get; set; }
    public static Color AmbientColor = new(0.1f, 0.1f, 0.1f);
    private const float moveSpeed = 1.5f;

    public Light(Vector3 position, Color intensity)
    {
        Position = position;
        Intensity = intensity;
    }

    public void MoveRight(float deltaTime) { Position += new Vector3(1, 0, 0) * moveSpeed * deltaTime; }
    public void MoveLeft(float deltaTime) { Position += new Vector3(-1, 0, 0) * moveSpeed * deltaTime; }

    public void MoveForward(float deltaTime) { Position += new Vector3(0, 0, -1) * moveSpeed * deltaTime; }
    public void MoveBackward(float deltaTime) { Position += new Vector3(0, 0, 1) * moveSpeed * deltaTime; }

    public void MoveUp(float deltaTime) { Position += new Vector3(0, 1, 0) * moveSpeed * deltaTime; }
    public void MoveDown(float deltaTime) { Position += new Vector3(0, -1, 0) * moveSpeed * deltaTime; }

    public void HandleInput(KeyboardState keyboardState, float deltaTime)
    {
        if (keyboardState[Keys.I]) MoveForward(deltaTime);
        if (keyboardState[Keys.K]) MoveBackward(deltaTime);
        if (keyboardState[Keys.J]) MoveLeft(deltaTime);
        if (keyboardState[Keys.L]) MoveRight(deltaTime);
        if (keyboardState[Keys.U]) MoveUp(deltaTime);
        if (keyboardState[Keys.N]) MoveDown(deltaTime);
    }
}