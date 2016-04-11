// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:33077,y:32542,varname:node_4795,prsc:2|emission-7162-OUT,custl-8497-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32383,y:32728,varname:node_2393,prsc:2|A-3374-RGB,B-797-RGB,C-9248-OUT;n:type:ShaderForge.SFN_Color,id:797,x:32135,y:32763,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.7338504,c2:0.6637111,c3:0.9117647,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32135,y:32926,cmnt:Emission Strength,varname:node_9248,prsc:2,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3374,x:32135,y:32579,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_3374,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ccc94a47f748d2d4eb53171dfe3c4f57,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Slider,id:3683,x:32253,y:33035,ptovrint:False,ptlb:Transparency,ptin:_Transparency,varname:node_3683,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8119658,max:1;n:type:ShaderForge.SFN_Subtract,id:8497,x:32684,y:32825,varname:node_8497,prsc:2|A-2393-OUT,B-3683-OUT;n:type:ShaderForge.SFN_Color,id:9851,x:32535,y:32503,ptovrint:False,ptlb:Emission Color,ptin:_EmissionColor,varname:node_9851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9338235,c2:0.5767733,c3:0.8796503,c4:1;n:type:ShaderForge.SFN_Slider,id:4822,x:32548,y:32697,ptovrint:False,ptlb:Emission Strength,ptin:_EmissionStrength,varname:node_4822,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:7162,x:32845,y:32540,varname:node_7162,prsc:2|A-9851-RGB,B-4822-OUT;proporder:797-3374-3683-9851-4822;pass:END;sub:END;*/

Shader "Shader Forge/Neon" {
    Properties {
        _TintColor ("Color", Color) = (0.7338504,0.6637111,0.9117647,1)
        _Texture ("Texture", 2D) = "black" {}
        _Transparency ("Transparency", Range(0, 1)) = 0.8119658
        _EmissionColor ("Emission Color", Color) = (0.9338235,0.5767733,0.8796503,1)
        _EmissionStrength ("Emission Strength", Range(0, 1)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TintColor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _Transparency;
            uniform float4 _EmissionColor;
            uniform float _EmissionStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float3 emissive = (_EmissionColor.rgb*_EmissionStrength);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 finalColor = emissive + ((_Texture_var.rgb*_TintColor.rgb*1.0)-_Transparency);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
