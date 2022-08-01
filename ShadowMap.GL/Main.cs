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
    private ShadowMap _shadowMap;

    private List<Model> _models;
    private Model _floor;
    
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
        _models = new List<Model>();

        _floor = new Model(cube, new Texture2D("Content/Textures/wood.png"))
        {
            Scale = new Vector3(20, 1, 20)
        };
        
        Random random = Random.Shared;
        Texture2D texture = new Texture2D("Content/Textures/awesomeface.png");
        
        for (int i = 0; i < 20; i++)
        {
            _models.Add(new Model(cube, texture)
            {
                Position = new Vector3(random.NextFloat(-10, 10), random.NextFloat(1f, 1f), random.NextFloat(-10, 10)),
                //Rotation = Quaternion.CreateFromYawPitchRoll(random.NextFloat(-2 * MathF.PI, 2 * MathF.PI), random.NextFloat(-2 * MathF.PI, 2 * MathF.PI), random.NextFloat(-2 * MathF.PI, 2 * MathF.PI))
            });
        }

        Camera = new Camera(new Size(Window.Size.X, Window.Size.Y), new Vector3(0, 2, 0), Quaternion.Identity);
        _shadowMap = new ShadowMap(new Size(1024, 1024));

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

        Vector3 lightPos = new Vector3(1, -1, 1) * 1;

        Gl.Viewport(0, 0, 1024, 1024);
        Matrix4x4 ls = _shadowMap.Use(lightPos);
        Gl.Clear(ClearBufferMask.DepthBufferBit);
        
        Gl.CullFace(CullFaceMode.Back);
        
        foreach (Model model in _models)
            model.DrawShadow(ls, _shadowMap.Effect);
        
        _shadowMap.UnUse();
        
        Gl.CullFace(CullFaceMode.Front);
        
        Gl.Viewport(0, 0, 1280, 720);

        _floor.Draw(Camera, lightPos, ls, _shadowMap);
        
        foreach (Model model in _models)
            model.Draw(Camera, lightPos, ls, _shadowMap);
    }
}