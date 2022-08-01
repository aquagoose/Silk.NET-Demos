#version 330 core

struct Material
{
    sampler2D albedo;
    sampler2D specular;
    vec4 color;
    int shininess;
};

struct DirectionalLight
{
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec2 frag_texCoords;
in vec3 frag_normal;
in vec3 frag_position;
out vec4 out_color;

uniform DirectionalLight uSun;
uniform Material uMaterial;
uniform vec3 uCameraPos;

vec4 CalculateDirectional(DirectionalLight light, vec3 normal, vec3 viewDir);

void main()
{
    vec3 norm = normalize(frag_normal);
    vec3 viewDir = normalize(uCameraPos - frag_position);

    vec4 result = CalculateDirectional(uSun, norm, viewDir);
    out_color = result * uMaterial.color;
}

vec4 CalculateDirectional(DirectionalLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), uMaterial.shininess);
    vec4 alRes = texture(uMaterial.albedo, frag_texCoords);
    vec3 ambient = light.ambient * vec3(alRes);
    vec3 diffuse = light.diffuse * diff * vec3(alRes);
    vec3 specular = light.specular * spec * vec3(texture(uMaterial.specular, frag_texCoords));
    return vec4(ambient + diffuse + specular, alRes.a);
}