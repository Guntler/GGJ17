using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {

    public float TimeToLive;
    public float ExpandRate;
    public GameObject Spawner;

    [HideInInspector]
    public bool ResetToOrigin = true;

    private float elapsedTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        elapsedTime += Time.deltaTime;

		if(elapsedTime > TimeToLive)
        {
            Destroy(gameObject);
        }

        float newScaleX = transform.localScale.x + Time.deltaTime * ExpandRate * transform.localScale.x;
        float newScaleZ = transform.localScale.z + Time.deltaTime * ExpandRate * transform.localScale.z;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, newScaleZ);
        if (ResetToOrigin)
        {
            if (Spawner != null)
                transform.position = Spawner.transform.position;
        }
	}
}
