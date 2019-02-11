using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}

   
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "Choose")
        {
            Destroy(gameObject);
        }
	}

}
