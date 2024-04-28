using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotationTest : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.up, Space.Self);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right, Space.Self);
        }
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.up, Space.World);
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.right, Space.World);
        }
        
        //if (target == null) return;
        //Vector3 targetDir = target.position - transform.position;
        //Debug.DrawRay(transform.position, targetDir, Color.green);
        //Quaternion foodLookRotation = Quaternion.LookRotation(targetDir);
        //transform.rotation = foodLookRotation;
        //Debug.DrawRay(transform.position, foodLookRotation * Vector3.forward, Color.red);
        //Debug.Log(foodLookRotation.eulerAngles);
        
        
        //Debug.Log(transform.rotation.eulerAngles + " " + transform.eulerAngles);
    }
}
