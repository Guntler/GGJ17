using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeComponent : MonoBehaviour {

    public bool Lunging = false;
    public bool Charging = false;
    public Vector2 Direction;
    public int Damage;
    public float Speed;
    public float ChargeTime;

    private float elapsedTime;
    private Vector2 lungeDirection;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 lastDir = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start () {
        m_Cam = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        var lungeDir = Vector3.zero;
        // read inputs
        float h = Input.GetAxis("Horizontal_Mnst") * 5f;
        float v = Input.GetAxis("Vertical_Mnst") * 5f;

        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            lungeDir = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            lungeDir = v * Vector3.forward * -1 + h * Vector3.right * -1;
        }

        lungeDir.Normalize();

        if (Lunging)
        {
            print(gameObject.tag);
            var rigid = gameObject.GetComponent<Rigidbody>();

            lungeDirection.Normalize();
            Vector3 force = new Vector3(lungeDirection.x * Speed, 0, lungeDirection.y * Speed);
            force = Vector3.ClampMagnitude(force, 1000.0f);
            rigid.AddForce(force, ForceMode.VelocityChange);
        }
        else if(Charging && elapsedTime > ChargeTime)
        {
            Charging = false;
            Lunging = true;
            lungeDirection = new Vector2(lastDir.x, lastDir.z);
        }
        else if(Input.GetButtonDown("Fire2") && !Charging)
        {
            Charging = true;
            elapsedTime = 0;
        }
        else if(Input.GetButtonUp("Fire2"))
        {
            Charging = false;
        }

        if (lungeDir.x != 0 || lungeDir.z != 0)
        {
            lastDir = lungeDir;
        }

        elapsedTime += Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        print("collided");
        if (Lunging && other.gameObject.GetComponent<HealthBehaviour>() != null)
            other.gameObject.GetComponent<HealthBehaviour>().Health -= Damage;
        Lunging = false;
    }
}
