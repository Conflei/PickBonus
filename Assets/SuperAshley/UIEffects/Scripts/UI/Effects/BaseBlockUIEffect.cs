using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SuperAshley.Utility.Extensions;

namespace SuperAshley.UI.Effects
{
    /// <summary>
    /// Base class of all UI Effects in the package
    /// </summary>
    public abstract class BaseBlockUIEffect : BaseUIEffect
    {
        /// <summary>
        /// Number of horizontal slices
        /// </summary>
        [Tooltip("Number of horizontal slices")]
        [Range(1, 100)]
        public int horizontalSlices = 10;

        /// <summary>
        /// Number of vertical slices
        /// </summary>
        [Tooltip("Number of vertical slices")]
        [Range(1, 100)]
        public int verticalSlices = 10;

        /// <summary>
        /// Fill method
        /// </summary>
        [Tooltip("Fill method")]
        public FillMethod fillMethod;

        /// <summary>
        /// Inverse fill direction, only affects vertical or horizontal fill method
        /// </summary>
        [Tooltip("Inverse fill direction")]
        [DisplayIf("fillMethod", FillMethod.Horizontal, FillMethod.Vertical, FillMethod.HorizontalRandom, FillMethod.VerticalRandom)]
        public bool inverse = false;

        /// <summary>
        /// Whether we should apply the fade color
        /// </summary>
        [Tooltip("Whether to apply a color to fade to")]
        public bool applyFadeColor = true;

        /// <summary>
        /// The color to fade to
        /// </summary>
        [Tooltip("The color to fade to")]
        [DisplayIf("applyFadeColor", true)]
        public Color fadeColor = Color.clear;

        /// <summary>
        /// Store the block sequences
        /// </summary>
        protected List<int> blockSequence;

#if UNITY_EDITOR
        /// <summary>
        /// Used in editor only, to force update the effect after changing inverse
        /// </summary>
        private bool cachedInverse = false;

        /// <summary>
        /// Used in editor only, to force update the effect after changing fill method
        /// </summary>
        private FillMethod cachedFillMethod;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (cachedInverse != inverse || cachedFillMethod != fillMethod)
            {
                cachedInverse = inverse;
                cachedFillMethod = fillMethod;
                GenerateBlockSequence();
            }
        }
#endif

        /// <summary>
        /// Generate block sequence according to fill method
        /// </summary>
        private void GenerateBlockSequence()
        {
            if (blockSequence == null)
                blockSequence = new List<int>();

            blockSequence.Clear();

            switch (fillMethod)
            {
                case FillMethod.Horizontal:
                    GenerateBlockSequenceHorizontal(false);
                    break;
                case FillMethod.Vertical:
                    GenerateBlockSequenceVertical(false);
                    break;
                case FillMethod.HorizontalRandom:
                    GenerateBlockSequenceHorizontal(true);
                    break;
                case FillMethod.VerticalRandom:
                    GenerateBlockSequenceVertical(true);
                    break;
                case FillMethod.Random:
                    GenerateBlockSequenceRandom();
                    break;
            }
        }

        /// <summary>
        /// Generate block sequence randomly row by row
        /// </summary>
        private void GenerateBlockSequenceVertical(bool random)
        {
            List<int> blocksInThisRow = new List<int>();

            for (int i = 0; i < horizontalSlices; i++)
            {
                blocksInThisRow.Clear();
                for (int j = 0; j < verticalSlices; j++)
                {
                    blocksInThisRow.Add(i * verticalSlices + j);
                }

                if (random)
                    blocksInThisRow.Shuffle();

                if (inverse)
                {
                    blockSequence.InsertRange(0, blocksInThisRow);
                }
                else
                {
                    blockSequence.AddRange(blocksInThisRow);
                }
            }
        }

        /// <summary>
        /// Generate block sequence randomly column by column
        /// </summary>
        private void GenerateBlockSequenceHorizontal(bool random)
        {
            List<int> blocksInThisColumn = new List<int>();

            for (int i = 0; i < verticalSlices; i++)
            {
                blocksInThisColumn.Clear();
                for (int j = 0; j < horizontalSlices; j++)
                {
                    blocksInThisColumn.Add(j * verticalSlices + i);
                }

                if (random)
                    blocksInThisColumn.Shuffle();

                if (inverse)
                {
                    blockSequence.InsertRange(0, blocksInThisColumn);
                }
                else
                {
                    blockSequence.AddRange(blocksInThisColumn);
                }
            }
        }

        /// <summary>
        /// Generate block sequence completely random
        /// </summary>
        private void GenerateBlockSequenceRandom ()
        {
            for (int i = 0; i < verticalSlices; i++)
            {
                for (int j = 0; j < horizontalSlices; j++)
                {
                    blockSequence.Add(j * verticalSlices + i);
                }
            }
            blockSequence.Shuffle();
        }

        /// <summary>
        /// Apply effect to the UI
        /// </summary>
        protected override void ApplyEffect(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            int totalblocks = verticalSlices * horizontalSlices;

            if (blockSequence == null)
            {
                blockSequence = new List<int>();
            }

            if (blockSequence.Count != totalblocks)
            {
                GenerateBlockSequence();
            }

            var original = new List<UIVertex>();
            vh.GetUIVertexStream(original);

            UIVertex top = original[2];
            UIVertex bottom = original[0];

            float uiElementHeight = top.position.y - bottom.position.y;
            float heightPerBlock = uiElementHeight / horizontalSlices;

            float uiElementWidth = top.position.x - bottom.position.x;
            float widthPerBlock = uiElementWidth / verticalSlices;

            float uvHeight = top.uv0.y - bottom.uv0.y;
            float uvHeightPerBlock = uvHeight / horizontalSlices;

            float uvWidth = top.uv0.x - bottom.uv0.x;
            float uvWidthPerBlock = uvWidth / verticalSlices;

            vh.Clear();

            ApplyBlockEffect(vh, top, bottom, uiElementWidth, uiElementHeight, widthPerBlock, heightPerBlock, uvWidth, uvHeight, uvWidthPerBlock, uvHeightPerBlock);
        }

        /// <summary>
        /// Apply block effect to the UI
        /// </summary>
        protected abstract void ApplyBlockEffect(VertexHelper vh, UIVertex top, UIVertex bottom, float uiElementWidth, float uiElementHeight,
            float widthPerBlock, float heightPerBlock, float uvWidth, float uvHeight, float uvWidthPerBlock, float uvHeightPerBlock);
    }
}