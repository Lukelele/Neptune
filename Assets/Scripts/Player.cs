using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float timeScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        timeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        FastForward();
        SlowDown();
    }
    
    void FastForward()
    {
        if (!Input.GetKeyUp(KeyCode.RightArrow)) return;
        Time.timeScale += 0.5f;
        timeScale = Time.timeScale;
    }
    
    void SlowDown()
    {
        if (!Input.GetKeyUp(KeyCode.LeftArrow)) return;
        Time.timeScale -= 0.5f;
        timeScale = Time.timeScale;
    }
}