namespace ShadowMap.Shared;

// https://github.com/IsometricSoftware/Cubic/blob/Cubic.Next/Cubic/Primitives/IPrimitive.cs
public interface IPrimitive
{
    public VertexPositionTextureNormal[] Vertices { get; }
    
    public uint[] Indices { get; }
}