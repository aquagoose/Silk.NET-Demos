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
in vec4 frag_lightSpace;

out vec4 out_color;

uniform DirectionalLight uSun;
uniform Material uMaterial;
uniform vec3 uCameraPos;
uniform sampler2D uShadowMap;

vec4 CalculateDirectional(DirectionalLight light, vec3 normal, vec3 viewDir, float shadow);
float CalculateShadow(vec4 lightSpace);

void main()
{
    vec3 norm = normalize(frag_normal);
    vec3 viewDir = normalize(uCameraPos - frag_position);

    float shadow = CalculateShadow(frag_lightSpace);
    vec4 result = CalculateDirectional(uSun, norm, viewDir, shadow);
    out_color = result * uMaterial.color;
}

vec4 CalculateDirectional(DirectionalLight light, vec3 normal, vec3 viewDir, float shadow)
{
    vec3 lightDir = normalize(-light.direction);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), uMaterial.shininess);
    vec4 alRes = texture(uMaterial.albedo, frag_texCoords);
    vec3 ambient = light.ambient * vec3(alRes);
    vec3 diffuse = light.diffuse * diff * vec3(alRes);
    vec3 specular = light.specular * spec * vec3(texture(uMaterial.specular, frag_texCoords));
    //return vec4(ambient + diffuse + specular, alRes.a);
    return vec4(ambient + (1.0 - shadow) * (diffuse + specular), 1.0);
}

float CalculateShadow(vec4 lightSpace)
{
    vec3 proj = lightSpace.xyz / lightSpace.w;
    proj = proj * 0.5 + 0.5;
    float closestDepth = texture(uShadowMap, proj.xy).r;
    float currentDepth = proj.z;
    float bias = 0.001;
    float shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;
    return shadow; 
}