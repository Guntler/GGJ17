using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour {

    public GameObject Bullet;
    public float ReloadTime;
    public bool HasBullet = true;
    public float BulletSpeed;

    private float elapsedTime;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 lastDir = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start () {
		m_Cam = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        var shootDir = Vector3.zero;
        // read inputs
        float h = Input.GetAxis("Horizontal") * 5f;
        float v = Input.GetAxis("Vertical") * 5f;

        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            shootDir = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            shootDir = v * Vector3.forward * -1 + h * Vector3.right * -1;
        }

        shootDir.Normalize();

        if (HasBullet && Input.GetButtonDown("Fire1"))
        {
            HasBullet = false;
            elapsedTime = 0;

            var bulletPos = new Vector3(transform.position.x, 1, transform.position.z);

            GameObject bullet = Instantiate(Bullet, bulletPos + new Vector3(lastDir.x, 0, lastDir.z), transform.rotation) as GameObject;

            var behaviour = bullet.GetComponent<BulletBehaviour>();

            behaviour.Direction = new Vector2(lastDir.x, lastDir.z);
            behaviour.Speed = BulletSpeed;
            behaviour.Shooter = gameObject;
        }
        else if(!HasBullet && elapsedTime > ReloadTime)
        {
            HasBullet = true;
        }

        if(shootDir.x != 0 || shootDir.z != 0)
        {
            lastDir = shootDir;
        }
    }
}
