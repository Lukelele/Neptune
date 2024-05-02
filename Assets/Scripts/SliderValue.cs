using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// The SliderValue class sets the default value of a slider.
/// </summary>
public class SliderValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float defaultValue;
    [SerializeField] private TextMeshProUGUI valueText;
    
    void Start()
    {
        slider.value = defaultValue;
        slider.onValueChanged.Invoke(defaultValue);
    }
    
    public void UpdateValue(float value)
    {
        valueText.text = value.ToString("0.00");
    }
}