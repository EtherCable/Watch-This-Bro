using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimGif : MonoBehaviour {

    [SerializeField] private Texture2D[] frames;
    [SerializeField] private  float fps = 20.0f;

    private Material mat;

    void Start () {
        mat = GetComponent<Renderer> ().material;

        // Load the images from the Resources folder
        frames = new Texture2D[72];
        for (int i = 1; i <= 72; i++)
        {
            frames[i - 1] = Resources.Load<Texture2D>("rock" + i);
        }
        mat.mainTextureOffset = new Vector2(-0.2f, 0f);
    }

    void Update () {
        int maxIndex = frames.Length - 1;
        int cycles = (int)(Time.time * fps / maxIndex);
        int direction = cycles % 2 == 0 ? 1 : -1;
        int index = (int)(Time.time * fps) % maxIndex;
        if (direction == -1) {
            index = maxIndex - 1 - index;
        }
        mat.mainTexture = frames[index];
    }
}
