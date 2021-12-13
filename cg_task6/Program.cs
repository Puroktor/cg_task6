using cg_task6;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

NativeWindowSettings settings = new()
{
    Size = new OpenTK.Mathematics.Vector2i(800, 800),
    WindowBorder = WindowBorder.Fixed,
    Title = "CG Task 6",
    NumberOfSamples = 4,

    APIVersion = new(3, 3),
    Flags = ContextFlags.Default,
    Profile = ContextProfile.Compatability,
    API = ContextAPI.OpenGL
};

using (Window window = new(GameWindowSettings.Default, settings))
{
    window.Run();
}