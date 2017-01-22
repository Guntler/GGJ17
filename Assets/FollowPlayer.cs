using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public GameObject Target;
    public Vector3 Offset = Vector3.zero;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Target.transform.position.x - Offset.x, Target.transform.position.y - Offset.y, Target.transform.position.z - Offset.z);
	}
}
