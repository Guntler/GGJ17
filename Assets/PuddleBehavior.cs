using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleBehavior : SpawnerBehaviour {
    Bounds b;
    float maxDivergence = 1;

    public void Start()
    {
        b = GetComponent<Renderer>().bounds;
    }

    protected override void SpawnWave()
    {
        float x = Random.Range(-maxDivergence, maxDivergence);
        float y = Random.Range(-maxDivergence, maxDivergence);
        var pos = transform.position + new Vector3(x, 0, y);
        GameObject waveObject = Instantiate(Wave, pos, new Quaternion(0,0,0,0)) as GameObject;
        var behaviour = waveObject.GetComponent<WaveBehaviour>();
        behaviour.ResetToOrigin = false;
        behaviour.Spawner = gameObject;
        behaviour.TimeToLive = WaveTimeToLive;
        behaviour.ExpandRate = WaveExpandRate;
    }
}
