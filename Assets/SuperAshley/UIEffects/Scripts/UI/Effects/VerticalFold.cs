using UnityEngine;
using UnityEngine.UI;

namespace SuperAshley.UI.Effects
{
    [AddComponentMenu("UI/Effects/Extensions/VerticalFold")]
    public class VerticalFold : BaseBlockUIEffect
    {
        /// <summary>
        /// Vertical fold effect logic
        /// </summary>
        protected override void ApplyBlockEffect(VertexHelper vh, UIVertex top, UIVertex bottom, float uiElementWidth, float uiElementHeight,
            float widthPerBlock, float heightPerBlock, float uvWidth, float uvHeight, float uvWidthPerBlock, float uvHeightPerBlock)
        {
            float modifiedFill = fill * 1.1f;

            for (int i = blockSequence.Count - 1; i >= 0; i--)
            {
                int y = blockSequence[i] / verticalSlices;
                int x = blockSequence[i] % verticalSlices;
                float percent = (float)i / (float)blockSequence.Count;

                if (percent < (1.1f - modifiedFill))
                {
                    float lerp = Mathf.Lerp(1f, 0f, ((1.1f - modifiedFill) - percent) * 10);
                    float inverseLerp = 1 - lerp;

                    float yOffset = heightPerBlock * y + inverseLerp * heightPerBlock * 0.5f;
                    float yOffset2 = heightPerBlock * (y + 1) - inverseLerp * heightPerBlock * 0.5f;

                    Color finalColor = top.color;

                    if (applyFadeColor)
                    {
                        finalColor = Color.Lerp(fadeColor, top.color, lerp);
                    }

                    AddQuad(vh,
                            new Vector2(bottom.position.x + x * widthPerBlock, bottom.position.y + yOffset),
                            new Vector2(bottom.position.x + (x + 1) * widthPerBlock, bottom.position.y + yOffset2),
                            finalColor,
                            new Vector2(bottom.uv0.x + x * uvWidthPerBlock, bottom.uv0.y + y * uvHeightPerBlock),
                            new Vector2(bottom.uv0.x + (x + 1) * uvWidthPerBlock, bottom.uv0.y + (y + 1) * uvHeightPerBlock));
                }
                else
                {
                    AddQuad(vh,
                            new Vector2(bottom.position.x + x * widthPerBlock, bottom.position.y + y * heightPerBlock),
                            new Vector2(bottom.position.x + (x + 1) * widthPerBlock, bottom.position.y + (y + 1) * heightPerBlock),
                            top.color,
                            new Vector2(bottom.uv0.x + x * uvWidthPerBlock, bottom.uv0.y + y * uvHeightPerBlock),
                            new Vector2(bottom.uv0.x + (x + 1) * uvWidthPerBlock, bottom.uv0.y + (y + 1) * uvHeightPerBlock));
                }
            }
        }
    }
}