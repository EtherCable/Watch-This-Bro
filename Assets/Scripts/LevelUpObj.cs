using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUpObj : MonoBehaviour
{
    //private string sceneName; // Declare sceneName at the class level

    public LevelSystem level_system;
    // Start is called before the first frame update
    void Start()
    {
        //sceneName = SceneManager.GetActiveScene().name; // Initialize sceneName
        //Debug.Log("Current scene: " + sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    if (sceneName == "TutorialLevel") {
        //        SceneManager.LoadScene("Level1"); // Load the next scene
        //    }
        //    else if (sceneName == "Level1") {
        //        SceneManager.LoadScene("Level2"); // Load the next scene
        //    }
        //}
        ///*
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            level_system.LevelUp();
        }
        //*/
    }
}
