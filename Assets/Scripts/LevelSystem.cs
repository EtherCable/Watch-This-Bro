using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{

    public int current_level = 0;
    public List<GameObject> levels;
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }
        LoadLevel(current_level);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUp()
    {
        this.LoadLevel(this.current_level+1);
        this.current_level++;
    }

    public void LoadLevel(int lvl = -1)
    {
        if (lvl == -1) lvl = this.current_level;
        GameObject olvl = this.levels[this.current_level];
        GameObject nlvl = this.levels[lvl];

        olvl.SetActive(false);
        nlvl.SetActive(true);
        Vector3 sp = nlvl.GetComponent<Level>().StartPos.transform.position;
        this.player.transform.position = sp;
        this.player.GetComponent<PlayerController>().spawnPoint = sp;


        

    }
}
