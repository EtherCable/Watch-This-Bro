using System.Collections;
using UnityEngine;
using TMPro;

public class RainbowText : MonoBehaviour
{
    public float speed = 1.0f;
    public TextMeshProUGUI text;
    private Color currentColor = Color.red;
    private void Start()
    {
        StartCoroutine(ChangeColor());
    }

    public void StarColorChange()
    {
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            if (text!= null) // Check if text is not null
            {
                for (float i = 0; i < 1; i += Time.deltaTime * speed)
                {
                    text.color = Color.Lerp(currentColor, GetNextColor(currentColor), i);
                    yield return null;
                }
                currentColor = GetNextColor(currentColor);
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found.");
                yield break; // Exit the coroutine if text is null
            }
        }
    }


    Color GetNextColor(Color currentColor)
    {
        if (currentColor == Color.red)
            return Color.magenta;
        if (currentColor == Color.magenta)
            return Color.cyan;
        if (currentColor == Color.cyan)
            return Color.green;
        if (currentColor == Color.green)
            return Color.yellow;
        if (currentColor == Color.yellow)
            return Color.red;
        return Color.red;
    }
}
