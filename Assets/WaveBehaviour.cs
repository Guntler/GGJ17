using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {

    public float TimeToLive;
    public float ExpandRate;
    public GameObject Spawner;

    private float elapsedTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

		if(elapsedTime > TimeToLive)
        {
            Destroy(gameObject);
        }

        float newScaleX = transform.localScale.x + Time.deltaTime * ExpandRate * transform.localScale.x;
        float newScaleZ = transform.localScale.z + Time.deltaTime * ExpandRate * transform.localScale.z;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, newScaleZ);

        if (Spawner != null)
            transform.position = Spawner.transform.position;
	}
}
