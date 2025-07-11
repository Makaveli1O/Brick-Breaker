using TMPro;
using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _instruction;
    private float _pulseSpeed = 2f;
    private float _minAlpha = 0.3f;
    private float _maxAlpha = 1f;
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        _instruction.text = text;
    }

    void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        float alpha = Mathf.Lerp(_minAlpha, _maxAlpha, (Mathf.Sin(Time.time * _pulseSpeed) + 1f) / 2f);
        Color color = _instruction.color;
        color.a = alpha;
        _instruction.color = color;
    }

}