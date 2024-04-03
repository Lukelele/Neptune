using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class flock : MonoBehaviour
{

    // Movement params
    public float speed = 0.001f;
    float rotationspeed = 2.0f;

    Vector3 averageHeading;
    Vector3 averagePosition;

    float neighbourDistance = 3.0f;

    bool turning = false;

    private FlockLeader leader;

    // Avoidance params
    public LayerMask Obstacles;
    public float avoidanceDistance = 2f;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.3f, 1);
        leader = FindObjectOfType<FlockLeader>();
        Debug.Log(leader);
    }

    // Update is called once per frame
    void Update()
    {
        // if(Vector3.Distance(transform.position, Vector3.zero) >= leader.tankSize)
        // {
        //     turning = true;
        // }
        // else { turning = false; }

        Debug.DrawRay(transform.position, transform.forward * avoidanceDistance, Color.green);

        if(turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationspeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        else {
            if (Random.Range(0, 3) < 1) {
                ApplyRules();
            }
        }
        
        transform.Translate(0, 0, Time.deltaTime * speed);
        
    }

    void ApplyRules()
    {
        float cohesionWeight = leader.cohesionWeight;
        float alignmentWeight = leader.alignmentWeight;
        float avoidanceWeight = leader.avoidanceWeight;

        GameObject[] gos;
        gos = leader.fishList;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = leader.transform.position;

        float dist;

        float predatorDistanceThreshold = 4f; 

        int groupSize = 0;
        
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);

                // Predator and neighbour avoidance
                // Terrain/object avoidance necessary?

                if (dist < predatorDistanceThreshold && go.CompareTag("Predator"))
                {
                    // Can introduce scaling so fish are more/less prone to avoiding predators
                    vavoid += (this.transform.position - go.transform.position);
                }
                else if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vavoid += (this.transform.position - go.transform.position);
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed += anotherFlock.speed; //Group speed for dynamism
                    
                }

            }
        }

        if(groupSize>0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            
            // Scaled speed
            // speed = (gSpeed / groupSize);

            // Constant speed for debug
            speed = leader.flockSpeed;

            // Calculate cohesion, alignment, and avoidance vectors
            // Vector3 cohesionVector = (vcentre - goalPos - transform.position);
            // Vector3 avoidanceVector = (vavoid);
            // Vector3 alignmentVector = (goalPos);

            Vector3 cohesionVector = (vcentre + (goalPos - transform.position));
            Vector3 avoidanceVector = (vavoid);
            Vector3 alignmentVector = (goalPos).normalized;

            // Apply weights to influence the directions
            Vector3 direction = (cohesionVector * cohesionWeight + avoidanceVector * avoidanceWeight + alignmentVector * alignmentWeight) - transform.position;
            AvoidPredators();

            // Unweighted 
            // Vector3 direction = (vcentre + vavoid) - transform.position;
            // AvoidPredators();


            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationspeed * Time.deltaTime);
            }
        }


        void AvoidPredators()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, avoidanceDistance, Obstacles))
            {
                // steering
                Vector3 avoidanceDirection = transform.position - hit.transform.position;

                // steer away from predactor
                Vector3 desiredDirection = (direction - transform.position).normalized;
                Vector3 steerDirection = (desiredDirection + avoidanceDirection.normalized).normalized;

                // target position to move in the steering direction
                direction = transform.position + steerDirection * avoidanceDistance;
            }
        }
    }
}
