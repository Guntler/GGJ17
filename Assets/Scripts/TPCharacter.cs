using UnityEngine;
using System.Collections;

public class TPCharacter : MonoBehaviour {

    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_JumpPower = 12f;
    [Range(1f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;
    [SerializeField]
    float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    bool m_Crouching;
    float m_RotRequired;
    float m_OrigGroundCheckDistance;
    Vector3 m_NormForward;
    GameObject m_Target;

    bool m_IsJumping;
    bool m_IsFalling;

    CapsuleCollider m_capsuleCol;
    BoxCollider m_boxCol;


    // Use this for initialization
    void Start () {
        //m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_boxCol = GetComponent<BoxCollider>();
        m_capsuleCol = GetComponent<CapsuleCollider>();
        //m_Capsule = GetComponent<CapsuleCollider>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_RotRequired = 0.0f;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        m_IsJumping = false;
        m_IsFalling = false;


    }
	
	// Update is called once per frame
	void Update () {
        m_NormForward = RotateX(transform.GetChild(0).GetChild(0).forward, -40);
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool sprint, GameObject target)
    {
        m_Target = target;
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        //Vector3 newDir = (new Vector3(move.x, 0, 0) + new Vector3(0, 0, move.z))/ 2;
        //newDir.Normalize();
        m_ForwardAmount = move.z;
        CheckGroundStatus();

        //ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            HandleGroundedMovement(crouch, jump);
        }
        else
        {
            HandleAirborneMovement();
        }
        move.y = 0;
        if (m_IsGrounded && !m_IsFalling)
        {
           if (sprint)
            {
                m_Rigidbody.velocity += move * 5f;
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, 15.0f);
            }
           else
            {
                m_Rigidbody.velocity += move * 1.5f;
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, 5.0f);
            }
              

            

            Vector3 tiltedDir = RotateX(move, -40);
            RotateCharacter(tiltedDir);
        }

            //Use with CharacterContainer

            //Vector3 tiltedDir = move;



    }

    //Stop player upon getting hit
    public void Pause()
    {
        m_Rigidbody.velocity = m_Rigidbody.velocity;
        //angularVelocity = m_Rigidbody.angularVelocity;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;
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

    Vector3 RotateY( Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) + (sin * tz);
        v.z = (cos * tz) - (sin * tx);

        return v;
    }

    Vector3 RotateZ( Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (cos * ty) + (sin * tx);

        return v;
    }

    void RotateCharacter(Vector3 newDir)
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        newDir.z *= -1;
        if(m_Target)
        {
            Vector3 targetDir = (m_Target.transform.position - transform.position).normalized;
            Quaternion rotRequired = Quaternion.FromToRotation(transform.GetChild(0).GetChild(0).transform.forward, targetDir);
            transform.GetChild(0).GetChild(0).Rotate(0, rotRequired.y * turnSpeed * Time.deltaTime * 4, 0);
        }
        else
        {
            Quaternion rotRequired = Quaternion.FromToRotation(transform.GetChild(0).GetChild(0).transform.forward, newDir);
            //Vector3 rotRequired = Vector3.RotateTowards(transform.GetChild(0).position, transform.GetChild(0).position + newDir, 90 * Mathf.Deg2Rad,150);
            //print(rotRequired + "REQUIRED ROT || " + transform.GetChild(0).GetChild(0).transform.forward + " Forward || " + newDir + " New Direction");

            transform.GetChild(0).GetChild(0).Rotate(0, rotRequired.y * turnSpeed * Time.deltaTime * 4, 0);
            //m_RotRequired += rotRequired.y * turnSpeed * Time.deltaTime * 4;
            //transform.GetChild(0).GetChild(0).Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }

    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce*10);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        //Vector3 angle = Quaternion.ToEulerAngles(transform.GetChild(0).GetChild(0).rotation)*Mathf.Rad2Deg;
        //print(transform.GetChild(0).GetChild(0).rotation.eulerAngles.y + " Rotation || " + transform.forward + " Old forward || " + normForward + "New Forward");
        //Vector3 normForward = transform.forward;

        Vector3 normForward = m_NormForward;
        normForward.y = 0.0f;
        normForward.z *= -1.0f;

#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        //Debug.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + normForward + (Vector3.down * m_GroundCheckDistance)*0.5f,Color.cyan,2.0f,false);
        Debug.DrawLine(transform.position - (new Vector3(0, 0, 0.18f)) + (Vector3.up * 0.1f), transform.position - (new Vector3(0, 0, 0.18f)) + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance),Color.white,1.0f);
        //print(RotateX(transform.GetChild(0).GetChild(0).transform.forward,40) + "  " + transform.GetChild(0).GetChild(0).transform.forward);
        Debug.DrawLine(transform.position - normForward * 0.5f + (Vector3.up * 0.1f), transform.position - normForward * 0.5f + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.green);
        //Debug.DrawLine(transform.position - RotateY(normForward,90.0f) * 0.5f + (Vector3.up * 0.1f), transform.position - RotateY(normForward, 90.0f) * 0.5f + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.green);
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + normForward + (Vector3.up * 0.1f), Color.red);
#endif

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), normForward, out hitInfo, normForward.magnitude*0.5f))
        {
            print("Found a wall");
        }

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position - (new Vector3(0,0,0.18f)) + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_IsFalling = false;
            m_IsJumping = false;
            //m_Animator.applyRootMotion = true;
        }
        else
        {
            Vector3 horizontalVelocity = m_Rigidbody.velocity;
            horizontalVelocity.y = 0.0f;
            //print(horizontalVelocity.magnitude);
            //Check for ground behind character's feet.
            if (!Physics.Raycast(transform.position - normForward * 0.5f + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                //TODO: Do jump
                
                if (horizontalVelocity.magnitude > 3.0f && !m_IsJumping)
                {
                    m_IsJumping = true;
                    m_Rigidbody.AddForce(normForward.x * 3.5f, transform.up.y * m_JumpPower * 10 * Time.deltaTime, normForward.z * 3.5f, ForceMode.VelocityChange);
                }
                else if (m_IsJumping)
                {

                }
                else
                {
                    m_IsFalling = true;
                }
                //m_Animator.applyRootMotion = false;
            }
            else
            {
                if(horizontalVelocity.magnitude<=3.0) {
                    //TODO off-balance animation
                    StartCoroutine(AlignWithEdge());
                    print("OFF BALANCE!");
                }

            }
        }
    }

    IEnumerator AlignWithEdge()
    {
        RaycastHit hitInfo;
        Vector3 normForward = m_NormForward;
        normForward.y = 0.0f;
        normForward.z *= -1.0f;
        while(!Physics.Raycast(transform.position - (new Vector3(0, 0, 0.18f)) + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            transform.position -= normForward*0.1f;
        }
        
        yield return null;
    }   
}
