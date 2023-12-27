#define LIGHT_THRESHOLD 0.1
#define SHADOW_THRESHOLD 0.5

#define BRC_LIGHTING_PROPERTIES \
float4 LightColor; \
float4 ShadowColor;

#define BRC_LIGHTING_FRAGMENT \
float shadowAtten = SHADOW_ATTENUATION(i); \
if (shadowAtten > SHADOW_THRESHOLD) \
    shadowAtten = 1.0; \
else \
    shadowAtten = 0.0; \
fixed lighting = saturate(dot(i.normal, _WorldSpaceLightPos0)) * shadowAtten; \
if (lighting > LIGHT_THRESHOLD) \
    lighting = 1.0; \
else \
    lighting = 0.0; \
float4 BRCLighting = lerp(ShadowColor, LightColor, lighting) * _LightColor0.rgba;
