using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MadnessBar : MonoBehaviour
{
    [SerializeField] private Gradient _madnessGradient;
    [SerializeField, Range(0f, 1f)] private float _madnessPercents;
    [SerializeField] private Image _fillArea;

    public float MadnessPercents { get => _madnessPercents; set => _madnessPercents = value < 0 ? 0 : value > 1 ? 1 : value; }

    void Update()
    {
        if (!_fillArea) return;

        _fillArea.fillAmount = _madnessPercents;
        _fillArea.rectTransform.localScale = new Vector3(_madnessPercents, _fillArea.rectTransform.localScale.y, _fillArea.rectTransform.localScale.z);
        _fillArea.color = _madnessGradient.Evaluate(_madnessPercents);
    }
}
