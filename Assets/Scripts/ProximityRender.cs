using UnityEngine;
using System.Collections.Generic;


public class DistanceCheck : MonoBehaviour
{
    public float maxDistance = 45f;
    private string curr_level_str;
    private List<string> tags = new List<string> { "Platform", "MovingPlatform", "Spike" };

    private Dictionary<string, GameObject[]> objectsByTag = new Dictionary<string, GameObject[]>();

    void Start()
    {
        GameObject level0 = GameObject.Find("Level0");
		GameObject level1 = GameObject.Find("Level1");
		GameObject level2 = GameObject.Find("Level2");
        if (level0 != null) {
            curr_level_str = "Level0";
        }
        else if (level1 != null) {
            curr_level_str = "Level1";
        }
        else if (level2 != null) {
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
