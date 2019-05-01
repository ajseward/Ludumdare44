// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

    Shader "Lit/TiledWorldSpace" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _WallTex ("Base (RGB)", 2D) = "white" {}
        _TextureScale ("Texture Scale", Float) = 1.0
        _WallScale("Wall Scale", float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
     
    CGPROGRAM
    #pragma surface surf Lambert
     
    sampler2D _MainTex;
    sampler2D _WallTex;
    fixed4 _Color;
    float _TextureScale;
    float _WallScale;
     
    struct Input
    {
        float3 worldNormal;
        float3 worldPos;
    };
     
    void surf (Input IN, inout SurfaceOutput o)
    {
        float2 UV;
        fixed4 c;
        ///////////////////
        // https://forum.unity.com/threads/mapping-texture-to-world-position-instead-of-object-position.94766/
        ///////////////////
        if(abs(IN.worldNormal.x)>0.5)
        {
            UV = IN.worldPos.yz; // side
            float2x2 rotMatrix = float2x2(0, 1, -1, 0);
            UV = mul(UV, rotMatrix);
            c = tex2D(_WallTex, UV* _WallScale); // use WALLSIDE texture
        }
        else if(abs(IN.worldNormal.z)>0.5)
        {
            UV = IN.worldPos.xy; // front
            c = tex2D(_WallTex, UV * _WallScale); // use WALL texture
        }
        else
        {
            UV = IN.worldPos.xz; // top
            c = tex2D(_MainTex, UV* _TextureScale); // use FLR texture
        }
     
        o.Albedo = c.rgb * _Color;
    }
    ENDCG
    }
     
    Fallback "VertexLit"
    }
     
