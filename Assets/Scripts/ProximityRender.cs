using UnityEngine;
using System.Collections.Generic;


public class DistanceCheck : MonoBehaviour
{
    public float maxDistance = 45f;
    public LevelSystem level_sys;
    private string curr_level_str;
    private List<string> tags = new List<string> { "Platform", "MovingPlatform", "Spike" };

    private Dictionary<string, GameObject[]> objectsByTag = new Dictionary<string, GameObject[]>();

    void Start()
    {
        if (level_sys.current_level == 0) {
            curr_level_str = "Level0";
        }
        else if (level_sys.current_level == 1) {
            curr_level_str = "Level1";
        }
        else if (level_sys.current_level == 2) {
            curr_level_str = "Level2";
        }

        foreach (var tag in tags)
        {
            objectsByTag[tag] = GameObject.FindGameObjectsWithTag(tag);
        }
    }

    void Update()
    {
        foreach (var tag in tags)
        {
            foreach (GameObject obj in objectsByTag[tag])
            {
                Transform parent = obj.transform.parent;
                while (parent != null)
                {
                    if (parent.name == curr_level_str)
                    {
                        float distance = Vector3.Distance(transform.position, obj.transform.position);
                        obj.GetComponent<Renderer>().enabled = distance <= maxDistance;
                        break;
                    }
                    parent = parent.parent;
                }
            }
        }
    }
}
