using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEggBehavior : MonoBehaviour {

    int _multiplier = 1;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0,1,0), 10.0f*Time.deltaTime);
        if(transform.position.y >= 1.75f)
        {
            _multiplier = -1;
        }
        else if (transform.position.y <= 1.25f)
        {
            _multiplier = 1;
        }
        transform.position = Vector3.LerpUnclamped(transform.position, transform.position + new Vector3(0, Time.deltaTime * 2.0f * _multiplier, 0),Time.deltaTime*2.0f);
	}
}
