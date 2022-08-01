using System;
using System.Drawing;
using System.Numerics;
using ShadowMap.Shared;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace ShadowMapGL;

public class MainWindow : IDisposable
{
    private IWindow _window;
    private static IInputContext _input;
    public static GL Gl;
    public static Camera Camera;

    private Model[] _models;

    public MainWindow()
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 720);
        options.ShouldSwapAutomatically = true;
        options.WindowBorder = WindowBorder.Fixed;
        options.VSync = true;
        _window = Window.Create(options);
        _window.Load += Initialize;
        _window.Update += Update;
        _window.Render += Draw;
    }

    private void Initialize()
    {
        _input = _window.CreateInput();
        Gl = GL.GetApi(_window.GLContext);
        Gl.Enable(EnableCap.DepthTest);
        Gl.DepthFunc(DepthFunction.Lequal);
        Gl.FrontFace(FrontFaceDirection.Ccw);
        Gl.CullFace(CullFaceMode.Front);

        Cube cube = new Cube();
        Texture2D texture = new Texture2D("Content/Textures/awesomeface.png");
        _models = new[]
        {
            new Model(cube, texture)
            {
                Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(70), MathHelper.ToRadians(50), 0)
            }
        };

        Camera = new Camera(new Size(_window.Size.X, _window.Size.Y), new Vector3(0, 0, -5), Quaternion.Identity);
    }

    private void Update(double obj)
    {
        if (IsKeyDown(Key.Escape))
            _window.Close();
    }

    private void Draw(double obj)
    {
        Gl.ClearColor(Color.CornflowerBlue);
        Gl.Clear((uint) ClearBufferMask.ColorBufferBit | (uint) ClearBufferMask.DepthBufferBit | (uint) ClearBufferMask.StencilBufferBit);
        
        Camera.GenerateViewMatrix();
        
        foreach (Model model in _models)
            model.Draw(Camera.ViewMatrix * Camera.ProjectionMatrix, Vector3.One);
    }

    public void Run()
    {
        _window.Run();
    }

    public void Dispose() { }

    public static bool IsKeyDown(Key key)
    {
        return _input.Keyboards[0].IsKeyPressed(key);
    }
}