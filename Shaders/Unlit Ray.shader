Shader "ProceduralRays/Unlit Ray"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor ("Color", Color) = (1,1,1,1)
		_AnimationSpeed ("Animation Speed", float) = 1
		
		[HideInInspector] _RaysCount ("Rays Count", int) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MainColor;
			int _RaysCount;
			float _AnimationSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv - float2(1,1) * ((int) (_Time.w * _AnimationSpeed)) % _RaysCount;
				if (length(uv) > 1.415f || uv.x + uv.y < 0) {
					return fixed4(0,0,0,0);
				}
				fixed4 col = tex2D(_MainTex, uv) * _MainColor;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
