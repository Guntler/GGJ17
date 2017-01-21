using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour {

    public GameObject Wave;
    public float TimeToLive;
    public float SpawnInterval;
    public float WaveTimeToLive;
    public float WaveExpandRate;

    private float elapsedTime = 0;
    private float totalTime = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        totalTime += Time.deltaTime;

        if(elapsedTime > SpawnInterval)
        {
            GameObject waveObject = Instantiate(Wave, transform.position, transform.rotation) as GameObject;
            //waveObject.transform.Rotate(new Vector3(1, 0, 0), 90);
            var behaviour = waveObject.GetComponent<WaveBehaviour>();
            behaviour.TimeToLive = WaveTimeToLive;
            behaviour.ExpandRate = WaveExpandRate;

            elapsedTime = 0;
        }

        if (totalTime > TimeToLive)
        {
            Destroy(gameObject);
        }
    }
}
