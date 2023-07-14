using System;
using System.IO;
using System.Numerics;
using Silk.NET.OpenGL;
using static ShadowMapGL.Main;

namespace ShadowMapGL;

public class Effect : IDisposable
{
    public readonly uint Handle;
    
    public Effect(string vertex, string fragment)
    {
        uint vShader = Gl.CreateShader(ShaderType.VertexShader);
        uint fShader = Gl.CreateShader(ShaderType.FragmentShader);

        string shader = File.ReadAllText(vertex);
        shader = shader.Insert(0, "#version 330 core\n");
        Gl.ShaderSource(vShader, shader);
        shader = File.ReadAllText(fragment);
        shader = shader.Insert(0, "#version 330 core\n");
        Gl.ShaderSource(fShader, shader);
        
        CompileShader(vShader);
        CompileShader(fShader);

        Handle = Gl.CreateProgram();
        Gl.AttachShader(Handle, vShader);
        Gl.AttachShader(Handle, fShader);
        Gl.LinkProgram(Handle);
        Gl.GetProgram(Handle, ProgramPropertyARB.LinkStatus, out int status);
        if (status != (int) GLEnum.True)
            throw new Exception($"Program {Handle} failed to link: {Gl.GetProgramInfoLog(Handle)}");
        Gl.DetachShader(Handle, vShader);
        Gl.DetachShader(Handle, fShader);
        Gl.DeleteShader(vShader);
        Gl.DeleteShader(fShader);
    }

    private void CompileShader(uint shader)
    {
        Gl.CompileShader(shader);
        
        Gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
        if (status != (int) GLEnum.True)
            throw new Exception($"Shader {shader} failed to compile: {Gl.GetShaderInfoLog(shader)}");
    }

    public uint GetAttribLocation(string name) => (uint) Gl.GetAttribLocation(Handle, name);

    public void SetUniform(string name, int value)
    {
        int location = Gl.GetUniformLocation(Handle, name);
        Gl.Uniform1(location, value);
    }
    
    public void SetUniform(string name, Vector3 value)
    {
        int location = Gl.GetUniformLocation(Handle, name);
        Gl.Uniform3(location, value);
    }
    
    public void SetUniform(string name, Vector4 value)
    {
        int location = Gl.GetUniformLocation(Handle, name);
        Gl.Uniform4(location, value);
    }

    public unsafe void SetUniform(string name, Matrix4x4 value, bool transpose = true)
    {
        int location = Gl.GetUniformLocation(Handle, name);
        Gl.UniformMatrix4(location, 1, transpose, (float*) &value);
    }

    public void Use()
    {
        Gl.UseProgram(Handle);
    }

    public static void UnUse()
    {
        Gl.UseProgram(0);
    }

    public void Dispose()
    {
        Gl.DeleteProgram(Handle);
    }
}