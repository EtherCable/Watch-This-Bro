using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    private float displayDistance = 11f; // The distance within which the text will be displayed

    void Update()
    {
        // Find all TextMeshPro objects with the "HoverText" tag
        GameObject[] texts = GameObject.FindGameObjectsWithTag("HoverText");

        foreach (GameObject textObject in texts)
        {
            TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();

            // Calculate the distance between the player and the text
            float distanceToText = Vector3.Distance(transform.position, textObject.transform.position);

            // If the text is close enough, display it
            if (distanceToText <= displayDistance)
            {
                textMesh.enabled = true;
            }
            // Otherwise, hide it
            else 
            {
                textMesh.enabled = false;
            }
        }
    }
}
