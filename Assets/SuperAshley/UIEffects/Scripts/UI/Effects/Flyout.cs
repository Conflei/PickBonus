using UnityEngine;
using UnityEngine.UI;

namespace SuperAshley.UI.Effects
{
    [AddComponentMenu("UI/Effects/Extensions/Flyout")]
    public class Flyout : BaseBlockUIEffect
    {
        [Header("Flyout Properties")]

        /// <summary>
        /// Final scale of the flyout block
        /// </summary>
        public float finalScale = 5f;

        /// <summary>
        /// The distance to flyout to
        /// </summary>
        public float flyoutDistance = 100f;

        /// <summary>
        /// Flyout effect logic
        /// </summary>
        protected override void ApplyBlockEffect(VertexHelper vh, UIVertex top, UIVertex bottom, float uiElementWidth, float uiElementHeight,
            float widthPerBlock, float heightPerBlock, float uvWidth, float uvHeight, float uvWidthPerBlock, float uvHeightPerBlock)
        {
            float modifiedFill = fill * 1.1f;
            float xCentre = verticalSlices * 0.5f;
            float yCentre = horizontalSlices * 0.5f;

            for (int i = blockSequence.Count - 1; i >= 0; i--)
            {
                int y = blockSequence[i] / verticalSlices;
                int x = blockSequence[i] % verticalSlices;
                float percent = (float)i / (float)blockSequence.Count;

                if (percent < (1.1f - modifiedFill))
                {
                    float lerp = Mathf.Lerp(1f, 0f, ((1.1f - modifiedFill) - percent) * 10);
                    float inverseLerp = 1 - lerp;

                    float scale = Mathf.Lerp(1f, finalScale, inverseLerp);

                    float distanceFromCenterX = x - xCentre;
                    float distanceFromCenterY = y - yCentre;

                    float xOffset = widthPerBlock * (x + 0.5f - scale * 0.5f) + distanceFromCenterX * inverseLerp * flyoutDistance;
                    float yOffset = heightPerBlock * (y + 0.5f - scale * 0.5f) + distanceFromCenterY * inverseLerp * flyoutDistance;

                    Color finalColor = top.color;

                    if (applyFadeColor)
                    {
                        finalColor = Color.Lerp(fadeColor, top.color, lerp);
                    }

                    AddQuad(vh,
                            new Vector2(bottom.position.x + xOffset, bottom.position.y + yOffset),
                            new Vector2(bottom.position.x + xOffset + widthPerBlock * scale, bottom.position.y + yOffset + heightPerBlock * scale),
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