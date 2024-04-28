using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class causticsAnimation : MonoBehaviour
{
    public Texture[] caustics;
    [Range(1,4.5f)]
    public float boost = 3;

    [Range(-10,10)]
    public float xSpeed = 0.8f;
    [Range(-10,10)]
    public float ySpeed = 0.15f;

    private UniversalAdditionalLightData lightData;

    [SerializeField] private Vector2 cookieOffset;

    [SerializeField] private Color causticColor;

    private Light light;

   
    void Start()
    {     
        light = GetComponent<Light>();

        lightData = GetComponent<UniversalAdditionalLightData>();
        if(lightData == null) {
            enabled = false;
        }

        light.color = causticColor;
        lightData.lightCookieOffset = cookieOffset;
    }
    
    void Update()
    {

        if (light && caustics.Length>=1)
        {
            int index = ((int)(Time.frameCount/ boost))%caustics.Length;
            light.cookie = caustics[index];
            cookieOffset[0] = cookieOffset[0] + Time.deltaTime*xSpeed;
            cookieOffset[1] = cookieOffset[1] + Time.deltaTime*ySpeed;
            lightData.lightCookieOffset = cookieOffset;
        }
    }

    private void OnDisable()
    {
        if (light)
        {
            light.cookie = null;
        }
    }
}