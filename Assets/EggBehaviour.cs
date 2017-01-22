using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour {

    public GameObject Egg;
    private bool pickedUp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player") && !pickedUp)
        {
            pickedUp = true;
            var follow = Egg.AddComponent<FollowPlayer>();
            Egg.transform.localScale = Egg.transform.localScale * 0.5f;
            Egg.AddComponent<FloatEggBehaviour>();

            follow.Target = obj.gameObject;
            follow.Offset = new Vector3(0, -2, 1);
        }
    }
}
