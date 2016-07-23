using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TagManager : MonoBehaviour {

    
    GameManager gameManager;


    void Start()
    {
        

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	void Update () {
        
	}
}
