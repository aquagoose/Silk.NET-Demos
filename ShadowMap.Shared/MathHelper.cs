namespace ShadowMap.Shared;

public static class MathHelper
{
    public static float ToRadians(float degrees) => degrees * (MathF.PI / 180);

    public static float ToDegrees(float radians) => radians * (180 / MathF.PI);

    public static float Clamp(float value, float min, float max) => value <= min ? min : value >= max ? max : value;
}