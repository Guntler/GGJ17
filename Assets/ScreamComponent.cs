using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamComponent : MonoBehaviour {

    public List<AudioClip> RoarSFX = new List<AudioClip>();

    AudioSource srcSFX;

    // Use this for initialization
    void Start () {
        srcSFX = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DoScream()
    {
        int r = Random.Range(0, RoarSFX.Count - 1);
        srcSFX.PlayOneShot(RoarSFX[r]);
    }
}
