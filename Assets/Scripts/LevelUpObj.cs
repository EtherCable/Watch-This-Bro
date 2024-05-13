using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpObj : MonoBehaviour
{

    public GameObject level;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            this.level.GetComponent<Level>().LevelUp();
        }
    }
}
