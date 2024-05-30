using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public float respawn_time= 15f;
    public float respawn_timer;
    public bool is_dead = false;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        this.respawn_timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
       if(is_dead)
        {
            if(respawn_timer >=respawn_time)
            { 
                target.SetActive(true);
                this.is_dead = false;
            } else
            {
                this.respawn_timer += Time.deltaTime;
            }
           
        }
    }

    public void Kill()
    {
        target.SetActive(false);
        this.respawn_timer = 0f;
        is_dead = true;
    }
}
