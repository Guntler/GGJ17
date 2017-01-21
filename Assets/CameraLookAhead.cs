using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAhead : MonoBehaviour {
    public float LookaheadRate = 2.0f;
    public float MaxLookahead = 2.5f;

    private float lookAheadX = 0.0f;
    private float lookAheadZ = 0.0f;

    private Vector3 defaultPos;
    Vector3 velocity;
    // Use this for initialization
    void Start () {
        defaultPos = Camera.main.transform.localPosition;
        velocity = GetComponent<Rigidbody>().velocity;
    }
	
	// Update is called once per frame
	void Update () {
        lookAheadX = velocity.x * Time.deltaTime * LookaheadRate;
        lookAheadZ = velocity.z * Time.deltaTime * LookaheadRate;

        if (velocity.magnitude > 0)
        {
            Vector3 newPos = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(lookAheadX, 0, lookAheadZ), Time.deltaTime*2.0f);
            if(velocity.x > 0) 
                newPos.x = Mathf.Clamp(newPos.x, defaultPos.x, defaultPos.x + MaxLookahead);
            else
                newPos.x = Mathf.Clamp(newPos.x, defaultPos.x - MaxLookahead, defaultPos.x);

            if (velocity.z > 0)
                newPos.x = Mathf.Clamp(newPos.z, defaultPos.z, defaultPos.z + MaxLookahead);
            else
                newPos.x = Mathf.Clamp(newPos.z, defaultPos.z - MaxLookahead, defaultPos.z);

            newPos.y = defaultPos.y;
            Camera.main.transform.localPosition = newPos;
            
        }
	}
}
