using System.Drawing;
using StbImageSharp;

namespace ShadowMap.Shared;

public struct Bitmap
{
    public readonly byte[] Data;
    public readonly Size Size;
    public readonly bool RGBA;

    public Bitmap(string path)
    {
        ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(path));
        Data = result.Data;
        Size = new Size(result.Width, result.Height);
        RGBA = result.Comp == ColorComponents.RedGreenBlueAlpha;
    }
}