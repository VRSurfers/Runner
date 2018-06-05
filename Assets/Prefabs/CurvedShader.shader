Shader "Unlit/TestUnlitShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc" // for _LightColor0

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		fixed4 diff : COLOR0; // diffuse lighting color
		UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata v)
	{
		v2f o;
		float3 worldPos = UnityObjectToViewPos(v.vertex);
		float4 res = UnityObjectToClipPos(v.vertex);
		float distCoeff = worldPos.z / 15;
		float2 directionVector = float2(0.5, 1);
		distCoeff *= distCoeff;
		res.x += directionVector.x * distCoeff;
		res.y -= directionVector.y * distCoeff;
		o.vertex = res;
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.vertex = res;



		// get vertex normal in world space
		half3 worldNormal = UnityObjectToWorldNormal(v.normal);
		// dot product between normal and light direction for
		// standard diffuse (Lambert) lighting
		half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
		// factor in the light color
		o.diff = nl * _LightColor0;
		//return o;
		// the only difference from previous shader:
		// in addition to the diffuse lighting from the main light,
		// add illumination from ambient or light probes
		// ShadeSH9 function from UnityCG.cginc evaluates it,
		// using world space normal
		o.diff.rgb += ShadeSH9(half4(worldNormal, 1));



		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		//// sample the texture
		//fixed4 col = tex2D(_MainTex, i.uv);
		//// apply fog
		//UNITY_APPLY_FOG(i.fogCoord, col);
		//return col;

		// sample texture
		fixed4 col = tex2D(_MainTex, i.uv);
	// multiply by lighting
	col *= i.diff;
	return col;

	//fixed4 c = 0;
	//c.rgb = i.normal*0.5 + 0.5;
	//return c;

	//fixed4 col;
	//fixed4 tex =
	//	UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv.xy);
	//half3 bakedColor = DecodeLightmap(tex);
	//
	//tex = tex2D(_MainTex, i.uv);
	//col.rgb = tex.rgb * bakedColor;
	//col.a = 1.0f;
	//return col;
	}
		ENDCG
	}
	}
}
