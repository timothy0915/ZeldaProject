// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "QFX/ProjectilesFX/Particles Cutout"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("SrcBlend", Int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("DstBlend", Int) = 10
		_EmissiveMultiply("Emissive Multiply", Float) = 1
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_Cutout("Cutout", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend [_SrcBlend] [_DstBlend]
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform int _DstBlend;
				uniform int _SrcBlend;
				uniform float _EmissiveMultiply;
				uniform sampler2D _NoiseTexture;
				uniform float4 _NoiseTexture_ST;
				uniform float _Cutout;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					float4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
					float4 temp_output_7_0 = ( _EmissiveMultiply * ( _TintColor * tex2DNode1 * i.color ) );
					float4 appendResult11 = (float4((temp_output_7_0).rgb , saturate( (temp_output_7_0).a )));
					float2 uv_NoiseTexture = i.texcoord.xy * _NoiseTexture_ST.xy + _NoiseTexture_ST.zw;
					float3 uv028 = i.texcoord.xyz;
					uv028.xy = i.texcoord.xyz.xy * float2( 1,1 ) + float2( 0,0 );
					clip( ( tex2DNode1.a * tex2D( _NoiseTexture, uv_NoiseTexture ).r ) - ( 1.0 - ( uv028.z + _Cutout ) ));
					

					fixed4 col = appendResult11;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
// // // // // // // // // // // // // // // // // // // // // // // // // // 	CustomEditor "ASEMaterialInspector"  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool  // Removed by tool
	
	
}
/*ASEBEGIN
Version=18301
-36.8;503.2;1523;276;1766.701;265.7599;2.493052;True;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-795.2956,-94.87796;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;5;-468.5959,96.65337;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;4;-461.2593,-272.6733;Inherit;False;0;0;_TintColor;Shader;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-589.9226,-96.67332;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-238.5061,-115.3267;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-227.63,-266.6576;Inherit;False;Property;_EmissiveMultiply;Emissive Multiply;2;0;Create;True;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-10.31044,-138.8582;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;9;167.2415,-52.20482;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-269.7397,545.9268;Inherit;False;Property;_Cutout;Cutout;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-286.6126,375.4192;Inherit;False;0;-1;3;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-96.43347,174.9702;Inherit;True;Property;_NoiseTexture;Noise Texture;3;0;Create;True;0;0;False;0;False;-1;None;1583414be9e1fcc44a683569804dc712;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;29;382.2204,-49.13945;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;5.174201,441.7669;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;8;163.4177,-143.5959;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;534.2768,-135.0296;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;32;-842.863,-492.3533;Inherit;False;222.385;257.1656;Custom Blending;2;34;33;;1,0,0,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;27;148.4788,438.7831;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;314.4461,91.64698;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;33;-792.863,-350.5877;Inherit;False;Property;_DstBlend;DstBlend;1;1;[Enum];Create;True;0;1;UnityEngine.Rendering.BlendMode;True;0;False;10;1;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;34;-791.278,-440.3533;Inherit;False;Property;_SrcBlend;SrcBlend;0;1;[Enum];Create;True;0;1;UnityEngine.Rendering.BlendMode;True;0;False;5;5;0;1;INT;0
Node;AmplifyShaderEditor.ClipNode;25;693.6459,10.03131;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;35;916.7482,6.416676;Float;False;True;-1;2;ASEMaterialInspector;0;7;QFX/ProjectilesFX/Particles Cutout;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;0;True;34;0;True;33;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;1;0;2;0
WireConnection;6;0;4;0
WireConnection;6;1;1;0
WireConnection;6;2;5;0
WireConnection;7;0;12;0
WireConnection;7;1;6;0
WireConnection;9;0;7;0
WireConnection;29;0;9;0
WireConnection;36;0;28;3
WireConnection;36;1;37;0
WireConnection;8;0;7;0
WireConnection;11;0;8;0
WireConnection;11;3;29;0
WireConnection;27;0;36;0
WireConnection;30;0;1;4
WireConnection;30;1;22;1
WireConnection;25;0;11;0
WireConnection;25;1;30;0
WireConnection;25;2;27;0
WireConnection;35;0;25;0
ASEEND*/
//CHKSM=696201B6430CD713CC6299EC4F31BB6FD7007520
