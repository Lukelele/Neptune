
#ifndef SharkShader
#define SharkShader
void SharkShader_float(float4 mainTexture, float4 color, out float4 output)
{
    output = mainTexture;
    if (abs(mainTexture.r-0.22f) < 0.01f) output = color;
}
#endif