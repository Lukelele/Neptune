using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownValue : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private int defaultValue = 0;
    
    void Start()
    {
        
        dropdown.value = defaultValue;
    }
}
