using TMPro;
using UnityEngine;

namespace UIControllers
{
    public class UIBetPanel : MonoBehaviour
    {
        [SerializeField] private UIBetButton[] _buttons;
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        public float CurrentBet {get; private set;}

        private void Awake()
        {
            foreach (var button in _buttons)
                button.OnClick += ChangeCurrentBetByButton;
        }

        private void ChangeCurrentBetByButton(UIBetButton uiBetButton)
        {
            if (uiBetButton.Type is ButtonType.Max or ButtonType.Min)
                CurrentBet = uiBetButton.Value;
            else
                CurrentBet += uiBetButton.Value;

            _textMesh.text = $"{CurrentBet}";
        }
    }
}