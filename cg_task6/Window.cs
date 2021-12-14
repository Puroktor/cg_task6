using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace cg_task6;

class Window : GameWindow
{
    private List<Triangle> model;
    private Vector4 lightPos = new(-1, 0, 0, 1);
    private Vector4 ambLight = new(1, 1, 1, 1);
    private Vector4 diffLight = new(1, 1, 1, 1);
    private int texture;
    private double frameTime = 0;
    private int fps = 0;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        VSync = VSyncMode.On;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        using Stream stream = File.OpenRead("model.obj");
        model = OBJ.Load(stream);

        GL.ClearColor(Color4.Black);
        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.Lighting);
        GL.Light(LightName.Light0, LightParameter.Position, lightPos);
        GL.Light(LightName.Light0, LightParameter.Ambient, ambLight);
        GL.Light(LightName.Light0, LightParameter.Diffuse, diffLight);
        GL.Enable(EnableCap.Light0);

        LoadTex("model.jpg");
    }

    private void LoadTex(string fileName)
    {
        GL.Enable(EnableCap.Texture2D);
        GL.GenTextures(1, out texture);
        GL.BindTexture(TextureTarget.Texture2D, texture);

        using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(fileName);
        image.Mutate(x => x.Flip(FlipMode.Vertical));
        var pixels = new List<byte>(4 * image.Width * image.Height);

        for (int y = 0; y < image.Height; y++)
        {
            var row = image.GetPixelRowSpan(y);

            for (int x = 0; x < image.Width; x++)
            {
                pixels.Add(row[x].R);
                pixels.Add(row[x].G);
                pixels.Add(row[x].B);
                pixels.Add(row[x].A);
            }
        }
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height,
            0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        frameTime += args.Time;
        fps++;
        if (frameTime >= 1)
        {
            string sep = ", FPS: ";
            Title = Title.Split(sep).First() + sep + fps;
            fps = 0;
            frameTime = 0;
        }

        CheckKeys();

        base.OnUpdateFrame(args);
    }

    private void CheckKeys()
    {
        if (KeyboardState.IsKeyDown(Keys.W))
        {
            GL.Rotate(-1, 1, 0, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.S))
        {
            GL.Rotate(1, 1, 0, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.A))
        {
            GL.Rotate(-1, 0, 1, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.D))
        {
            GL.Rotate(1, 0, 1, 0);
        }
        if (KeyboardState.IsKeyDown(Keys.Q))
        {
            GL.Rotate(-1, 0, 0, 1);
        }
        if (KeyboardState.IsKeyDown(Keys.E))
        {
            GL.Rotate(1, 0, 0, 1);
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.Begin(PrimitiveType.Triangles);
        foreach (var triangle in model)
        {
            for (int i = 0; i < 3; i++)
            {
                GL.Normal3(triangle[i].VN);
                GL.TexCoord3(triangle[i].VT);
                GL.Vertex3(triangle[i].V);
            }
        }
        GL.End();
        SwapBuffers();
        base.OnRenderFrame(args);
    }

    protected override void OnUnload()
    {
        GL.DeleteTexture(texture);
        base.OnUnload();
    }
}
