using System.Drawing;
using System.Numerics;
using Cubic.Graphics;
using Silk.NET.OpenGL;
using Texture = Cubic.Graphics.Texture;
using static ShadowMap.CubicGraphics.Main;
using Framebuffer = Cubic.Graphics.Framebuffer;
using PixelFormat = Cubic.Graphics.PixelFormat;

namespace ShadowMap.CubicGraphics;

public class ShadowMap : IDisposable
{
    private readonly Framebuffer _framebuffer;
    public readonly Texture DepthMap;

    public readonly Effect Effect;

    public readonly Size Size;
    
    public ShadowMap(Size size)
    {
        Size = size;

        _framebuffer = Device.CreateFramebuffer();
        DepthMap = Device.CreateTexture((uint) size.Width, (uint) size.Height, PixelFormat.DepthOnly,
            TextureSample.Nearest, false, TextureUsage.Framebuffer);
        DepthMap.BorderColor = Color.White;
        _framebuffer.AttachTexture(DepthMap);

        Effect = new Effect("Content/Shaders/Shadow.vert", "Content/Shaders/Shadow.frag");
    }

    public Matrix4x4 Use(Vector3 lightPos)
    {
        Matrix4x4 projection = Matrix4x4.CreateOrthographicOffCenter(-10.0f, 10.0f, -10.0f, 10.0f, 1.0f, 10.0f);
        Matrix4x4 view = Matrix4x4.CreateLookAt(-lightPos, Vector3.Zero, Vector3.UnitY);
        Matrix4x4 lightSpace = view * projection;
        Device.SetFramebuffer(_framebuffer);
        return lightSpace;
    }

    public void UnUse()
    {
        Device.SetFramebuffer(null);
    }

    public void Bind(int textureUnit)
    {
        Device.SetTexture((uint) textureUnit, DepthMap);
    }
    
    public void Dispose() { }
}