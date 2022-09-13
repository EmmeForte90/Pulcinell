// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sprite Shaders Ultimate/Standard/Color/Recolor"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_RecolorFade("Recolor: Fade", Range( 0 , 1)) = 1
		_RecolorTintAreas("Recolor: Tint Areas", 2D) = "white" {}
		[HDR]_RecolorRedTint("Recolor: Red Tint", Color) = (1,1,1,0.5019608)
		[HDR]_RecolorYellowTint("Recolor: Yellow Tint", Color) = (1,1,1,0.5019608)
		[HDR]_RecolorGreenTint("Recolor: Green Tint", Color) = (1,1,1,0.5019608)
		[HDR]_RecolorCyanTint("Recolor: Cyan Tint", Color) = (1,1,1,0.5019608)
		[HDR]_RecolorBlueTint("Recolor: Blue Tint", Color) = (1,1,1,0.5019608)
		[HDR]_RecolorPurpleTint("Recolor: Purple Tint", Color) = (1,1,1,0.5019608)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _RecolorTintAreas;
			uniform float4 _RecolorTintAreas_ST;
			uniform float4 _RecolorPurpleTint;
			uniform float4 _RecolorBlueTint;
			uniform float4 _RecolorCyanTint;
			uniform float4 _RecolorGreenTint;
			uniform float4 _RecolorYellowTint;
			uniform float4 _RecolorRedTint;
			uniform float _RecolorFade;
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 temp_output_1_0_g46 = tex2D( _MainTex, uv_MainTex );
				float2 uv_RecolorTintAreas = IN.texcoord.xy * _RecolorTintAreas_ST.xy + _RecolorTintAreas_ST.zw;
				float3 hsvTorgb33_g46 = RGBToHSV( tex2D( _RecolorTintAreas, uv_RecolorTintAreas ).rgb );
				float temp_output_43_0_g46 = ( ( hsvTorgb33_g46.x + 0.08333334 ) % 1.0 );
				float4 ifLocalVar46_g46 = 0;
				if( temp_output_43_0_g46 >= 0.8333333 )
				ifLocalVar46_g46 = _RecolorPurpleTint;
				else
				ifLocalVar46_g46 = _RecolorBlueTint;
				float4 ifLocalVar44_g46 = 0;
				if( temp_output_43_0_g46 <= 0.6666667 )
				ifLocalVar44_g46 = _RecolorCyanTint;
				else
				ifLocalVar44_g46 = ifLocalVar46_g46;
				float4 ifLocalVar47_g46 = 0;
				if( temp_output_43_0_g46 <= 0.3333333 )
				ifLocalVar47_g46 = _RecolorYellowTint;
				else
				ifLocalVar47_g46 = _RecolorGreenTint;
				float4 ifLocalVar45_g46 = 0;
				if( temp_output_43_0_g46 <= 0.1666667 )
				ifLocalVar45_g46 = _RecolorRedTint;
				else
				ifLocalVar45_g46 = ifLocalVar47_g46;
				float4 ifLocalVar35_g46 = 0;
				if( temp_output_43_0_g46 >= 0.5 )
				ifLocalVar35_g46 = ifLocalVar44_g46;
				else
				ifLocalVar35_g46 = ifLocalVar45_g46;
				float4 break55_g46 = ifLocalVar35_g46;
				float3 appendResult56_g46 = (float3(break55_g46.r , break55_g46.g , break55_g46.b));
				float4 break2_g47 = temp_output_1_0_g46;
				float3 lerpResult26_g46 = lerp( (temp_output_1_0_g46).rgb , ( appendResult56_g46 * pow( ( ( break2_g47.x + break2_g47.y + break2_g47.z ) / 3.0 ) , max( ( break55_g46.a * 2.0 ) , 0.01 ) ) ) , ( hsvTorgb33_g46.z * _RecolorFade ));
				float4 appendResult30_g46 = (float4(lerpResult26_g46 , temp_output_1_0_g46.a));
				
				fixed4 c = ( appendResult30_g46 * IN.color );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "SpriteShadersUltimate.SingleShaderGUI"
	
	
}
/*ASEBEGIN
Version=18800
270;114;1338;608;1367.838;195.4122;2.895323;True;True
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;39;-980.0442,-54.6264;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-687.5431,30.52385;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;50;-342.9563,52.86554;Inherit;False;_Recolor;0;;46;d648533a061de38468cb13630c8ea7cf;0;1;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;22;-42.6362,53.99575;Inherit;False;TintVertex;-1;;45;b0b94dd27c0f3da49a89feecae766dcc;0;1;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;14;245.4418,49.685;Float;False;True;-1;2;SpriteShadersUltimate.SingleShaderGUI;0;8;Sprite Shaders Ultimate/Standard/Color/Recolor;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;24;0;39;0
WireConnection;50;1;24;0
WireConnection;22;1;50;0
WireConnection;14;0;22;0
ASEEND*/
//CHKSM=6E7D0D9B804346B5051069B85CC87CC2B86D8413