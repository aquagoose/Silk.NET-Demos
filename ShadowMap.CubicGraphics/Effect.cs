using System.Numerics;
using Cubic.Graphics;
using Silk.NET.OpenGL;
using static ShadowMap.CubicGraphics.Main;
using Shader = Cubic.Graphics.Shader;

namespace ShadowMap.CubicGraphics;

public class Effect : IDisposable
{
    public readonly Shader Shader;
    
    public Effect(string vertex, string fragment)
    {
        Shader = Device.CreateShader(new ShaderAttachment(AttachmentType.Vertex, File.ReadAllText(vertex)),
            new ShaderAttachment(AttachmentType.Fragment, File.ReadAllText(fragment)));
    }

    public void SetUniform(string name, int value) => Shader.SetUniform(name, value);

    public void SetUniform(string name, Vector3 value) => Shader.SetUniform(name, value);

    public void SetUniform(string name, Vector4 value) => Shader.SetUniform(name, value);

    public void SetUniform(string name, Matrix4x4 value, bool transpose = true) =>
        Shader.SetUniform(name, value, transpose);

    public void Dispose()
    {
        Shader.Dispose();
    }
}