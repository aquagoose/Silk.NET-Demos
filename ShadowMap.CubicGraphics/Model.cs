using System.Numerics;
using Cubic.Graphics;
using ShadowMap.Shared;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using static ShadowMap.CubicGraphics.Main;
using Buffer = Cubic.Graphics.Buffer;

namespace ShadowMap.CubicGraphics;

public class Model : IDisposable
{
    private Buffer _vertexBuffer;
    private Buffer _indexBuffer;

    private Texture2D _texture;
    private Effect _effect;
    private uint _numIndices;

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    private ShaderLayout[] _layout;
    
    public Model(IPrimitive primitive, Texture2D texture)
    {
        _texture = texture;
        _numIndices = (uint) primitive.Indices.Length;
        
        Position = Vector3.Zero;
        Rotation = Quaternion.Identity;
        Scale = Vector3.One;

        _vertexBuffer = Device.CreateBuffer(BufferType.VertexBuffer,
            (uint) primitive.Vertices.Length * VertexPositionTextureNormal.SizeInBytes);
        _vertexBuffer.Update(0, primitive.Vertices);

        _indexBuffer = Device.CreateBuffer(BufferType.IndexBuffer, (uint) primitive.Indices.Length * sizeof(uint));
        _indexBuffer.Update(0, primitive.Indices);

        _effect = new Effect("Content/Shaders/Model.vert", "Content/Shaders/Model.frag");

        const uint stride = 32;

        _layout = new[]
        {
            new ShaderLayout("aPosition", 3, AttribType.Float),
            new ShaderLayout("aTexCoords", 2, AttribType.Float),
            new ShaderLayout("aNormals", 3, AttribType.Float)
        };
    }

    public void Draw(Camera camera, Vector3 lightPos, Matrix4x4 lightSpace, ShadowMap map)
    {
        // Model & camera matrices
        _effect.SetUniform("uModel",
            Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateFromQuaternion(Rotation) *
            Matrix4x4.CreateTranslation(Position));
        if (Input.KeyDown(Key.C))
        {
            _effect.SetUniform("uCamera", lightSpace);
        }
        else
            _effect.SetUniform("uCamera", camera.ViewMatrix * camera.ProjectionMatrix);
        _effect.SetUniform("uCameraPos", camera.Position);
        
        // Material
        _effect.SetUniform("uMaterial.albedo", 0);
        _effect.SetUniform("uMaterial.specular", 1);
        _effect.SetUniform("uMaterial.color", Vector4.One);
        _effect.SetUniform("uMaterial.shininess", 32);
        
        // Light
        _effect.SetUniform("uSun.direction", lightPos);
        _effect.SetUniform("uSun.ambient", new Vector3(0.1f));
        _effect.SetUniform("uSun.diffuse", new Vector3(0.7f));
        _effect.SetUniform("uSun.specular", new Vector3(1.0f));
        _effect.SetUniform("uLightSpace", lightSpace);
        
        Device.SetTexture(0, _texture.Texture);
        Device.SetTexture(1, _texture.Texture);
        
        _effect.SetUniform("uShadowMap", 2);
        map.Bind(2);
        
        Device.SetShader(_effect.Shader);
        Device.SetVertexBuffer(_vertexBuffer, VertexPositionTextureNormal.SizeInBytes, _layout);
        Device.SetIndexBuffer(_indexBuffer);
        Device.Draw(_numIndices);
    }

    public void DrawShadow(Matrix4x4 lightSpace, Effect effect)
    {
        effect.SetUniform("uLightSpace", lightSpace);
        effect.SetUniform("uModel", Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateFromQuaternion(Rotation) *
                                    Matrix4x4.CreateTranslation(Position));
        
        Device.SetShader(effect.Shader);
        Device.SetVertexBuffer(_vertexBuffer, VertexPositionTextureNormal.SizeInBytes, _layout);
        Device.SetIndexBuffer(_indexBuffer);
        
        Device.Draw(_numIndices);
    }

    public void Dispose()
    {
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _effect.Dispose();
    }
}