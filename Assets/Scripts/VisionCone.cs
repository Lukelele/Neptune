using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// The VisionCone class represents a vision cone that can be attached onto and shark or a flock.
/// </summary>
public class VisionCone : MonoBehaviour
{
    [Header("Vision Cone Settings")]
    [SerializeField] public Transform subject;
    [SerializeField] private List<Transform> seen = new List<Transform>();
    [SerializeField][TagSelector] public string[] targetTags;
    [SerializeField] private bool drawLine;
    
    [Header("Hunting System Settings")]
    
    private Vector3 _randomDir;
    private float _randomDirUpdateTime = -10;
    
    private Transform _target;
    private Collider _targetCollider;
    
    public Transform Target
    {
        get
        {
            if (_target == null) FindClosestTarget();
            return _target;
        }
    }

    public Vector3 TargetDir
    {
        get
        {
            if (Target == null || _targetCollider == null)
            {
                if (Time.time - _randomDirUpdateTime > 3) UpdateRandomDir();
                return _randomDir;
            }
            return _targetCollider.ClosestPoint(subject.position) - subject.position;
        }
    }

    void Start()
    {
        if (subject == null) subject = transform;
    }
    
    /// <summary>
    /// Finds the closest target from the seen list.
    /// </summary>
    void FindClosestTarget()
    {
        Transform tMin = null;
        Collider tMinCollider = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = subject.transform.position;
        for (int i = seen.Count - 1; i >= 0; i--)
        {
            var t = seen[i];
            if (t == null)
            {
                seen.RemoveAt(i);
                continue;
            }
            if (!Array.Exists(targetTags, element => t.CompareTag(element))) continue;
            //float dist = Vector3.Distance(t.position, currentPos);
            tMinCollider = t.GetComponent<Collider>();
            Vector3 point = tMinCollider.ClosestPoint(currentPos);
            float dist = Vector3.Distance(point, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        _target = tMin;
        _targetCollider = tMinCollider;
    }
    
    private void OnTriggerExit(Collider other)
    {
        seen.Remove(other.transform);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        seen.Add(other.transform);
    }
    
    private void FixedUpdate()
    {
        if (drawLine) Debug.DrawRay(subject.position, TargetDir, Color.green);
    }
    
    /// <summary>
    /// Updates the random direction of the shark.
    /// </summary>
    private void UpdateRandomDir()
    {
        float randomRightMagnitude = Random.Range(0.5f, 1f) * Random.Range(-1, 2);
        while (randomRightMagnitude == 0) randomRightMagnitude = Random.Range(0.5f, 1f) * Random.Range(-1, 2);
        float randomUpMagnitude = Random.Range(0.5f, 1f) * Random.Range(-1, 2);
        _randomDir = randomRightMagnitude*subject.transform.right + randomUpMagnitude*subject.transform.up;
        //_randomDir = new Vector3(2, 0, 0);
        _randomDirUpdateTime = Time.time;
    }
}
