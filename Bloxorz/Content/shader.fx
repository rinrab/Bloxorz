float4x4 World;
float4x4 View;
float4x4 Projection;
float3 Light;

Texture2D Texture;
sampler2D TextureSampler = sampler_state
{
    Texture = <Texture>;
};

struct VertexShaderInput
{
    float4 TexCoord : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Normal : NORMAL;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Normal : TEXCOORD2;
    float2 TexCoord : TEXCOORD0;
    float4 Pos2 : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.TexCoord = input.TexCoord;
    output.Normal = input.Normal;

    output.Pos2 = worldPosition;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float normalMul = dot(input.Normal, normalize(float4(Light, 0)));

    float4 color = tex2D(TextureSampler, input.TexCoord);

    return color * (normalMul * 1 + 0.2);
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
