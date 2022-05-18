using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Healthbar : MonoBehaviour
{
    [SerializeField] private Gradient _healthGradient;
    [SerializeField, Range(0f, 1f)] private float _healthPercents;
    [SerializeField] private Image _fillArea;
    [SerializeField] private Text _textArea;
    [SerializeField] private GameRuler _gameRuler;
    
    public float HealthPercents { get => _healthPercents; set => _healthPercents = value < 0 ? 0 : value > 1 ? 1 : value; }

    void Update()
    {
        if (!_fillArea) return;

        _fillArea.fillAmount = _healthPercents;
        _fillArea.rectTransform.localScale = new Vector3(_healthPercents, _fillArea.rectTransform.localScale.y, _fillArea.rectTransform.localScale.z);
        _fillArea.color = _healthGradient.Evaluate(_healthPercents);

        _textArea.text = $"{_gameRuler.CurrentHealth} / {_gameRuler.MaxHealth}";

    }
}
