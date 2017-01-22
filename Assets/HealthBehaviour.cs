using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBehaviour : MonoBehaviour {

    public int Health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Health <= 0)
        {
            if(!gameObject.CompareTag("Player"))
                Destroy(gameObject);
            else
            {
                SceneManager.LoadScene(3);
            }
        }
	}
}
