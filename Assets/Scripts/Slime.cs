using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {

    GameObject player;
    GameObject cophat;
    Zoe playerscript;
    public int HitPoints;
    public float Speed;
    public float JumpHeight;
    public int Damage;
    public float torque;
    float timesincelastjump;
    Vector3 home;
    Vector3 targpos;
    Vector3 targlook;
    float invisibletime;
    Renderer rend;
    Rigidbody rb;
    Animator animator;
    Collider coll;
    float waitforabit;
    bool dead;
    bool initialized;
    //bool visible;
    //bool isactive;
	// Use this for initialization
	void Start () {
        if (!initialized) {
            DoInit();
        }
	}

    public void DoInit()
    {
        if (transform.Find("Armature/Head/cophat") != null)
        {
            cophat = transform.Find("Armature/Head/cophat").gameObject;
        }
        else {
            cophat = null;
        }
        invisibletime = 100f;
        //visible = false;
        dead = false;
        timesincelastjump = 0f;
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        waitforabit = 0f;
        rend = GetComponentInChildren<Renderer>();
        home = transform.position + Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
        playerscript = player.GetComponent<Zoe>();
    }
	
    public void ForceMotion(bool option) {
        coll.enabled = !option;
        rb.isKinematic = option;
        dead = option;
    }

	// Update is called once per frame
	void Update () {
        if (dead) {
            return;
        }
        if (HitPoints<=0) {
            dead = true;
            StartCoroutine(Die());
        }
        targpos = player.transform.position;
        if (Mathf.Abs(player.transform.position.x-transform.position.x)>10) {
            invisibletime += Time.deltaTime;
            targpos = home;
        }
        else {
            invisibletime = 0f;
        }
        if (invisibletime>10) {
            return;
        }
        waitforabit -= Time.deltaTime;
        timesincelastjump += Time.deltaTime;
        targlook = new Vector3(transform.position.x-targpos.x, 0, transform.position.z-targpos.z);
        //rb.MoveRotation(Quaternion.LookRotation(targlook));
        if (Mathf.Abs(transform.rotation.eulerAngles[0]) < 10 && Mathf.Abs(transform.rotation.eulerAngles[2]) < 10)
        {
            rb.AddTorque(Vector3.Cross(transform.forward, -targlook).normalized * torque * Time.deltaTime, ForceMode.Impulse);
        }
        else {
            rb.AddTorque(Vector3.Cross(transform.up, Vector3.up).normalized * torque * Time.deltaTime, ForceMode.Impulse);
        }
        if (waitforabit > 0)
        {
            animator.SetBool("Forward", false);
            return;
        }
        if (waitforabit < -10f && Random.Range(0, 10f) > 5) { waitforabit = Random.Range(0, 5f); }

        if (timesincelastjump>5) {
            timesincelastjump = -Random.Range(0,10);
            Jump();
        }
        //Debug.Log(transform.forward * Speed * Time.deltaTime);

        if (Vector3.Dot(targlook.normalized, transform.forward) < -0.9)
        {
            animator.SetBool("Forward",true);
            rb.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
        }
        else {
            animator.SetBool("Forward", false);
        }
        
	}

    void Jump() {
        animator.SetTrigger("Jump");
        rb.AddForce((Vector3.up + transform.forward) * JumpHeight, ForceMode.VelocityChange);
    }

	protected void OnCollisionEnter(Collision collision)
	{
        // Hit the player!
        if (collision.gameObject.tag=="Player") {
            playerscript.GetHit(Damage);
            Jump();
        }
	}

    public void GetHit(int dmg)
    {
        HitPoints -= dmg;
    }

    IEnumerator Die() {
        //GetComponent<ParticleSystem>().Play();
        Vector3 size = transform.localScale;
        if (cophat != null)
        {
            cophat.transform.SetParent(null);
            cophat.GetComponent<Rigidbody>().isKinematic = false;
        }
        for (int i = 0; i < 150;i++) {
            if (i < 100)
            {
                size[1] /= 1.05f;
            }
            else {
                size /= 1.1f;
            }
            transform.localScale = size;
            yield return null;
        }
        Destroy(gameObject);
    }

}
