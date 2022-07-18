using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonHighlightTextChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI buttonText;

        [SerializeField]
        private Color baseColor;
        [SerializeField]
        private Color highlightColor;
        #region Implementation of IPointerEnterHandler

        private void Start()
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <inheritdoc />
        public void OnPointerEnter(PointerEventData eventData)
        {
            buttonText.color = highlightColor; 
        }

        #endregion

        #region Implementation of IPointerExitHandler

        /// <inheritdoc />
        public void OnPointerExit(PointerEventData eventData)
        {
            buttonText.color = baseColor;
        }

        #endregion
    }
}
