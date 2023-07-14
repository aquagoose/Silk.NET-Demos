using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace ShadowMap.Shared;

public abstract class MainWindow : IDisposable
{
    protected IWindow Window;

    public MainWindow()
    {
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(1280, 720),
            ShouldSwapAutomatically = true,
            WindowBorder = WindowBorder.Resizable,
            VSync = true,
            Samples = 4
        };

        Window = Silk.NET.Windowing.Window.Create(options);
        Window.Load += Initialize;
        Window.Update += Update;
        Window.Render += Draw;
        
        Window.Resize += OnResize;
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
    
    protected virtual void OnResize(Vector2D<int> obj) { }

    public void Run()
    {
        Window.Run();
    }

    public void Dispose() { }
}