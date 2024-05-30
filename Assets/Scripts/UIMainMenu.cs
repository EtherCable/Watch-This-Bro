using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{


    public void PlayGame()
    {
        Globals.current_level = 1;
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayTutorial()
    {
        Globals.current_level = 0;
        SceneManager.LoadScene(1);
    }
}
