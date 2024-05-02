using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The MatController class is a MonoBehaviour that controls the material of an object.
/// </summary>
public class MatController : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Shader initialShader;
    private Material _initialMat;
    void Start()
    {
        _initialMat = Instantiate(mat);
    }
    
    /// <summary>
    /// ToggleTransparency method toggles the transparency of the material.
    /// </summary>
    public void ToggleTransparency(bool isNotTransparent)
    {
        if (initialShader == null) mat.shader = isNotTransparent ? _initialMat.shader : Shader.Find("Shader Graphs/Transparent");
        else mat.shader = isNotTransparent ? initialShader : Shader.Find("Shader Graphs/Transparent");
        
    }

}
