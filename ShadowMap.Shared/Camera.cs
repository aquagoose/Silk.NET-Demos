using System.Drawing;
using System.Numerics;

namespace ShadowMap.Shared;

public class Camera
{
    private float _fov;
    private float _near;
    private float _far;

    public Vector3 Position;
    public Quaternion Rotation;
    
    public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, Rotation);
    
    public Vector3 Up => Vector3.Transform(Vector3.UnitY, Rotation);
    
    public Vector3 Right => Vector3.Transform(Vector3.UnitX, Rotation);

    public Matrix4x4 ProjectionMatrix { get; private set; }
    
    public Matrix4x4 ViewMatrix { get; private set; }

    public Camera(Size winSize, Vector3 position, Quaternion rotation)
    {
        // 45deg
        _fov = MathF.PI / 4;
        _near = 0.1f;
        _far = 1000f;
        Position = position;
        Rotation = rotation;
        GenerateProjectionMatrix(winSize);
    }

    public void GenerateViewMatrix()
    {
        ViewMatrix = Matrix4x4.CreateLookAt(Position, Position + Forward, Up);
    }

    public void GenerateProjectionMatrix(Size winSize)
    {
        ProjectionMatrix =
            Matrix4x4.CreatePerspectiveFieldOfView(_fov, winSize.Width / (float) winSize.Height, _near, _far);
    }
}