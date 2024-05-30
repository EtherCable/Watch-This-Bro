using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public float powerup_duration = 10.0f;
    public int type = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(GameObject player)
    {
        PlayerController p = player.GetComponent<PlayerController>();
        
        p.PowerUp(type == -1 ? Random.Range(1, 3 + 1) : type, this.powerup_duration);
    //    p.powerup_current(())
    //    p.powerrup_max_time = this.powerup_time;
    //    p.powerup_time = 0.0f;
    //    p.powerup_current = type == -1 ? Random.Range(1, 3) : type;
				//Debug.Log("applied powerup:  "+p.powerup_current);

        this.gameObject.SetActive(false);
        this.GetComponentInParent<Respawner>().Kill();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Use(other.gameObject);
        }
    }
}
