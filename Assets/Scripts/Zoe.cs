using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoe : MonoBehaviour {
    public int HitPoints;
    public int MaxHitPoints;
    public int MaxEstrogen;
    public int Estrogen;
    public float Speed;
    public float Acceleration;
    float speed2;
    public float RotationSpeed;
    public float Jump;
    public GameObject Bat;
    public GameObject Camera;
    public float maxspeed;
    Vector3 campos;
    Vector3 horizvel;
    Vector3 forwardforce;
    Rigidbody rb;
    Rigidbody batrb;
    Animator anim;
    Quaternion batrot;
    Collider coll;
    Renderer rend;
    int doublejump;
    bool dooring;
    bool endgame;
    bool dmgcooldown;
    public float doorspeed;
    float targx;
    //float currentspeed2;
    float currentspeed;
    float jumpspeed;
    public float swingforce;
    //bool isgrounded;
    float distToGround;
    bool jumping;
	// Use this for initialization
	void Start () {
        jumpspeed = 0;
        //speed2 = Speed * Speed;
        endgame = false;
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        dmgcooldown = false;
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

        transform.position = new Vector3(GameManager.instance.startx,
                                         transform.position.y, transform.position.z);

        distToGround = coll.bounds.extents.y;
        GameManager.instance.UpdateHP(HitPoints, MaxHitPoints);
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
        //jumping = false;
        //float rotation;
        if (HitPoints <= 0) { return; }
        if (transform.position.y < -20) { GetHit(1000); }
        if (rb.velocity.magnitude>maxspeed) {
            rb.velocity = maxspeed * rb.velocity.normalized;
        }

        float hx = Input.GetAxis("Vertical");
        float vx = Input.GetAxis("Horizontal");
        /*if (rb.velocity.magnitude>0.1) {
            forwardforce = new Vector3(rb.velocity[0],0,rb.velocity[2]);
            currentspeed = forwardforce.magnitude;
            forwardforce = forwardforce.normalized;
            rb.velocity = new Vector3(0, rb.velocity[1], 0);
            //
        }*/
        //rotation = Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;



        if (currentspeed < 0.01)
        {
            forwardforce = new Vector3(vx, 0,
                                       hx).normalized;
        }
        else {
            forwardforce = Vector3.RotateTowards(forwardforce, new Vector3(vx, 0, hx).normalized, (Time.deltaTime*RotationSpeed * 3.14f / 360f),0);
        }

        if (!endgame)
        {
            campos[0] = transform.position[0];
        }
        else {
            campos[0] = 201;
        }
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

        if (Input.GetKeyDown(KeyCode.X) && (IsGrounded() || doublejump > 0))
        {
            doublejump--;
            anim.SetTrigger("JumpButton");
            //Debug.Log("JumpButton");
            rb.AddForce((Vector3.up+forwardforce/2f) * Jump, ForceMode.VelocityChange);
            //jumping = true;
            //forwardforce *= 1.5f;
        }

        if (Mathf.Abs(hx) > 0.05 || Mathf.Abs(vx) > 0.05)
        {
            anim.SetFloat("Forward",1f);
            Quaternion targdir = Quaternion.LookRotation(forwardforce, Vector3.up)*Quaternion.Euler(0,90f,0);
            rb.MoveRotation(targdir);
            if (currentspeed<Speed) {
                //if (currentspeed +)
                currentspeed += Acceleration*Time.deltaTime;
                if (currentspeed > Speed) { currentspeed = Speed; }
            }
            else {
                currentspeed -= (Acceleration/5) * Time.deltaTime;
                if (currentspeed < Speed) { currentspeed = Speed; }
            }
        }
        else {
            //currentspeed = 0;
            if (currentspeed > 0)
            {
                currentspeed -= Acceleration*Time.deltaTime;
                if (currentspeed < 0) { currentspeed = 0; }
            }
            else { currentspeed = 0; }
            anim.SetFloat("Forward", 0f);
        }

        if (Input.GetKeyDown(KeyCode.Z) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Swing")) {
            anim.SetTrigger("SwingButton");
            //HitInFront();
            //Debug.Log("SwingButton");
            BatAttack();
        }
        rb.MovePosition(transform.position + currentspeed*forwardforce*Time.deltaTime);
        /*if (!jumping)
        {
            rb.MovePosition(transform.position + forwardforce);
        }
        else
        {*/
        /*if (jumping && Mathf.Abs(rb.velocity[1]) < 0.001 && IsGrounded())
        {

            jumping = false;
        }*/
        //}*/
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
            if (collision.gameObject.tag == "EndDoor") {
                endgame = true;
                transform.position = new Vector3(197, 0, -5);
                GameManager.instance.CheckRescued();
                StartCoroutine(GameManager.instance.RunEpilogue());
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
                    //Debug.Log(other.gameObject.name);
                    if (other.GetComponent<Rigidbody>() != null)
                    {
                        other.GetComponent<Rigidbody>().AddForce(swingforce * (-transform.right+Vector3.up/2),ForceMode.Impulse);
                        if (other.GetComponent<Slime>()!=null) {
                            other.GetComponent<Slime>().GetHit(2);
                        }
                        else if (other.GetComponent<Driver>()!=null) {
                            other.GetComponent<Driver>().GetHit(2);
                        }
                        else if (other.GetComponent<Landlord>()!=null) {
                            other.GetComponent<Landlord>().GetHit(2);
                        }
                    }
                }
            }
        }
    }

    public void GetHit(int dmg)
    {
        if (dmgcooldown) { return; }
        HitPoints -= dmg;
        //Debug.Log(dmg);
        GameManager.instance.UpdateHP(HitPoints, MaxHitPoints);
        StartCoroutine(DamageFlash());
        if (HitPoints <= 0)
        {
            rb.constraints = RigidbodyConstraints.None;
            anim.SetFloat("Forward", 0f);
            rb.AddTorque(new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3)), ForceMode.VelocityChange);
            StartCoroutine(Die());
        }
    }

    IEnumerator DamageFlash() {
        float timepassed = 0f;
        dmgcooldown = true;
        rend.material.EnableKeyword("_EMISSION");
        while (timepassed<1f) {
            timepassed += Time.deltaTime;
            rend.material.SetColor("_EmissionColor", (1 - timepassed) * Color.red);
            yield return null;
        }
        rend.material.SetColor("_EmissionColor", Color.black);
        rend.material.DisableKeyword("_EMISSION");
        dmgcooldown = false;
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(2);
        yield return GameManager.instance.OpenScreen(false);
        GameManager.instance.startx = (int)transform.position.x;
        GameManager.instance.Restart();
    }
}
