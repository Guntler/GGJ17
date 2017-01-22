using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : SpawnerBehaviour
{
    float maxDivergence = 1;

    public void Start()
    {
    }

    protected override void SpawnWave()
    {
        GameObject waveObject = Instantiate(Wave, transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
        var behaviour = waveObject.GetComponent<WaveBehaviour>();
        behaviour.ResetToOrigin = false;
        behaviour.Spawner = gameObject;
        behaviour.TimeToLive = WaveTimeToLive;
        behaviour.ExpandRate = WaveExpandRate;
    }
}