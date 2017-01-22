using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : SpawnerBehaviour
{
    float maxDivergence = 1;
    public bool IsActive = true;

    public void Start()
    {
    }

    protected override void SpawnWave()
    {
        if(IsActive)
        {
            GameObject waveObject = Instantiate(Wave, transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
            var behaviour = waveObject.GetComponent<WaveBehaviour>();
            behaviour.ResetToOrigin = false;
            behaviour.Spawner = gameObject;
            behaviour.TimeToLive = WaveTimeToLive;
            behaviour.ExpandRate = WaveExpandRate;
        }
    }
}