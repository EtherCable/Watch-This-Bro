using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{


    public GameObject StartPos;
    public GameObject LevelSys;
    public Material skybox= null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelUp()
    {
        this.LevelSys.GetComponent<LevelSystem>().LevelUp();
    }
}
