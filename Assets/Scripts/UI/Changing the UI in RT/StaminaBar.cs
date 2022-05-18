using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Gradient _staminaGradient;
    [SerializeField, Range(0f, 1f)] private float _staminaPercents;
    [SerializeField] private Image _fillArea;
    [SerializeField] private TPPlayerMovement _player;

    public float HealthPercents { get => _staminaPercents; set => _staminaPercents = value < 0 ? 0 : value > 1 ? 1 : value; }

    void Update()
    {
        if (!_fillArea) return;

        _staminaPercents = _player.Stamina / _player.MaxStamina;

        _fillArea.fillAmount = _staminaPercents;
        _fillArea.rectTransform.localScale = new Vector3(_staminaPercents, _fillArea.rectTransform.localScale.y, _fillArea.rectTransform.localScale.z);
        _fillArea.color = _staminaGradient.Evaluate(_staminaPercents);
    }
}
