Shader "Custom/Bloom"
{
 Properties {
        _MainTex("MainTex", 2D) = ""{}
    }

    SubShader {
        Pass {
            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert_img
            #pragma fragment frag

            sampler2D _MainTex;
            float2 _PixelSize;
fixed4 gaussian(v2f_img i,int num)
{
_PixelSize = 0.001;
float4 col = float4(0, 0, 0, 0);
/*
[loop]for (int j = 1 ;j < num;j++){
col += tex2D(_MainTex, i.uv + _PixelSize * float2(-j, j)) * 0.033;
col += tex2D(_MainTex, i.uv + _PixelSize * float2(j, j)) * 0.033;
col += tex2D(_MainTex, i.uv + _PixelSize * float2(-j, -j)) * 0.033;
col += tex2D(_MainTex, i.uv + _PixelSize * float2(j, -j)) * 0.033;
}*/
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(-3, 3)) * 0.053;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(-2, 2)) * 0.123;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(-1, 1)) * 0.203;
	//col += tex2D(_MainTex, i.uv) * 0.240;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(1, 1)) * 0.203;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(2, 2)) * 0.123;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(3, 3)) * 0.053;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(-3, -3)) * 0.053;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(-2, -2)) * 0.123;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(-1, -1)) * 0.203;
	//col += tex2D(_MainTex, i.uv) * 0.240;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(1, -1)) * 0.203;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(2, -2)) * 0.123;
	//col += tex2D(_MainTex, i.uv + _PixelSize * float2(3, -3)) * 0.053;
	return col;
}

            fixed4 frag(v2f_img i) : COLOR {
                fixed4 c = tex2D(_MainTex, i.uv);
                c*=0.5;
                float gray = c.r  + c.g + c.b;
                c += gaussian(i,7);

                //c.rgb = fixed3(gray, gray, gray);
                return c;
            }

            ENDCG
        }
    }
}