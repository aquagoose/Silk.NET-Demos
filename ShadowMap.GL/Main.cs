using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ShadowMap.Shared;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace ShadowMapGL;

public class Main : MainWindow
{
    public static GL Gl;
    public static Camera Camera;
    private Vector2 _cameraRot;
    private Vector2 _lastMousePos;

    private List<Model> _models;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        Gl = GL.GetApi(Window.GLContext);
        Gl.Enable(EnableCap.Multisample);
        Gl.Enable(EnableCap.DepthTest);
        Gl.DepthFunc(DepthFunction.Lequal);
        Gl.Enable(EnableCap.CullFace);
        Gl.FrontFace(FrontFaceDirection.Ccw);
        Gl.CullFace(CullFaceMode.Front);

        Cube cube = new Cube();
        Texture2D texture = new Texture2D("Content/Textures/awesomeface.png");
        _models = new List<Model>()
        {
            new Model(cube, texture)
            {
                Scale = new Vector3(10, 1, 10)
            }
        };
        
        Random random = Random.Shared;

        for (int i = 0; i < 10; i++)
        {
            _models.Add(new Model(cube, texture)
            {
                Position = new Vector3(random.NextFloat(-5, 5), random.NextFloat(1, 3), random.NextFloat(-5, 5)),
                Rotation = Quaternion.CreateFromYawPitchRoll(random.NextFloat(-2 * MathF.PI, 2 * MathF.PI), random.NextFloat(-2 * MathF.PI, 2 * MathF.PI), random.NextFloat(-2 * MathF.PI, 2 * MathF.PI))
            });
        }

        Camera = new Camera(new Size(Window.Size.X, Window.Size.Y), new Vector3(0, 2, 0), Quaternion.Identity);

        Input.MouseVisible = false;
    }

    protected override void Update(double obj)
    {
        if (Input.KeyPressed(Key.Escape))
            if (!Input.MouseVisible)
                Input.MouseVisible = true;
            else 
                Window.Close();

        if (Input.KeyPressed(Key.M))
            Input.MouseVisible = !Input.MouseVisible;

        const float sensitivity = 0.01f;
        Vector2 deltaPos = Input.MouseVisible ? Vector2.Zero : Input.MousePosition - _lastMousePos;
        _cameraRot.X -= deltaPos.X * sensitivity;
        _cameraRot.Y += deltaPos.Y * sensitivity;
        _cameraRot.Y = MathHelper.Clamp(_cameraRot.Y, -MathF.PI / 2, MathF.PI / 2);
        Camera.Rotation = Quaternion.CreateFromYawPitchRoll(_cameraRot.X, _cameraRot.Y, 0);
        const float cameraSpeed = 20;
        if (Input.KeyDown(Key.W))
            Camera.Position += Camera.Forward * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.S))
            Camera.Position -= Camera.Forward * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.A))
            Camera.Position += Camera.Right * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.D))
            Camera.Position -= Camera.Right * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.E))
            Camera.Position += Camera.Up * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.Q))
            Camera.Position -= Camera.Up * cameraSpeed * (float) obj;

        _lastMousePos = Input.MousePosition;
        
        base.Update(obj);
    }

    protected override void Draw(double obj)
    {
        base.Draw(obj);
        
        Gl.ClearColor(Color.CornflowerBlue);
        Gl.Clear((uint) ClearBufferMask.ColorBufferBit | (uint) ClearBufferMask.DepthBufferBit | (uint) ClearBufferMask.StencilBufferBit);
        
        Camera.GenerateViewMatrix();
        
        foreach (Model model in _models)
            model.Draw(Camera, new Vector3(1, -1, 1));
    }
}