using Cubic.Graphics;
using ShadowMap.Shared;
using Silk.NET.OpenGL;
using static ShadowMap.CubicGraphics.Main;
using PixelFormat = Cubic.Graphics.PixelFormat;
using Texture = Cubic.Graphics.Texture;

namespace ShadowMap.CubicGraphics;

public class Texture2D : IDisposable
{
    public readonly Texture Texture;

    public Texture2D(string path)
    {
        Bitmap bp = new Bitmap(path);
        Texture = Device.CreateTexture((uint) bp.Size.Width, (uint) bp.Size.Height,
            bp.RGBA ? PixelFormat.RGBA8 : PixelFormat.RGB8, TextureSample.Linear, true, wrap: TextureWrap.ClampToEdge);
        Texture.Update(0, 0, (uint) bp.Size.Width, (uint) bp.Size.Height, bp.Data);
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}