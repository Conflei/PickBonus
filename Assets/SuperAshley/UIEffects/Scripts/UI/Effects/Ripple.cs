using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SuperAshley.UI.Effects
{
    [AddComponentMenu("UI/Effects/Extensions/Ripple")]
    public class Ripple : BaseUIEffect
    {
        [Tooltip("Number of slices")]
        [Range(1, 200)]
        public int slices = 2;

        [Header("Ripple Properties")]

        /// <summary>
        /// Magnitude of the ripple
        /// </summary>
        [Tooltip("Magnitude of the ripple")]
        public float magnitude = 10f;

        /// <summary>
        /// Frequency of the ripple
        /// </summary>
        [Tooltip("Frequency of the ripple")]
        public float frequency = 10f;

        /// <summary>
        /// Ripple effect
        /// </summary>
        /// <param name="vh"></param>
        protected override void ApplyEffect(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }
            var original = new List<UIVertex>();
            vh.GetUIVertexStream(original);

            UIVertex top = original[2];
            UIVertex bottom = original[0];

            float uiElementHeight = top.position.y - bottom.position.y;
            float heightPerPiece = uiElementHeight / slices;

            float uvHeight = top.uv0.y - bottom.uv0.y;
            float uvPerPiece = uvHeight / slices;

            vh.Clear();

            for (int i = 0; i < slices; i++)
            {
                float xOffset = Mathf.Sin((1 - fill) * i * frequency * Mathf.PI / slices) * magnitude;

                AddQuad(vh,
                    new Vector2(bottom.position.x + xOffset, bottom.position.y + i * heightPerPiece),
                    new Vector2(top.position.x + xOffset, bottom.position.y + (i + 1) * heightPerPiece),
                    top.color,
                    new Vector2(bottom.uv0.x, bottom.uv0.y + i * uvPerPiece),
                    new Vector2(top.uv0.x, bottom.uv0.y + (i + 1) * uvPerPiece));
            }
        }
    }
}