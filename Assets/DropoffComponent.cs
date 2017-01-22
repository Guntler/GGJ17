using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropoffComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider obj)
    {
        print("collided ");
        print(obj.tag);
        if (obj.gameObject.CompareTag("Egg"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
