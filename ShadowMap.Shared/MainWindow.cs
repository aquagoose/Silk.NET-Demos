using System;
using System.Drawing;
using System.Numerics;
using ShadowMap.Shared;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace ShadowMap.Shared;

public abstract class MainWindow : IDisposable
{
    protected IWindow Window;

    public MainWindow()
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 720);
        options.ShouldSwapAutomatically = true;
        options.WindowBorder = WindowBorder.Fixed;
        options.VSync = true;
        options.Samples = 4;
        Window = Silk.NET.Windowing.Window.Create(options);
        Window.Load += Initialize;
        Window.Update += Update;
        Window.Render += Draw;
    }

    protected virtual void Initialize()
    {
        Input.Initialize(Window.CreateInput());
    }

    protected virtual void Update(double obj)
    {
        Input.Update();
    }

    protected virtual void Draw(double obj) { }

    public void Run()
    {
        Window.Run();
    }

    public void Dispose() { }
}