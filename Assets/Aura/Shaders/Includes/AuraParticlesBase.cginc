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

struct appdata_t
{
	float4 vertex : POSITION;
	fixed4 color : COLOR;
	float2 texcoord : TEXCOORD0;
};

struct v2f
{
	float4 vertex : SV_POSITION;
	fixed4 color : COLOR;
	float2 texcoord : TEXCOORD0;
	float4 projPos : TEXCOORD2;
    float3 frustumSpacePosition : TEXCOORD3;
};

v2f vert(appdata_t v)
{
	v2f o;

	o.vertex = UnityObjectToClipPos(v.vertex);
	
    o.projPos = ComputeScreenPos(o.vertex);
	COMPUTE_EYEDEPTH(o.projPos.z);
	
	o.color = v.color;

	o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

    o.frustumSpacePosition = Aura_GetFrustumSpaceCoordinates(v.vertex);

    #if _USAGESTAGE_VERTEX
        #if defined(_USAGETYPE_LIGHT) || defined(_USAGETYPE_BOTH)
	        Aura_ApplyLighting(o.color.xyz, o.frustumSpacePosition, _LightingFactor);
        #endif
        #if defined(_USAGETYPE_FOG) || defined(_USAGETYPE_BOTH)
	        Aura_ApplyFog(o.color, o.frustumSpacePosition);
        #endif
    #endif

	return o;
}

fixed4 frag(v2f i) : SV_Target
{
	float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
	float partZ = i.projPos.z;
	float fade = saturate(InverseLerp( 0, _SoftParticleDistanceFade, (sceneZ - partZ)));
	i.color.a *= fade;
	
	fixed4 col = i.color * _TintColor * tex2D(_MainTex, i.texcoord);

	#if _USAGESTAGE_PIXEL
        #if defined(_USAGETYPE_LIGHT) || defined(_USAGETYPE_BOTH)
				Aura_ApplyLighting(col.xyz, i.frustumSpacePosition, _LightingFactor);
        #endif
        #if defined(_USAGETYPE_FOG) || defined(_USAGETYPE_BOTH)
				Aura_ApplyFog(col, i.frustumSpacePosition);
        #endif
	#endif

	PREMULTIPLY_ALPHA(col)

	return col;
}