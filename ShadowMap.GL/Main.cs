using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ShadowMap.Shared;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace ShadowMapGL;

public class Main : MainWindow
{
    public static GL Gl;
    private Camera _camera;
    private Vector2 _cameraRot;
    private Vector2 _lastMousePos;
    private ShadowMap _shadowMap;

    private List<Model> _models;
    private Model _floor;

    private Cube _cube;
    private Texture2D _texture;
    
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

        _cube = new Cube();
        _models = new List<Model>();

        _floor = new Model(_cube, new Texture2D("Content/Textures/wood.png"))
        {
            Scale = new Vector3(14, 1, 14)
        };
        
        _texture = new Texture2D("Content/Textures/awesomeface.png");

        AddModels(true);

        _camera = new Camera(new Size(Window.Size.X, Window.Size.Y), new Vector3(9.101653f, 11.751301f, -12.974878f),
            new Quaternion(-0.3225966f, 0.28561804f, -0.10277174f, -0.8965443f));
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
        _camera.Rotation = Quaternion.CreateFromYawPitchRoll(_cameraRot.X, _cameraRot.Y, 0);
        const float cameraSpeed = 20;
        if (Input.KeyDown(Key.W))
            _camera.Position += _camera.Forward * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.S))
            _camera.Position -= _camera.Forward * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.A))
            _camera.Position += _camera.Right * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.D))
            _camera.Position -= _camera.Right * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.E))
            _camera.Position += _camera.Up * cameraSpeed * (float) obj;
        if (Input.KeyDown(Key.Q))
            _camera.Position -= _camera.Up * cameraSpeed * (float) obj;

        _lastMousePos = Input.MousePosition;

        if (Input.KeyPressed(Key.R))
        {
            _models.Clear();
            AddModels(false);
        }

        if (Input.KeyPressed(Key.T))
        {
            _models.Clear();
            AddModels(true);
        }

        base.Update(obj);
    }

    protected override void Draw(double obj)
    {
        base.Draw(obj);
        
        Gl.ClearColor(Color.CornflowerBlue);
        Gl.Clear((uint) ClearBufferMask.ColorBufferBit | (uint) ClearBufferMask.DepthBufferBit | (uint) ClearBufferMask.StencilBufferBit);
        
        _camera.GenerateViewMatrix();

        Vector3 lightPos = new Vector3(1, -1, 1) * 1;

        Gl.Viewport(0, 0, (uint) _shadowMap.Size.Width, (uint) _shadowMap.Size.Height);
        Matrix4x4 ls = _shadowMap.Use(lightPos);
        Gl.Clear(ClearBufferMask.DepthBufferBit);
        
        Gl.CullFace(CullFaceMode.Back);
        
        foreach (Model model in _models)
            model.DrawShadow(ls, _shadowMap.Effect);
        
        _shadowMap.UnUse();
        
        Gl.CullFace(CullFaceMode.Front);
        
        Gl.Viewport(0, 0, (uint) Window.Size.X, (uint) Window.Size.Y);

        _floor.Draw(_camera, lightPos, ls, _shadowMap);
        
        foreach (Model model in _models)
            model.Draw(_camera, lightPos, ls, _shadowMap);
    }

    private void AddModels(bool randomizeRot)
    {
        Random random = Random.Shared;
        
        for (int i = 0; i < 20; i++)
        {
            _models.Add(new Model(_cube, _texture)
            {
                Position = new Vector3(random.NextFloat(-7, 7), randomizeRot ? random.NextFloat(1f, 5f) : 1, random.NextFloat(-7, 7)),
                Rotation = randomizeRot ? Quaternion.CreateFromYawPitchRoll(random.NextFloat(-2 * MathF.PI, 2 * MathF.PI), random.NextFloat(-2 * MathF.PI, 2 * MathF.PI), random.NextFloat(-2 * MathF.PI, 2 * MathF.PI)) : Quaternion.Identity
            });
        }
    }

    protected override void OnResize(Vector2D<int> obj)
    {
        base.OnResize(obj);
        
        Gl.Viewport(0, 0, (uint) obj.X, (uint) obj.Y);
        _camera.GenerateProjectionMatrix(new Size(obj.X, obj.Y));
    }
}