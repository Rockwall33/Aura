///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                                                                                                                                             ///
///     MIT License                                                                                                                                             ///
///                                                                                                                                                             ///
///     Copyright (c) 2016 Raphaël Ernaelsten (@RaphErnaelsten)                                                                                                 ///
///                                                                                                                                                             ///
///     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),      ///
///     to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,                  ///
///     and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:              ///
///                                                                                                                                                             ///
///     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                          ///
///                                                                                                                                                             ///
///     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     ///
///     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER      ///
///     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS    ///
///     IN THE SOFTWARE.                                                                                                                                        ///
///                                                                                                                                                             ///
///     PLEASE CONSIDER CREDITING AURA IN YOUR PROJECTS. IF RELEVANT, USE THE UNMODIFIED LOGO PROVIDED IN THE "LICENSE" FOLDER.                                 ///
///                                                                                                                                                             ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// Shader used for drawing the Texture3D previews
Shader "Hidden/Aura/DrawTexture3DPreview"
{
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}

		CGINCLUDE
		#include "UnityCG.cginc"

		int _SamplingQuality;
		sampler3D _MainTex;
		float _Density;

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 localPos : TEXCOORD0;
			float4 screenPos : TEXCOORD1;
			float3 worldPos : TEXCOORD2;
		};

		v2f vert(appdata_base v)
		{
			v2f OUT;
			OUT.pos = UnityObjectToClipPos(v.vertex);
			OUT.localPos = v.vertex.xyz;
			OUT.screenPos = ComputeScreenPos(OUT.pos);
			COMPUTE_EYEDEPTH(OUT.screenPos.z);
			OUT.worldPos = mul(unity_ObjectToWorld, v.vertex);
			return OUT;
		}

		// usual ray/cube intersection algorithm
		struct Ray
		{
			float3 origin;
			float3 direction;
		};
		bool IntersectBox(Ray ray, out float entryPoint, out float exitPoint)
		{
			float3 invR = 1.0 / ray.direction;
			float3 tbot = invR * (float3(-0.5, -0.5, -0.5) - ray.origin);
			float3 ttop = invR * (float3(0.5, 0.5, 0.5) - ray.origin);
			float3 tmin = min(ttop, tbot);
			float3 tmax = max(ttop, tbot);
			float2 t = max(tmin.xx, tmin.yz);
			entryPoint = max(t.x, t.y);
			t = min(tmax.xx, tmax.yz);
			exitPoint = min(t.x, t.y);
			return entryPoint <= exitPoint;
		}

		float4 frag(v2f IN) : COLOR
		{
			float3 localCameraPosition = UNITY_MATRIX_IT_MV[3].xyz;
	
			Ray ray;
			ray.origin = localCameraPosition;
			ray.direction = normalize(IN.localPos - localCameraPosition);

			float entryPoint, exitPoint;
			IntersectBox(ray, entryPoint, exitPoint);
		
			if (entryPoint < 0.0) entryPoint = 0.0;

			float3 rayStart = ray.origin + ray.direction * entryPoint;
			float3 rayStop = ray.origin + ray.direction * exitPoint;

			float3 start = rayStop;
			float dist = distance(rayStop, rayStart);
			float stepSize = dist / float(_SamplingQuality);
			float3 ds = normalize(rayStop - rayStart) * stepSize;
				
			float4 color = float4(0,0,0,0);
			for (int i = _SamplingQuality; i >= 0; --i)
			{
				float3 pos = start.xyz + 0.5f;
				pos.x = 1.0f - pos.x;
				float4 mask = tex3D(_MainTex, pos);
					
				color.xyz += mask.xyz * mask.w;
				
				start -= ds;
			}
			color *= _Density / (uint)_SamplingQuality;

			// Weird enough, bruteforce seems quicker (to be investigated)
			//const float maxDist = 0.86602540378443864676372317075294f;
			//float stepSize = maxDist / (float)_SamplingQuality;
			//float dist = distance(rayStop, rayStart);
			//int steps = ceil(dist / stepSize);
			//float3 ds = normalize(rayStop - rayStart) * stepSize;
			//float3 color = float3(0,0,0);
			//[unroll(512)]
			//for (int i = steps; i >= 0; --i)
			//{
			//	float3 pos = start.xyz;
			//	pos.xyz = pos.xyz + 0.5f;
			//	float4 mask = tex3D(_MainTex, pos);
			//	
			//	color += mask.xyz * mask.w;
			//
			//	start += ds;
			//}				
			//color *= _Density / steps;

			return color;
		}
		ENDCG

		Pass
		{
			Cull front
			Blend One One
			ZWrite Off

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			ENDCG

		}
	}
}