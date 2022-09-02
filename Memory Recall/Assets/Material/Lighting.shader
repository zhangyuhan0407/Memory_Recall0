// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Lighting"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_Color("Color", Color) = (0.09149162,0.1438234,0.1886792,0)
		_Texture2("Texture 2", 2D) = "white" {}
		_Texture3("Texture 3", 2D) = "white" {}
		_Texture4("Texture 4", 2D) = "white" {}
		_Vector0("Vector 0", Vector) = (0.1,0.5,0.6,0.8)
		_FlashSpeed("Flash Speed", Vector) = (5,4,8,3)
		_Intensity("Intensity", Float) = 8.48
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Background+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform float _Intensity;
		uniform sampler2D _Texture0;
		uniform float4 _Vector0;
		uniform float4 _FlashSpeed;
		uniform sampler2D _Texture2;
		uniform sampler2D _Texture3;
		uniform sampler2D _Texture4;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color60 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float2 uv_TexCoord6 = i.uv_texcoord + float2( 0,0.75 );
			float2 panner7 = ( ( _Vector0.x * _Time.y ) * float2( 2,0 ) + uv_TexCoord6);
			float mulTime9 = _Time.y * _FlashSpeed.x;
			float2 temp_cast_0 = (mulTime9).xx;
			float simplePerlin2D8 = snoise( temp_cast_0 );
			simplePerlin2D8 = simplePerlin2D8*0.5 + 0.5;
			float2 uv_TexCoord16 = i.uv_texcoord + float2( 0,0.75 );
			float2 panner19 = ( ( _Vector0.y * _Time.y ) * float2( 2,0 ) + uv_TexCoord16);
			float mulTime18 = _Time.y * _FlashSpeed.y;
			float2 temp_cast_1 = (mulTime18).xx;
			float simplePerlin2D21 = snoise( temp_cast_1 );
			simplePerlin2D21 = simplePerlin2D21*0.5 + 0.5;
			float clampResult56 = clamp( ( tex2D( _Texture2, panner19 ).r * simplePerlin2D21 ) , 0.0 , 1.0 );
			float2 uv_TexCoord44 = i.uv_texcoord + float2( 0,0.75 );
			float2 panner46 = ( ( _Vector0.z * _Time.y ) * float2( 2,0 ) + uv_TexCoord44);
			float mulTime43 = _Time.y * _FlashSpeed.z;
			float2 temp_cast_2 = (mulTime43).xx;
			float simplePerlin2D42 = snoise( temp_cast_2 );
			simplePerlin2D42 = simplePerlin2D42*0.5 + 0.5;
			float clampResult57 = clamp( ( tex2D( _Texture3, panner46 ).r * simplePerlin2D42 ) , 0.0 , 1.0 );
			float2 uv_TexCoord52 = i.uv_texcoord + float2( 0,0.75 );
			float2 panner54 = ( ( _Vector0.w * _Time.y ) * float2( 2,0 ) + uv_TexCoord52);
			float mulTime51 = _Time.y * _FlashSpeed.w;
			float2 temp_cast_3 = (mulTime51).xx;
			float simplePerlin2D50 = snoise( temp_cast_3 );
			simplePerlin2D50 = simplePerlin2D50*0.5 + 0.5;
			float clampResult58 = clamp( ( tex2D( _Texture4, panner54 ).r * simplePerlin2D50 ) , 0.0 , 1.0 );
			float temp_output_59_0 = ( ( tex2D( _Texture0, panner7 ).r * simplePerlin2D8 ) + clampResult56 + clampResult57 + clampResult58 );
			float4 lerpResult61 = lerp( _Color , color60 , ( _Intensity * temp_output_59_0 ));
			float4 temp_output_15_0 = ( lerpResult61 * temp_output_59_0 );
			o.Emission = temp_output_15_0.rgb;
			o.Alpha = temp_output_15_0.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
0;468;1043;315.4;243.2079;188.5464;1.365064;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;14;-1688.619,574.4297;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;11;-1680.832,371.5515;Inherit;False;Property;_Vector0;Vector 0;6;0;Create;True;0;0;0;False;0;False;0.1,0.5,0.6,0.8;0.1,0.5,0.6,0.8;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1385.546,290.1519;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.75;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1416.096,708.2053;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.75;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;12;-1677.786,145.2108;Inherit;False;Property;_FlashSpeed;Flash Speed;7;0;Create;True;0;0;0;False;0;False;5,4,8,3;5,4,8,3;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-1410.459,1164.775;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.75;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1333.988,812.537;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1303.437,394.4838;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1325.648,1269.106;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;51;-1117.504,1285.896;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;18;-1092.59,411.2723;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1307.562,-126.011;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;54;-1212.42,1165.655;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;43;-1123.141,829.3261;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;46;-1218.057,709.0851;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;55;-1265.339,984.8552;Inherit;True;Property;_Texture4;Texture 4;5;0;Create;True;0;0;0;False;0;False;99071f840bab5c643b0625cd65bdc3df;99071f840bab5c643b0625cd65bdc3df;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;19;-1187.506,291.0317;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1386.581,-245.5372;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.75;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;17;-1240.425,110.2323;Inherit;True;Property;_Texture2;Texture 2;3;0;Create;True;0;0;0;False;0;False;99071f840bab5c643b0625cd65bdc3df;99071f840bab5c643b0625cd65bdc3df;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;47;-1270.976,528.2856;Inherit;True;Property;_Texture3;Texture 3;4;0;Create;True;0;0;0;False;0;False;99071f840bab5c643b0625cd65bdc3df;99071f840bab5c643b0625cd65bdc3df;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.NoiseGeneratorNode;50;-972.788,1287.334;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-1061.705,635.6053;Inherit;True;Property;_TextureSample15;Texture Sample 15;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-1031.154,217.5519;Inherit;True;Property;_TextureSample14;Texture Sample 14;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;42;-978.425,830.7651;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-984.6629,-132.7917;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1224.567,-414.9722;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;0;False;0;False;99071f840bab5c643b0625cd65bdc3df;99071f840bab5c643b0625cd65bdc3df;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.NoiseGeneratorNode;21;-947.874,412.7111;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-1056.068,1092.175;Inherit;True;Property;_TextureSample16;Texture Sample 16;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;7;-1182.86,-238.0754;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-753.8413,1121;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-728.9272,246.3772;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-992.8776,-400.5221;Inherit;True;Property;_TextureSample12;Texture Sample 12;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-759.4783,664.4305;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;8;-830.4435,-141.3473;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;57;-460.8665,674.8199;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;56;-468.0871,258.8702;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-536.379,-165.6938;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;58;-466.154,1121.426;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-29.18591,408.8223;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;261.4604,-85.09076;Inherit;False;Property;_Intensity;Intensity;8;0;Create;True;0;0;0;False;0;False;8.48;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;407.4177,-89.52166;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;60;56.54826,-161.7291;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;151.705,-424.9353;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;0;False;0;False;0.09149162,0.1438234,0.1886792,0;0.2039516,0.2568468,0.3207547,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;61;499.8733,-282.212;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;663.4845,-63.75224;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;874.7297,-160.4921;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Lighting;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Background;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;11;3
WireConnection;45;1;14;0
WireConnection;23;0;11;2
WireConnection;23;1;14;0
WireConnection;53;0;11;4
WireConnection;53;1;14;0
WireConnection;51;0;12;4
WireConnection;18;0;12;2
WireConnection;13;0;11;1
WireConnection;13;1;14;0
WireConnection;54;0;52;0
WireConnection;54;1;53;0
WireConnection;43;0;12;3
WireConnection;46;0;44;0
WireConnection;46;1;45;0
WireConnection;19;0;16;0
WireConnection;19;1;23;0
WireConnection;50;0;51;0
WireConnection;40;0;47;0
WireConnection;40;1;46;0
WireConnection;20;0;17;0
WireConnection;20;1;19;0
WireConnection;42;0;43;0
WireConnection;9;0;12;1
WireConnection;21;0;18;0
WireConnection;48;0;55;0
WireConnection;48;1;54;0
WireConnection;7;0;6;0
WireConnection;7;1;13;0
WireConnection;49;0;48;1
WireConnection;49;1;50;0
WireConnection;22;0;20;1
WireConnection;22;1;21;0
WireConnection;5;0;2;0
WireConnection;5;1;7;0
WireConnection;41;0;40;1
WireConnection;41;1;42;0
WireConnection;8;0;9;0
WireConnection;57;0;41;0
WireConnection;56;0;22;0
WireConnection;10;0;5;1
WireConnection;10;1;8;0
WireConnection;58;0;49;0
WireConnection;59;0;10;0
WireConnection;59;1;56;0
WireConnection;59;2;57;0
WireConnection;59;3;58;0
WireConnection;63;0;62;0
WireConnection;63;1;59;0
WireConnection;61;0;1;0
WireConnection;61;1;60;0
WireConnection;61;2;63;0
WireConnection;15;0;61;0
WireConnection;15;1;59;0
WireConnection;0;2;15;0
WireConnection;0;9;15;0
ASEEND*/
//CHKSM=4970A22D2D6C611FDD91B9009FE5F3B9CE997249