using System;
using System.Drawing;
using System.Numerics;
using Silk.NET.OpenGL;
using static ShadowMapGL.Main;

namespace ShadowMapGL;

public class ShadowMap : IDisposable
{
    private readonly uint _fbo;
    public readonly uint DepthMap;

    public readonly Effect Effect;
    
    public readonly Size Size;
    
    public unsafe ShadowMap(Size size)
    {
        Size = size;
        
        _fbo = Gl.GenFramebuffer();
        DepthMap = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, DepthMap);
        Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.DepthComponent, (uint) size.Width, (uint) size.Height,
            0, PixelFormat.DepthComponent, PixelType.Float, null);
        Gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureMinFilter, (int) TextureMinFilter.Nearest);
        Gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int) TextureMagFilter.Nearest);
        Gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureWrapS, (int) TextureWrapMode.ClampToBorder);
        Gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureWrapT, (int) TextureWrapMode.ClampToBorder);
        fixed (float* color = new float[] { 1.0f, 1.0f, 1.0f, 1.0f })
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, color);
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
            TextureTarget.Texture2D, DepthMap, 0);
        Gl.DrawBuffer(DrawBufferMode.None);
        Gl.ReadBuffer(GLEnum.None);
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        Effect = new Effect("Shaders/Shadow.vert", "Shaders/Shadow.frag");
    }

    public Matrix4x4 Use(Vector3 lightPos)
    {
        Matrix4x4 projection = Matrix4x4.CreateOrthographicOffCenter(-10.0f, 10.0f, -10.0f, 10.0f, 1.0f, 10.0f);
        Matrix4x4 view = Matrix4x4.CreateLookAt(-lightPos, Vector3.Zero, Vector3.UnitY);
        Matrix4x4 lightSpace = view * projection;
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        return lightSpace;
    }

    public void UnUse()
    {
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Bind(int textureUnit)
    {
        Gl.ActiveTexture(TextureUnit.Texture0 + textureUnit);
        Gl.BindTexture(TextureTarget.Texture2D, DepthMap);
    }
    
    public void Dispose() { }
}