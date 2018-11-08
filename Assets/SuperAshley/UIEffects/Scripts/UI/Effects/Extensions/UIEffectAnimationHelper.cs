using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperAshley.UI.Effects.Extensions
{
    /// <summary>
    /// A helper component used to animate UI Effect's fill variable
    /// It's useful for using the same animation clip to animate different effects
    /// </summary>
    [ExecuteInEditMode]
    public class UIEffectAnimationHelper : MonoBehaviour
    {
        /// <summary>
        /// Fill amount
        /// </summary>
        [Tooltip("Fill amount, or strength of the effect")]
        [Range(0f, 1f)]
        public float fill;

        /// <summary>
        /// The target UI Effect component to change the fill amount
        /// </summary>
        [Tooltip("The target UI Effect component to change the fill amount")]
        public BaseUIEffect uiEffect;

        /// <summary>
        /// The graphic component on this object
        /// </summary>
        private Graphic graphic;

        void Awake ()
        {
            uiEffect = GetComponent<BaseUIEffect>();
            graphic = GetComponent<Graphic>();
        }

        // Update is called once per frame
        void Update()
        {
            if (uiEffect != null && graphic != null)
            {
                uiEffect.fill = fill;
                graphic.SetVerticesDirty();
            }
#if !UNITY_EDITOR
            else
                enabled = false;
#endif
        }
    }
}