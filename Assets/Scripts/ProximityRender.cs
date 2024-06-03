using UnityEngine;

public class ProximityRender : MonoBehaviour
{
    public GameObject player;
    public float renderDistance = 5f;

    void Update()
    {
        // Calculate the distance between the player and the parent object
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Loop through each child of the parent object
        foreach (Transform child in transform)
        {
            Renderer renderer = child.gameObject.GetComponent<Renderer>();

            // Check if the Renderer component exists
            if (renderer != null)
            {
                // If the distance is less than the threshold, enable rendering
                if (distance < renderDistance)
                {
                    renderer.enabled = true;
                }
                // If the distance is greater than the threshold, disable rendering
                else
                {
                    renderer.enabled = false;
                }
            }
        }
    }
}
