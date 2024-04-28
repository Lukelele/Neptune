using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatController : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Shader initialShader;
    private Material _initialMat;
    void Start()
    {
        _initialMat = Instantiate(mat);
    }
    
    public void ToggleTransparency(bool isNotTransparent)
    {
        if (initialShader == null) mat.shader = isNotTransparent ? _initialMat.shader : Shader.Find("Shader Graphs/Transparent");
        else mat.shader = isNotTransparent ? initialShader : Shader.Find("Shader Graphs/Transparent");
        
    }

}
