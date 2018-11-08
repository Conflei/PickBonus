using UnityEngine;
using UnityEngine.UI;

namespace SuperAshley.UI.Effects
{
    [AddComponentMenu("UI/Effects/Extensions/Dust")]
    public class Dust : BaseBlockUIEffect
    {
        [Header("Dust Properties")]

        /// <summary>
        /// Whether to draw the dust pile
        /// </summary>
        [Tooltip("Show the dust pile?")]
        public bool showDustPile = true;

        /// <summary>
        /// Dust effect logic
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
                    float yOffset = heightPerBlock * lerp * y;

                    if (showDustPile)
                    {
                        if (fillMethod == FillMethod.Horizontal)
                        {
                            yOffset += heightPerBlock * (verticalSlices * 0.5f - Mathf.Abs(verticalSlices * 0.5f - x)) / verticalSlices * (1 - lerp) * y * 0.5f;
                        }
                        else
                        {
                            yOffset += heightPerBlock * (verticalSlices * 0.5f - Mathf.Abs(verticalSlices * 0.5f - x)) / verticalSlices * (1 - lerp) * i * 0.005f;
                        }
                    }

                    Color finalColor = top.color;

                    if (applyFadeColor)
                    {
                        finalColor = Color.Lerp(fadeColor, top.color, lerp);
                    }

                    if (showDustPile || lerp != 0)
                    {
                        AddQuad(vh,
                                new Vector2(bottom.position.x + x * widthPerBlock, bottom.position.y + yOffset),
                                new Vector2(bottom.position.x + (x + 1) * widthPerBlock, bottom.position.y + heightPerBlock + yOffset),
                                finalColor,
                                new Vector2(bottom.uv0.x + x * uvWidthPerBlock, bottom.uv0.y + y * uvHeightPerBlock),
                                new Vector2(bottom.uv0.x + (x + 1) * uvWidthPerBlock, bottom.uv0.y + (y + 1) * uvHeightPerBlock));
                    }
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