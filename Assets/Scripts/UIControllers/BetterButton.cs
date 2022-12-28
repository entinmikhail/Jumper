﻿using UnityEngine;
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