using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoe : MonoBehaviour {
    public int HitPoints;
    public int MaxHitPoints;
    public int MaxEstrogen;
    public int Estrogen;
    public float Speed;
    public float RotationSpeed;
    public float Jump;
    public GameObject Bat;
    public GameObject Camera;
    Vector3 campos;
    Rigidbody rb;
    Rigidbody batrb;
    Animator anim;
    Quaternion batrot;
    Collider coll;
    int doublejump;
    bool dooring;
    public float doorspeed;
    float targx;
    public float swingforce;
    //bool isgrounded;
    float distToGround;
	// Use this for initialization
	void Start () {
        //Estrogen = 2;
        doublejump = 2;
        coll = GetComponent<Collider>();
        dooring = false;
        campos = Camera.transform.position;
        //Bat=GameObject.G
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        batrot = Bat.transform.localRotation;
        batrb = Bat.GetComponent<Rigidbody>();

        distToGround = coll.bounds.extents.y;
	}

    bool IsGrounded() {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, distToGround + 0.1f)) {
            doublejump = 2;
            return true;
        }
        return false;
    }

	// Update is called once per frame
	void Update () {
        //float rotation;
        Vector3 forwardforce;
        float hx = Input.GetAxis("Vertical");
        float vx = Input.GetAxis("Horizontal");
        //rotation = Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
        forwardforce = new Vector3(vx * Speed * Time.deltaTime,0,
                                   hx * Speed * Time.deltaTime);

        campos[0] = transform.position[0];
        Camera.transform.position = campos;

        if (dooring) {
            float distchange = doorspeed * Time.deltaTime;
            if (transform.position.x<targx-distchange) {
                transform.position += Vector3.right * distchange;
            }
            else if (transform.position.x > targx + distchange) {
                transform.position += Vector3.left * distchange;
            }
            else {
                transform.position += Vector3.left * (transform.position.x - targx);
                dooring = false;
                coll.enabled = true;
                rb.isKinematic = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && (IsGrounded() || doublejump>0 ))
        {
            doublejump--;
            anim.SetTrigger("JumpButton");
            Debug.Log("JumpButton");
            rb.AddForce((Vector3.up+forwardforce.normalized/2f) * Jump, ForceMode.VelocityChange);
            //forwardforce *= 1.5f;
        }

        if (Mathf.Abs(hx) > 0.05 || Mathf.Abs(vx) > 0.05)
        {
            anim.SetFloat("Forward",1f);
            Quaternion targdir = Quaternion.LookRotation(forwardforce, Vector3.up)*Quaternion.Euler(0,90f,0);
            rb.MoveRotation(targdir);
        }
        else {

            anim.SetFloat("Forward", 0f);
        }

        if (Input.GetKeyDown(KeyCode.Z) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Swing")) {
            anim.SetTrigger("SwingButton");
            //HitInFront();
            Debug.Log("SwingButton");
            BatAttack();
        }

        rb.MovePosition(transform.position + forwardforce);
	}

	public void OnCollisionEnter(Collision collision)
	{
        if (dooring == false)
        {
            if (collision.gameObject.tag == "Door")
            {
                if (collision.transform.position.x < transform.position.x)
                {
                    targx = transform.position.x - 3;
                }
                else
                {
                    targx = transform.position.x + 3;
                }
                dooring = true;
                coll.enabled = false;
                rb.isKinematic = true;
            }
        }
        /*if (Vector3.Dot(collision.contacts[0].normal,Vector3.up)<0) {
            isgrounded = true;
        }*/
	}

    private void BatAttack() {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position + Vector3.up - transform.right/2, 1f);
        if (colliders.Length>0) {
            foreach (Collider other in colliders) {
                if (other.gameObject.tag != "Player")
                {
                    Debug.Log(other.gameObject.name);
                    if (other.GetComponent<Rigidbody>() != null)
                    {
                        other.GetComponent<Rigidbody>().AddForce(swingforce * (-transform.right+Vector3.up/2),ForceMode.Impulse);
                        if (other.GetComponent<Slime>()!=null) {
                            other.GetComponent<Slime>().GetHit(2);
                        }
                    }
                }
            }
        }
    }

	public void GetHit(int dmg) {
        HitPoints -= dmg;
    }
}
