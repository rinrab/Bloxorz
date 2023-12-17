float4x4 World;
float4x4 View;
float4x4 Projection;

struct VertexShaderInput
{
    float4 TexCoord : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Normal : NORMAL;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Normal : TEXCOORD2;
    float4 Color : COLOR0;
    float2 TextureCordinate : TEXCOORD0;
    float4 Pos2 : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Color = input.Color * output.Position.z * 10;
    output.TextureCordinate = input.TexCoord;
    output.Normal = input.Normal;

    output.Pos2 = worldPosition;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 light = normalize(float4(-1, 3, 1, 0));
    //float4 light = float4(0, -1, 0, 0);

    float normalMul = dot(input.Normal, light);

    return input.Color * (normalMul * 2 + 0);

    //return normalize(-input.Normal);

    //return float4(0.5, 0, 0, 0);
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}