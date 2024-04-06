using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum Dimension
{
    XY,
    XYZ,
}


public class Predator : Creature
{
    [Header("Predator Settings")]
    [SerializeField] private List<string> seekingTags;
    [SerializeField] private Dimension motionDimension;
    [SerializeField] private bool drawVision;
    [SerializeField] public float turningRate;
    public VisionCone vision;
    private Transform _target;
    public int foodCount;


    public Net brain;
    // Start is called before the first frame update
    void Start()
    {
        drawVision = true;
        brain = new Net(new[] { 2, 2 });
        age = 0;
        turningRate = 3;

        //for (int i = 0; i < brain.layers[0].size; i++)
        //{
        //    for (int j = 0; j < brain.layers[1].size; j++)
        //    {
        //        brain.layers[1].weights[i, j] = 0;
        //    }
        //}
        //brain.layers[1].weights[0,0] = 1.0f;
        //brain.layers[1].weights[1,1] = 1.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (!isAlive) return;
        
        age += Time.deltaTime;
        
        _target = vision.closetFood;
        
        // Draw vision lines
        if (drawVision)
        {
            Debug.DrawRay(transform.position, vision.closetFoodDir, Color.green);
            Debug.DrawRay(transform.position, vision.closetTerrainDir, Color.red);
        }
        
        Quaternion foodLookRotation = Quaternion.LookRotation(vision.closetFoodDir);
        Vector3 foodRotationDifference = foodLookRotation.eulerAngles - transform.rotation.eulerAngles;
        
        foodRotationDifference /= 360;
        
        
        float[] inputs = { foodRotationDifference.x, foodRotationDifference.y };
        
        brain.FeedForward(inputs);
        float[] outputs = brain.GetOutput();
        Vector3 angleToRotate = new Vector3(outputs[0], outputs[1], 0);
        angleToRotate *= 360;
        
        // switch (motionDimension)
        // {
        //     case Dimension.XY:
        //         transform.Rotate(0, angleToRotate.y*Time.timeScale*Time.deltaTime, 0);
        //         break;
        //     case Dimension.XYZ:
        //         Quaternion final = Quaternion.Euler(transform.eulerAngles + angleToRotate);
        //         transform.localRotation = Quaternion.Slerp(transform.localRotation, final, Time.deltaTime*Time.timeScale);
        //         transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //         break;
        // }

        // QuaternionMethod();
        
        Move();
    }


    void QuaternionMethod()
    {
        Quaternion rotation = Quaternion.LookRotation(vision.closetFoodDir);
        Quaternion diff = rotation * Quaternion.Inverse(transform.rotation);
        //Debug.Log(diff);
        
        float[] inputs = { diff.x, diff.y, diff.z, diff.w };
        
        brain.FeedForward(inputs);
        float[] outputs = brain.GetOutput();
        
        Quaternion outputDiff = new Quaternion(outputs[0], outputs[1], outputs[2], outputs[3]);
        //Debug.Log(outputDiff);
        
        switch (motionDimension)
        {
            case Dimension.XY:
                transform.Rotate(0, outputDiff.y*Time.timeScale*Time.deltaTime, 0);
                break;
            case Dimension.XYZ:
                //transform.Rotate((outputs[0]-0.5f)*Time.timeScale, (outputs[1]-0.5f)*Time.timeScale, (outputs[2]-0.5f)*Time.timeScale);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, outputDiff*transform.localRotation, Time.timeScale*Time.deltaTime);
                break;
        }
    }

    void Move()
    {
        transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
    }

    // float[] SeekingFood()
    // {
    //     float[] inputs = new float[_totalSeekingAngles];

    //     foreach (float y in seekingAnglesY)
    //     {
    //         foreach (float x in seekingAnglesX)
    //         {
    //             Quaternion angleX = Quaternion.AngleAxis(-x, transform.up);
    //             Quaternion angleY = Quaternion.AngleAxis(-y, transform.right);

    //             Vector3 origin = transform.position;
    //             Vector3 dir = angleX * angleY *  transform.forward;
    //             RaycastHit hit;
    //             int index = seekingAnglesY.IndexOf(y) * seekingAnglesX.Count + seekingAnglesX.IndexOf(x);
    //             if (Physics.Raycast(origin, dir, out hit, rayLength) && hit.collider.CompareTag("Food"))
    //             {
    //                 float distance = Vector3.Distance(transform.position, hit.transform.position);
    //                 inputs[index] = distance/rayLength;
    //                 Debug.DrawRay(origin, dir * distance, Color.red);
    //             }
    //             else
    //             {
    //                 inputs[index] = 1;
    //                 Debug.DrawRay(origin, dir * rayLength, Color.yellow);
    //             }
    //         }
    //     }
    //     return inputs;
    // }
    
    protected override void Reproduce()
    {
        //if (outputs[3] < 0.5f) return;
        energy -= energyForReproduction * 0.5f;
        
        GameObject newPredator = Instantiate(gameObject, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Predator newPredatorScript = newPredator.GetComponent<Predator>();
        newPredatorScript.brain = new Net(brain.CopyLayers());
        newPredatorScript.Mutate();
        newPredator.GetComponent<Creature>().generation = generation + 1;
        newPredator.name = "Predator " + newPredator.GetComponent<Creature>().generation;
        newPredator.GetComponent<MeshRenderer>().material.color = new Color(1, 0, generation * 0.1f);
    }
    
    private void Mutate()
    {
        float mutationProbability = Random.Range(0f, 1.0f);
        float mutationMagnitude = Random.Range(0f, 1.0f);

        brain.MutateNet(mutationProbability, mutationMagnitude);
    }
}
