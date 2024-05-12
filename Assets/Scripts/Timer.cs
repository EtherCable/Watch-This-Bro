using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startTime;
    private Text timerText;

    void Start()
    {
        // Record the time when the game starts
        startTime = Time.time;

        // Get the Text component
        timerText = GetComponent<Text>();
    }

    void Update()
    {
        // Calculate the elapsed time
        float t = Time.time - startTime;

        // Convert the time to minutes and seconds
        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        // Display the time
        timerText.text = minutes + ":" + seconds;
    }
}
