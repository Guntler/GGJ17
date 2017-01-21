using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TPCharacter))]
public class TPCharacterControl : MonoBehaviour {

    private TPCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool m_Sprint;
    private GameObject m_Target;

    private Animation m_Animation;
    public float m_AttackCooldown = 1.0f;
    Vector3 originalCamPos;
    Quaternion originalCamRot;
    private float m_AttackCooldownCount = 0.0f;

    public bool isMonster = false;
    public GameObject WaveSpawner;
    public float BaseSpawnInterval = 0.5f;
    private float elapsedTime = 0;

    public float m_CameraLockOnSpeed = 5.0f;
    // Use this for initialization
    void Start () {
        m_Character = GetComponentInChildren<TPCharacter>();
        m_Animation = GetComponentInChildren<Animation>();
        m_Cam = Camera.main.transform;
        originalCamPos = m_Cam.localPosition;
        originalCamRot = m_Cam.localRotation;
    }
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        var speed = GetComponent<Rigidbody>().velocity.magnitude;
        //print(speed);
        if (elapsedTime > BaseSpawnInterval && speed > 0.1)
        {
            var spawnerPos = new Vector3(transform.position.x, 1, transform.position.z);
            var spawner = Instantiate(WaveSpawner, spawnerPos, new Quaternion(0, 0, 0, 0)) as GameObject;

            var behaviour = spawner.GetComponent<SpawnerBehaviour>();
            behaviour.WaveTimeToLive += behaviour.WaveTimeToLive * speed * 0.1f;
            behaviour.WaveExpandRate += behaviour.WaveExpandRate * speed * 0.1f;
            elapsedTime = 0;
        }

        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }

        if (Input.GetButtonDown("Sprint") && !isMonster)
        {
            m_Sprint = true;
        }
        else if(Input.GetButtonUp("Sprint") && !isMonster)
        {
            m_Sprint = false;
        }

        if (m_Target)
        {
            if((m_Target.transform.position - transform.position).magnitude>5.0f)
            {
                m_Target = null;
            }
            else
            {
                Vector3 hwPoint = (m_Target.transform.position - transform.position) /2;
                hwPoint.z *= -1;
                hwPoint = RotateX(hwPoint, -40);
                m_Cam.localPosition = Vector3.Slerp(m_Cam.localPosition, originalCamPos + hwPoint, Time.deltaTime * m_CameraLockOnSpeed);
            }
        }
        else
        {
            m_Cam.localPosition = Vector3.Lerp(m_Cam.localPosition, originalCamPos, Time.deltaTime * m_CameraLockOnSpeed);
        }
    }


    Vector3 RotateX(Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float ty = v.y;
        float tz = v.z;
        v.y = (cos * ty) - (sin * tz);
        v.z = (cos * tz) + (sin * ty);

        return v;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if(Input.GetButtonDown("Fire3"))
        {

            if (Camera.main != null)
            {
                RaycastHit hitInfo = new RaycastHit();
                if (Input.GetKeyDown(KeyCode.Mouse2))
                {
                    print("Mouse Wheel Click!");

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
                    {
                        Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                        if (hitInfo.transform.gameObject.tag == "NPC")      //TODO REPLACE WITH TABLE
                        {
                            if(hitInfo.transform.gameObject.Equals(m_Target))
                            {
                                m_Target = null;
                            }
                            else
                            {
                                m_Target = hitInfo.transform.gameObject;
                            }
                        }
                        else
                        {
                            m_Target = null;
                        }
                    }
                    else
                    {
                        m_Target = null;
                        Debug.Log("No hit");
                    }
                    Debug.Log("Mouse is down");
                }
                else if (Input.GetKeyDown(KeyCode.Period))
                {
                    // Cast a sphere wrapping character controller 10 meters forward
                    // to see if it is about to hit anything.
                    RaycastHit[] hList = Physics.SphereCastAll(transform.position, 10, transform.forward, 10);
                    RaycastHit bestTarget = new RaycastHit();
                    float closestDistanceSqr = Mathf.Infinity;
                    Vector3 currentPosition = transform.position;
                    bool npcFound = false;
                    foreach (RaycastHit h in hList)
                    {
                        Vector3 directionToTarget = h.transform.position - currentPosition;
                        float dSqrToTarget = directionToTarget.sqrMagnitude;
                        if (dSqrToTarget < closestDistanceSqr && h.transform.gameObject.tag.ToString().Equals("NPC"))
                        {
                            npcFound = true;
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = h;
                        }

                        hitInfo = bestTarget;
                    }

                    if (npcFound && hitInfo.transform.gameObject.tag.ToString().Equals("NPC"))      //TODO REPLACE WITH TABLE
                    {
                        if (hitInfo.transform.gameObject.Equals(m_Target))
                        {
                            m_Target = null;
                        }
                        else
                        {
                            m_Target = hitInfo.transform.gameObject;
                        }
                    }
                    else
                    {
                        print("No targets found");
                        m_Target = null;
                    }
                }
            }
        }

        {
            // read inputs
            float h = Input.GetAxis("Horizontal") * 5f;
            float v = Input.GetAxis("Vertical") * 5f;
            bool crouch = Input.GetKey(KeyCode.C);

            if(isMonster)
            {
                h = Input.GetAxis("Horizontal_Mnst") * 5f;
                v = Input.GetAxis("Vertical_Mnst") * 5f;
            }

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward * -1 + h * Vector3.right * -1;
            }
            //print(m_Move);
            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump, m_Sprint, m_Target);
            if (h != 0 || v != 0)
            {
                if(m_Target != null)
                {
                    //TODO play cautious animation
                    m_Animation.Play("Walk");
                }
                else
                {
                    m_Animation.Play("Walk");
                }
            }
            else
            {
                if (m_Target != null)
                {
                    //TODO play cautious animation
                    m_Animation.Play("Wait");
                }
                else
                {
                    m_Animation.Play("Wait");
                }
            }
            m_Jump = false;
        }
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield break;
    }
}
