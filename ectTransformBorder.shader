Shader "UI/BorderWithScaleSupport"
{
    Properties
    {
        _Color ("Border Color", Color) = (1,1,1,1)
        _Thickness ("Border Thickness", Range(0.0, 0.5)) = 0.05
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 worldScale : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Thickness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Получаем масштаб объекта
                float4x4 localToWorld = unity_ObjectToWorld;
                o.worldScale = float2(
                    length(localToWorld._m00_m10_m20), // scale.x
                    length(localToWorld._m01_m11_m21)  // scale.y
                );

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // UV-координаты с центром в (0.5, 0.5)
                float2 uv = i.uv - 0.5;

                // Корректировка толщины с учётом масштаба
                float2 adjustedThickness = _Thickness / i.worldScale;

                // Границы
                float2 borderOuter = float2(0.5, 0.5); // Внешняя граница (размер текстуры)
                float2 borderInner = borderOuter - adjustedThickness; // Внутренняя граница

                // Рисуем только границы
                if (abs(uv.x) <= borderOuter.x && abs(uv.y) <= borderOuter.y &&
                    (abs(uv.x) > borderInner.x || abs(uv.y) > borderInner.y))
                {
                    return _Color; // Цвет границы
                }

                // Остальная часть прозрачна
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
