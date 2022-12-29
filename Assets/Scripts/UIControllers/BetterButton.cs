using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIControllers
{
    [RequireComponent(typeof(Button))]
    public class BetterButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Graphic[] _graphics;
        [SerializeField] private Color _pressedColor;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _lostConnectionColor;
        [SerializeField] private Sprite _lostConnectionsSprite;
        
        public Button Button => _button;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_button.interactable)
                SetColorToGraphic(_pressedColor);
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_button.interactable)
                SetColorToGraphic(_defaultColor);
        }

        public void SetLostConnection()
        {
            SetColorToGraphic(_lostConnectionColor);
            _button.interactable = false;
            _button.image.sprite = _lostConnectionsSprite;
        }
        
        public void SetInteractable(bool value)
        {
            _button.interactable = value;
            SetColorToGraphic(value ? _defaultColor : _disabledColor);
        }

        private void SetColorToGraphic(Color color)
        {
            foreach (var graphic in _graphics)
                graphic.color = color;
        }
    }   
}