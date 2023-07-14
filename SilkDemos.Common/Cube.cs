using System.Numerics;

namespace ShadowMap.Shared;

// https://github.com/IsometricSoftware/Cubic/blob/Cubic.Next/Cubic/Primitives/Cube.cs
public struct Cube : IPrimitive
{
    public VertexPositionTextureNormal[] Vertices => new[]
    {
        new VertexPositionTextureNormal(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 0), new Vector3(0, 1, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0), new Vector3(0, 1, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1), new Vector3(0, 1, 0)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1), new Vector3(0, 1, 0)),

        new VertexPositionTextureNormal(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 0), new Vector3(0, -1, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 0), new Vector3(0, -1, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 1), new Vector3(0, -1, 0)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1), new Vector3(0, -1, 0)),

        new VertexPositionTextureNormal(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 0), new Vector3(-1, 0, 0)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 0), new Vector3(-1, 0, 0)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(1, 1), new Vector3(-1, 0, 0)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1), new Vector3(-1, 0, 0)),

        new VertexPositionTextureNormal(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(0, 0), new Vector3(1, 0, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0), new Vector3(1, 0, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 1), new Vector3(1, 0, 0)),
        new VertexPositionTextureNormal(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 1), new Vector3(1, 0, 0)),

        new VertexPositionTextureNormal(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(0, 0), new Vector3(0, 0, -1)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1, 0), new Vector3(0, 0, -1)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1, 1), new Vector3(0, 0, -1)),
        new VertexPositionTextureNormal(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0, 1), new Vector3(0, 0, -1)),

        new VertexPositionTextureNormal(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 0), new Vector3(0, 0, 1)),
        new VertexPositionTextureNormal(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 0), new Vector3(0, 0, 1)),
        new VertexPositionTextureNormal(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 1), new Vector3(0, 0, 1)),
        new VertexPositionTextureNormal(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1), new Vector3(0, 0, 1))
    };

    public uint[] Indices => new uint[]
    {
        0, 1, 2, 0, 2, 3,
        4, 5, 6, 4, 6, 7,
        8, 9, 10, 8, 10, 11,
        12, 13, 14, 12, 14, 15,
        16, 17, 18, 16, 18, 19,
        20, 21, 22, 20, 22, 23
    };
}