using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// The DropdownValue class sets the default value of a dropdown.
/// </summary>
public class DropdownValue : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private int defaultValue = 0;
    
    void Start()
    {
        dropdown.value = defaultValue;
        dropdown.onValueChanged.Invoke(defaultValue);
    }
}
