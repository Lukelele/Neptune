using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float defaultValue;
    [SerializeField] private TextMeshProUGUI valueText;
    
    void Start()
    {
        slider.value = defaultValue;
    }
    
    public void UpdateValue(float value)
    {
        valueText.text = value.ToString("0.00");
    }
}