using UnityEngine;
using UnityEngine.UI;

namespace SuperAshley.UI.Effects
{
    public enum FillMethod
    {
        Horizontal,
        Vertical,
        HorizontalRandom,
        VerticalRandom,
        Random
    }

    /// <summary>
    /// Base class of all UI Effects in the package
    /// </summary>
    public abstract class BaseUIEffect : BaseMeshEffect
    {
        /// <summary>
        /// Fill amount
        /// </summary>
        [Tooltip("Fill amount, or strength of the effect")]
        [Range(0f, 1f)]
        public float fill = 1f;
        
        /// <summary>
        /// Unity method
        /// </summary>
        /// <param name="vh"></param>
        public override void ModifyMesh(VertexHelper vh)
        {
            if (fill == 1)
            {
                return;
            }

            ApplyEffect(vh);
        }

        /// <summary>
        /// Apply effect to the UI
        /// </summary>
        /// <param name="vh"></param>
        protected abstract void ApplyEffect(VertexHelper vh);

        /// <summary>
        /// Add a quad to VertexHelper
        /// </summary>
        /// <param name="vertexHelper"></param>
        /// <param name="posMin"></param>
        /// <param name="posMax"></param>
        /// <param name="color"></param>
        /// <param name="uvMin"></param>
        /// <param name="uvMax"></param>
        protected void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
    }
}