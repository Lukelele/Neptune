using UnityEngine;
using System.Globalization;

public class Stats : MonoBehaviour
{
    private float timeCounter;
    public float timeInterval;
    
    private void Start()
    {
        timeCounter = 0;
    }
    
    private void FixedUpdate()
    {
        timeCounter -= Time.fixedUnscaledDeltaTime;
        if (timeCounter <= 0)
        {
            timeCounter = timeInterval;
            FlockUnit[] creatures = FindObjectsOfType<FlockUnit>();
            
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
            Plant[] foods = FindObjectsOfType<Plant>();
            content2 += time + "," + creatures.Length + "," + foods.Length + "\n";
            if (!System.IO.File.Exists(path2)) System.IO.File.WriteAllText(path2, "");
            System.IO.File.AppendAllText(path2, content2);
        }
    }
}