﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public GameObject Shooter;
    public Vector2 Direction;
    public float Speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var rigid = gameObject.GetComponent<Rigidbody>();

        Direction.Normalize();
        rigid.velocity += new Vector3(Direction.x*Speed, 0, Direction.y*Speed);
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 20.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<HealthBehaviour>().Health--;
        Destroy(gameObject);
    }
}
