// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Roystan/Toon"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)	

		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		_PointDiffuse("Point Diffuse", Float) = 1
		_PointSpecular("Point Specular", Float) = 1
		_PointIntensity("Point Intensity", Range(0, 5)) = 1.0
		_PointThreshold("Point Threshold", Range(0, 5)) = .65
		_PointSecondStep("Point Second Step Threshold", Range(0, 5)) = .5
		_PointSecondIntensity("Point Second Step Intensity", Range(0, 5)) = .5


	}	
	SubShader
	{
		Pass
		{

			Tags
			{

				"LightMode" = "ForwardBase"
//				"PassFlags" = "OnlyDirectional"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma glsl
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float3 normal : NORMAL;
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 worldNormal : NORMAL;
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 vertexLighting : TEXCOORD4;
				float pL : TEXCOORD5;
				float4 posWorld : TEXCOORD3;
				// world position variable
//				float4 posWorld : TEXCOORD3;
			
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Glossiness;
			float4 _SpecularColor;
			float4 _Color;
			float _PointIntensity;

			v2f vert (appdata v)
			{
				v2f o;
				o.pL = 0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.vertexLighting = float3(0.0, 0.0, 0.0);

				float4x4 modelMatrix = unity_ObjectToWorld;
				o.posWorld = mul(modelMatrix, v.vertex);

				#ifdef VERTEXLIGHT_ON
				//loop through the four point lights
		        for (int index = 0; index < 4; index++)
		        {  
		        	//find position of point light, store values
			        float4 lightPosition = float4(unity_4LightPosX0[index], 
			        unity_4LightPosY0[index], 
			        unity_4LightPosZ0[index], 1.0);

			        // a vector going from the vert to the point light 
			        float3 vertexToLightSource = 
			        lightPosition.xyz - o.posWorld.xyz;    

			        // normalize it
			        float3 lightDirection = normalize(vertexToLightSource);

			        // dunno why tf we do this?
			        float squaredDistance = dot(vertexToLightSource, vertexToLightSource);
			        float3 normalDir = normalize(o.worldNormal);

			        // calculate attentuation based off built in atten params and multiply it by
			        // one I guess
			        float attenuation = 1.0 / (1.0 + unity_4LightAtten0[index] * squaredDistance);
//			        float vertIntensity = smoothstep(-_PointIntensity, _PointIntensity, squaredDistance);
			        
			        // create an RGB reflection via the attentuation, colors from lights, 
			        float3 diffuseReflection = attenuation 
			        * unity_LightColor[index].rgb * _Color.rgb 
			        * max(0.0, dot(o.worldNormal, lightDirection));   

//		            float3 diffuseReflection = attenuation 
//			        * unity_LightColor[index].rgb 
//			        * max(0.0, dot(o.worldNormal, lightDirection));       

			       	o.vertexLighting = 
			        o.vertexLighting + diffuseReflection + (.001, .001, .001);

			        o.pL = 1;
//			        float vertIntensity = length(o.vertexLighting) > _PointIntensity ? 1 : 0;
//
//			        o.vertexLighting = vertIntensity;

		     	}
		     	#endif

				TRANSFER_SHADOW(o)
				return o;
			}
			
//			float4 _Color;
			float4 _AmbientColor;

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;
			float _PointDiffuse;
			float _PointSpecular;
			float _PointThreshold;
			float _PointSecondStep;
			float _PointSecondIntensity;

			float4 frag (v2f i) : SV_Target
			{

				float3 lightDirection;
				float attenuation;

				float3 normal = normalize(i.worldNormal);

				float NdotL = dot(_WorldSpaceLightPos0, normal);

//				float lightIntensity = NdotL > 0 ? 1 : 0;

				float shadow = SHADOW_ATTENUATION(i);

				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

				float4 light = lightIntensity * _LightColor0;

				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);

				float NdotH = dot(normal, halfVector);

				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);

				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);

				float4 rimDot = 1 - dot(viewDir, normal);	

				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);

				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);

				float4 rim = rimIntensity * _RimColor;

				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 sample = tex2D(_MainTex, i.uv);
			
				float3 vertLight = i.vertexLighting;
				float3 vertLightSecondRing = i.vertexLighting;
			
				float vertIntensity = length(vertLight) > _PointThreshold ? _PointIntensity : 0;
				vertIntensity = vertIntensity;
				float vertSecondIntensity = 0;

				if (length(vertLightSecondRing) > _PointSecondStep && length(vertLightSecondRing) < _PointThreshold)
				{
					 vertSecondIntensity = _PointSecondIntensity;
				}
				else if (length(vertLightSecondRing) < _PointSecondStep)
				{
					 vertSecondIntensity = 0;
				}
				else if (length(vertLightSecondRing) > _PointThreshold)
				{
					 vertSecondIntensity = 0;
				}

				vertLightSecondRing = normalize(vertLightSecondRing) * vertSecondIntensity;
				vertLight = normalize(vertLight) * vertIntensity;

				float pointOn = i.pL;
				//BEGIN VERT SHIT

				// check if directional light 
//				if (0.0 != _WorldSpaceLightPos0.w) 
//				{
//					// vector of vertex pointing toward point light source 
//					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
//
//					//calculate vector length to determine distance
//					float distance = length(vertexToLightSource);
//
//					// make linear attenuation
//					attenuation = 1.0 / distance;
//
//					//normalize it
//					lightDirection = normalize(vertexToLightSource);
//				}
//
//				// this doesn't really modify color so much as brightness
//				float3 diffuseReflection = attenuation * max(0.0, dot(normal, lightDirection));
//
//				float3 specularReflection;
//
//
//				if (dot(normal, lightDirection) < 0.0) 
//		        // light source on the wrong side?
//		    	  {
//		       	 specularReflection = float3(0.0, 0.0, 0.0); 
//		         // no specular reflection
//		    	  }
//			      else // light source on the right side
//			      {
//		        	specularReflection = attenuation 
//		        	 * _SpecularColor.rgb * pow(max(0.0, dot(
//		        	 reflect(-lightDirection, normal), 
//		        	 viewDir)), _Glossiness);
//		     	  }
				//END VERT SHIT


				float4 pointTotal = float4(vertLight + vertLightSecondRing, 0);

				if (pointOn == 1) {
					return _Color * sample * (_AmbientColor + light + specular + rim + pointTotal);
				}
				else
				{
					return _Color * sample * (_AmbientColor + light + specular + rim);
				}
			}
			ENDCG
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

	}
}