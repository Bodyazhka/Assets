Shader "Custom/SimpleBlueCube"
{
    Properties
    {
        _Color ("Cube Color", Color) = (0, 0, 1, 1) // Синий цвет для куба
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1) // Белый цвет для ободка
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        // Основной цвет куба
        Pass
        {
            Name "MAIN"
            Tags { "LightMode"="Always" }

            ZWrite On
            ZTest LEqual
            Cull Back

            // Нарисуем синий куб
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color; // Синий цвет для куба

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _Color; // Возвращаем синий цвет
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}
