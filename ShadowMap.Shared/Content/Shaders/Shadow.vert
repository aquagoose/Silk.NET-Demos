#version 330 core

in vec3 aPosition;
in vec2 aTexCoords;
in vec3 aNormals;

uniform mat4 uLightSpace;
uniform mat4 uModel;

void main() 
{
    gl_Position = vec4(aPosition, 1.0) * uModel * uLightSpace;
}
