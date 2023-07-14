in vec3 aPosition;
in vec2 aTexCoords;
in vec3 aNormals;

out vec2 frag_texCoords;
out vec3 frag_normal;
out vec3 frag_position;
out vec4 frag_lightSpace;

uniform mat4 uModel;
uniform mat4 uCamera;
uniform mat4 uLightSpace;

void main() 
{
    frag_texCoords = aTexCoords;
    frag_position = vec3(vec4(aPosition, 1.0) * uModel);
    frag_lightSpace = vec4(frag_position, 1.0) * uLightSpace;
    gl_Position = vec4(frag_position, 1.0) * uCamera;
    frag_normal = aNormals * mat3(transpose(inverse(uModel)));
}