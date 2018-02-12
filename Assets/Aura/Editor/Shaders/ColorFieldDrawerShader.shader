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

/// Shader used for drawing the color disk of the CircularPicker attribute
Shader "Hidden/Aura/GUI/DrawCircularPickerDisk"
{
    CGINCLUDE

        #include "UnityCG.cginc"

        #define PI 3.14159265359
        #define PI2 6.28318530718

		float colorDiskSize;
		float2 pickerCoordinates;
		float alpha;

        float3 HsvToRgb(float3 rgb)
        {
			rgb.x = 1.0 - ((rgb.x > 0.0) ? rgb.x : PI2 + rgb.x) / PI2;

            float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 p = abs(frac(rgb.xxx + K.xyz) * 6.0 - K.www);
            return rgb.z * lerp(K.xxx, saturate(p - K.xxx), rgb.y);
        }

        float4 DrawDisk(v2f_img i, float offsetColor)
        {
            float4 color = (0.0).xxxx;
            float2 uvc = i.uv - (0.5).xx;
			float dist = sqrt(dot(uvc, uvc));
            float delta = fwidth(dist);
            float angle = atan2(uvc.x, uvc.y);

			color.w = smoothstep(0.5f, colorDiskSize + delta, dist) * offsetColor;

            float circleMask = smoothstep(colorDiskSize + delta, colorDiskSize - delta, dist);
			float saturationMask = smoothstep(0, colorDiskSize - delta, dist);
            float hue = angle;
            float4 c = float4(HsvToRgb(float3(angle, saturationMask, 1)), 1.0) * circleMask;
            color += c;

			float2 refPos = pickerCoordinates;
			const float sizeFactor = 0.02f;
			const float widthFactor = 0.005f;
			float uvDist = distance(refPos, i.uv);
			float ring = smoothstep(sizeFactor - widthFactor, sizeFactor, uvDist) - smoothstep(sizeFactor, sizeFactor + widthFactor, uvDist);
			color *= lerp(0.65f, 1.0f, pow(smoothstep(sizeFactor + delta * widthFactor, sizeFactor * 5, uvDist) + smoothstep(sizeFactor - delta * widthFactor, 0, uvDist), 0.25));
			color += ring;

			color.w *= alpha;

            return color;
        }

        float4 FragDiskDark(v2f_img i) : SV_Target
        {
            return DrawDisk(i, 0.3);
        }

        float4 FragDiskLight(v2f_img i) : SV_Target
        {
            return DrawDisk(i, 0.6);
        }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        // (0) Dark skin
        Pass
        {
            CGPROGRAM

                #pragma vertex vert_img
                #pragma fragment FragDiskDark

            ENDCG
        }

        // (1) Light skin
        Pass
        {
            CGPROGRAM

                #pragma vertex vert_img
                #pragma fragment FragDiskLight

            ENDCG
        }
    }
}
