using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private Creature creature;
    public List<Transform> seen = new List<Transform>();
    private float timer = 0;
    private float randomRightMagnitude;
    private float randomUpMagnitude;

    public int numFoodSeen
    {
        get
        {
            int count = 0;
            for (int i = seen.Count - 1; i >= 0; i--)
            {
                var t = seen[i];
                if (t == null)
                {
                    seen.RemoveAt(i);
                    continue;
                }
                if (t.CompareTag("Food")) count++;
            }
            return count;
        }
    }

    public float visionDistance
    {
        get => _visionDistance;
        set
        {
            if (value < 0) value = 0;
            Vector3 currentScale = transform.localScale;
            Vector3 currentPos = transform.localPosition;
            transform.localScale = new Vector3(currentScale.x, value, currentScale.y);
            transform.localPosition = new Vector3(currentPos.x, 0, currentPos.z);
            _visionDistance = value;
        }
    }
    private float _visionDistance = 0;
    
    public float visionRadius
    {
        get => _visionRadius;
        set
        {
            if (value < 0) value = 0;
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(value/2, currentScale.y, value);
            _visionRadius = value;
        }
    }
    private float _visionRadius = 0;

    public Transform closetFood
    {
        get
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = creature.transform.position;
            for (int i = seen.Count - 1; i >= 0; i--)
            {
                var t = seen[i];
                if (t == null)
                {
                    seen.RemoveAt(i);
                    continue;
                }
                if (!t.CompareTag("Food")) continue;
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }

            return tMin;
        }
    }

    public Vector3 closetFoodDir
    {
        get
        {   
            if (closetFood == null) return randomDir;
            //if (closetFood == null) return creature.transform.forward;
            Vector3 foodPos = closetFood.position;
            Vector3 currentPos = creature.transform.position;
            return foodPos - currentPos;
        }
    }
    
    public Vector3 randomDir => randomRightMagnitude*creature.transform.right + randomUpMagnitude*creature.transform.up;

    public Vector3 closetTerrainDir
    {
        get
        {
            Transform tMin = null;
            Vector3 tPos = Vector3.zero;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = creature.transform.position;
            for (int i = seen.Count - 1; i >= 0; i--)
            {
                var t = seen[i];
                if (t == null)
                {
                    seen.RemoveAt(i);
                    continue;
                }

                if (!t.CompareTag("Terrain")) continue;
                tPos = GetComponent<Collider>().ClosestPoint(t.position);
                float dist = Vector3.Distance(tPos, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }

            if (tMin == null) return -transform.forward;
            return tPos - currentPos;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        seen.Remove(other.transform);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        seen.Add(other.transform);
        //if (other.CompareTag("Food")) Debug.Log("Food");
        //if (other.CompareTag("Terrain")) Debug.Log("Terrain");
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            randomRightMagnitude = Random.Range(0.5f, 1f) * Random.Range(-1, 2);
            while (randomRightMagnitude == 0) randomRightMagnitude = Random.Range(0.5f, 1f) * Random.Range(-1, 2);
            randomUpMagnitude = Random.Range(0.5f, 1f) * Random.Range(-1, 2);
            //Debug.Log(randomRightMagnitude + " " + randomUpMagnitude);
            timer = 3;
        }
    }
}
