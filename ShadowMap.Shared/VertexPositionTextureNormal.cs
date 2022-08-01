using System.Numerics;

namespace ShadowMap.Shared;

// https://github.com/IsometricSoftware/Cubic/blob/Cubic.Next/Cubic/Render/VertexPositionTextureNormal.cs
public struct VertexPositionTextureNormal
{
    public Vector3 Position;
    public Vector2 TexCoords;
    public Vector3 Normals;

    public VertexPositionTextureNormal(Vector3 position, Vector2 texCoords, Vector3 normals)
    {
        Position = position;
        TexCoords = texCoords;
        Normals = normals;
    }
}