using OpenTK.Windowing.GraphicsLibraryFramework;
using Template;
class MyApplication
{
    // member variables
    public Surface screen;
    public RayTracer RayTracer { get; private set; }
    private readonly KeyboardState keyboardState;
    // constructor
    public MyApplication(Surface screen, KeyboardState keyboardState)
    {
        this.screen = screen;
        RayTracer = new RayTracer(screen);
        this.keyboardState = keyboardState;

    }
    // initialize
    public void Init()
    {
    }
    // tick: renders one frame
    public void Tick(double deltaTime)
    {
        RayTracer.HandleInput(keyboardState, deltaTime);
        RayTracer.Render((float)deltaTime);
    }
}