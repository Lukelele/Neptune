using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


/// <summary>
/// The causticsAnimation class is a MonoBehaviour that animates a light cookie with a caustics texture.
/// </summary>
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
        // Get the Light component attached to the GameObject
        light = GetComponent<Light>();

        // Get the UniversalAdditionalLightData component attached to the GameObject
        lightData = GetComponent<UniversalAdditionalLightData>();
        if(lightData == null) {
            enabled = false;
        }

        // Set the light cookie to the first caustics texture
        light.color = causticColor;
        lightData.lightCookieOffset = cookieOffset;
    }
    

    void Update()
    {
        // If the light is enabled and there is at least one caustics texture
        if (light && caustics.Length>=1)
        {
            // Set the light cookie to the caustics texture at the current frame
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