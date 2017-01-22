using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{

    public GameObject Wave;
    public float TimeToLive;
    public float SpawnInterval;
    public float WaveTimeToLive;
    public float WaveExpandRate;
    public GameObject Origin;

    private float elapsedTime = 0;
    private float totalTime = 0;
    private bool firstWave = true;
    private bool firstUpdate = true;
    private float defaultWaveTimeToLive;
    private float defaultWaveExpandRate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        totalTime += Time.deltaTime;

        if (Origin != null)
        {
            var velocity = Origin.GetComponent<Rigidbody>().velocity;

            if (firstUpdate)
            {
                defaultWaveTimeToLive = WaveTimeToLive;
                defaultWaveExpandRate = WaveExpandRate;
                firstUpdate = false;
            }

            WaveTimeToLive = defaultWaveTimeToLive + velocity.magnitude * 0.15f;
            WaveExpandRate = defaultWaveExpandRate + velocity.magnitude * 0.01f;


            if (elapsedTime > SpawnInterval && velocity.magnitude > 0.1)
            {
                SpawnWave();
                elapsedTime = 0;
            }

            transform.position = new Vector3(Origin.transform.position.x, transform.position.y, Origin.transform.position.z);
        }
        else
        {
            if (elapsedTime > SpawnInterval || firstWave)
            {
                firstWave = false;
                SpawnWave();

                elapsedTime = 0;
            }
        }

        if (TimeToLive > 0 && totalTime > TimeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnWave()
    {
        GameObject waveObject = Instantiate(Wave, transform.position, transform.rotation) as GameObject;
        var behaviour = waveObject.GetComponent<WaveBehaviour>();
        behaviour.Spawner = gameObject;
        behaviour.TimeToLive = WaveTimeToLive;
        behaviour.ExpandRate = WaveExpandRate;
    }
}
