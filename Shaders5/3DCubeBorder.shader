Shader "Custom/3DCubeWithBorderFixed"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0, 1, 1) // Синий цвет куба
        _BorderColor ("Border Color", Color) = (1, 1, 1, 1) // Белая обводка
        _BorderThickness ("Border Thickness", Range(0.0, 0.5)) = 0.1 // Толщина обводки
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
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
                float3 localPos : TEXCOORD0;
            };

            // Свойства
            float4 _BaseColor;         // Цвет куба
            float4 _BorderColor;       // Цвет обводки
            float _BorderThickness;    // Толщина обводки

            // Вершинный шейдер
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz; // Локальные координаты вершины (в пределах -1 до 1)
                return o;
            }

            // Фрагментный шейдер
            fixed4 frag (v2f i) : SV_Target
            {
                // Преобразуем локальные координаты в абсолютные
                float3 absPos = abs(i.localPos);

                // Определяем внутреннюю и внешнюю зоны
                float borderStart = 1.0 - _BorderThickness; // Начало границы
                float3 isBorder = step(borderStart, absPos) * step(absPos, 1.0);

                // Если в области границы, рисуем обводку
                if (max(isBorder.x, max(isBorder.y, isBorder.z)) > 0.0)
                {
                    return _BorderColor;
                }

                // Иначе рисуем базовый цвет
                return _BaseColor;
            }
            ENDCG
        }
    }
}
