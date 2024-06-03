using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenuUI; // Assign your Canvas object in the inspector

    void Start ()
    {
        EnableChildren(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0; // Pauses the game
                EnableChildren(true); // Activates the children of the pause menu
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1; // Resumes the game
                EnableChildren(false); // Deactivates the children of the pause menu
                Cursor.visible = false;
            }
        }
    }

    void EnableChildren(bool isEnabled)
    {
        foreach (Transform child in pauseMenuUI.transform)
        {
            child.gameObject.SetActive(isEnabled);
            pauseMenuUI.transform.Find("Keybinds").gameObject.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1; // Resumes the game
        EnableChildren(false); // Deactivates the children of the pause menu
    }
    public void Restart()
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    public void Keybinds()
    {
        EnableChildren(false);
        //enable canvas child called Keybinds
        pauseMenuUI.transform.Find("Keybinds").gameObject.SetActive(true);
    }
}
