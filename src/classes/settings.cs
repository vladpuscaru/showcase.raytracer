class Settings
{
    public static bool DEBUG_SCREEN = true;
    public static int ACTIVE_SCENE = 0;
    // n ray samples used for anti-aliasing, per (2d) axis. 1 = no anti-aliasing, aka using 1 ray per pixel.
    // Total number of rays per pixel is N_RAY_SAMPLES_PER_PX_AXIS squared. 
    // Reason for not making this variable the total number of samples is to avoid a square root operation in Raytracer.Render().
    public static int N_RAY_SAMPLES_PER_PX_AXIS = 2;
}