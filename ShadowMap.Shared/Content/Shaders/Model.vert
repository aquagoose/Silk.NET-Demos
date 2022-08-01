#version 330 core

in vec3 aPosition;
in vec2 aTexCoords;
in vec3 aNormals;

out vec2 frag_texCoords;
out vec3 frag_normal;
out vec3 frag_position;

uniform mat4 uModel;
uniform mat4 uCamera;

void main() 
{
    frag_texCoords = aTexCoords;
    frag_position = vec3(vec4(aPosition, 1.0) * uModel);
    gl_Position = vec4(frag_position, 1.0) * uCamera;
    frag_normal = aNormals * mat3(transpose(inverse(uModel)));
}