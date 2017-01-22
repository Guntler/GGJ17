using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamComponent : MonoBehaviour {

    public List<AudioClip> RoarSFX = new List<AudioClip>();
    public float Cooldown;
    public GameObject WaveSpawner;

    private float elapsedTime;

    AudioSource srcSFX;

    // Use this for initialization
    void Start () {
        srcSFX = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > Cooldown && Input.GetButtonDown("Scream"))
        {
            var spawner = Instantiate(WaveSpawner, transform.position, transform.rotation) as GameObject;
            var behaviour = spawner.GetComponent<SpawnerBehaviour>();

            behaviour.WaveTimeToLive = 5;
            behaviour.TimeToLive = 3;
            behaviour.WaveExpandRate = 1.3f;
            DoScream();
            elapsedTime = 0;
        }
	}

    void DoScream()
    {
        int r = Random.Range(0, RoarSFX.Count - 1);
        srcSFX.PlayOneShot(RoarSFX[r]);
    }
}
