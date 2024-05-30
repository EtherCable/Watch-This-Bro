using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{

    public int current_level = 0;
    public List<GameObject> levels;
    public GameObject player;
	//public AudioClip sound;
    public int starting_lives = 5;
    public Material default_skybox;
    



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }
        Debug.Log("level sys start. loading level " + current_level);
        //if (lvl == -1) lvl = this.current_level;
        GameObject nlvl = this.levels[Globals.current_level];
        Level lvl = nlvl.GetComponent<Level>();
        RenderSettings.skybox = lvl.skybox != null ? lvl.skybox : this.default_skybox;
        //GameObject nlvl = this.levels[lvl];

        //olvl.SetActive(false);
        nlvl.SetActive(true);
        Vector3 sp = lvl.StartPos.transform.position;
        this.player.transform.position = sp;

        PlayerController p = this.player.GetComponent<PlayerController>();
        p.spawnPoint = sp;
        p.level_sys = this;
        p.lives = starting_lives;
        p.timer = 0f;

        //LoadLevel(current_level);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUp()
    {
        this.LoadLevel(Globals.current_level+1);
		//player.GetComponent<AudioSource>().PlayOneShot(sound, 1.0f);
    }

    public void LoadLevel(int lvl = -1)
    {
       if(lvl != -1)
        {
            Globals.current_level = lvl;
        }
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);



    }
}
