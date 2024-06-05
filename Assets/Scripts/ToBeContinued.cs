using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tobecontinuedMenu : MonoBehaviour
{
    public Canvas tobecontinuedMenuUI;
    public PlayerController player;
    public AudioSource audioSource; // Declare the AudioSource variable
    public AudioClip audioClip;

    void Start ()
    {
        audioSource.clip = audioClip;
        EnableChildren(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EnableChildren(bool isEnabled)
    {
        foreach (Transform child in tobecontinuedMenuUI.transform)
        {
            child.gameObject.SetActive(isEnabled);
        }
    }

    


}
