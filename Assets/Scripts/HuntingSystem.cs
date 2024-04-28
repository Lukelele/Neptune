using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingSystem: Net
{
    private Quaternion _startRotation = Quaternion.identity;
    private Quaternion _huntingRotation = Quaternion.identity;
    private float _turningSpeed = 1f;
    private float _t;
    
    public HuntingSystem(int[] networkStructure) : base(networkStructure){}
    public HuntingSystem(Layer[] newLayers) : base(newLayers){}
    
    public void FeedForward(Transform transform, VisionCone vision)
    {
        
        Vector3 targetDir = vision.TargetDir;
        Transform target = vision.Target;
        Quaternion foodLookRotation = transform.rotation;
        if (targetDir != Vector3.zero) foodLookRotation = Quaternion.LookRotation(targetDir);
        Vector3 foodRotationDifference = foodLookRotation.eulerAngles - transform.eulerAngles;
        //Debug.Log("foodLookRotation: " + foodLookRotation.eulerAngles);
        //Debug.Log("transform.rotation: " + transform.eulerAngles);
        //Debug.Log("foodRotationDifference: " + foodRotationDifference);
        
        foodRotationDifference.x = ((foodRotationDifference.x + 180) % 360 - 180) / 180;
        foodRotationDifference.y = ((foodRotationDifference.y + 180) % 360 - 180) / 180;
        
        float[] inputs = { foodRotationDifference.x, foodRotationDifference.y };
        
        base.FeedForward(inputs);
        float[] outputs = GetOutput();
        Vector3 angleToRotate = new Vector3(outputs[0], outputs[1], 0);
        angleToRotate *= 180;
        
        _startRotation = transform.rotation;
        _huntingRotation = Quaternion.Euler(angleToRotate.x + transform.eulerAngles.x, angleToRotate.y + transform.eulerAngles.y, 0);
        _huntingRotation.eulerAngles = new Vector3(_huntingRotation.eulerAngles.x, _huntingRotation.eulerAngles.y, 0);
        _t = 0;
    }
    
    public void Rotate(Transform transform, float timeBetweenFeedForward=1f)
    {
        Debug.DrawRay(transform.position, _huntingRotation * Vector3.forward, Color.red);
        if (_t > 0.98f) _t = 0.99f;
        transform.rotation = Quaternion.Lerp(_startRotation, _huntingRotation, _t);
        _t += Time.deltaTime * Time.timeScale * _turningSpeed / timeBetweenFeedForward;
    }
    
    public void Mutate()
    {
        MutateNet(Random.Range(0f, 1.0f), Random.Range(0f, 1.0f));
    }
}
