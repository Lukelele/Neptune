using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public List<float[]> stats;

    private float timeCounter;
    public float timeInterval;

    private void Start()
    {
        stats = new List<float[]>();
        timeCounter = 0;
    }
    
    private void FixedUpdate()
    {
        timeCounter -= Time.fixedDeltaTime;
        if (timeCounter <= 0)
        {
            timeCounter = timeInterval/Time.timeScale;
            Creature[] creatures = FindObjectsOfType<Creature>();
            
            string path = "Assets/GenStats.txt";
            string content = "";
            string time = Time.time.ToString(CultureInfo.InvariantCulture);
            foreach (var creature in creatures)
            {
                //stats.Add(new float[]{Time.time, creature.generation});
                Debug.Log("Generation " + creature.generation + " at " + time);
                content += time + "," + creature.generation + "\n";
            }
            if (!System.IO.File.Exists(path)) System.IO.File.WriteAllText(path, "");
            System.IO.File.AppendAllText(path, content);
            
            string path2 = "Assets/FoodPopulationStats.txt";
            string content2 = "";
            Food[] foods = FindObjectsOfType<Food>();
            content2 += time + "," + creatures.Length + "," + foods.Length + "\n";
            if (!System.IO.File.Exists(path2)) System.IO.File.WriteAllText(path2, "");
            System.IO.File.AppendAllText(path2, content2);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            SaveStats();
        }
    }

    public void SaveStats()
    {
        Debug.Log("Saving stats...");
        string path = "Assets/Stats.txt";
        string content = "";
        for (int i = 0; i < stats.Count; i++)
        {
            content += stats[i][0] + "," + stats[i][1] + "\n";
        }
        System.IO.File.WriteAllText(path, content);
        Debug.Log("Stats saved to " + path);
    }
}
