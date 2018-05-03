using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {
    List<Damager> damagers;
    Driver driver;
    public int HitPoints;
    public float Speed;
    public float Rotation;
    public int Damage;
    float pause;
    bool active;
    Animator animator;
    GameObject Player;
    Zoe playerscript;
    Vector3 targrot;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        damagers = new List<Damager>();
        rb = GetComponent<Rigidbody>();
        foreach (Damager dmgr in GetComponentsInChildren<Damager>())
        {
            dmgr.Damage = Damage;
            dmgr.active = true;
            damagers.Add(dmgr);
        }
        pause = 0;
        driver = GetComponentInChildren<Driver>();
        driver.robot = this;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerscript = Player.GetComponent<Zoe>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (HitPoints <= 0) { return; }
        if (!active) {
            if (Mathf.Abs(Player.transform.position.x-transform.position.x)<15) {
                active = true;
                GameManager.instance.ActivateBossBar("MECHA NIMBY",HitPoints);
            }
            else {
                return;
            }
        }
        if (pause>0) {
            pause -= Time.deltaTime;
            animator.SetBool("Forward", false);
            return;
        }
        targrot = Player.transform.position - transform.position;
        targrot[1] = 0;
        if (Vector3.Dot(targrot.normalized,transform.forward)>0.8 && targrot.sqrMagnitude<16) {
            pause += 5;
            if (Random.Range(0, 10) > 5)
            {
                animator.SetTrigger("Attack1");
            }
            else
            {
                animator.SetTrigger("Attack2");
            }
        }
        else {
            animator.SetBool("Forward", true);
            rb.MovePosition(transform.forward*Speed*Time.deltaTime + rb.position);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,
                                                     Quaternion.LookRotation(targrot), Rotation));
        }
	}

    public void GetHit(int damage) {
        HitPoints -= damage;
        GameManager.instance.UpdateBossBar(HitPoints);
        if (HitPoints<=0 && active) {
            active = false;
            animator.SetBool("Forward", false);
            animator.SetBool("Dead",true);
            StartCoroutine(driver.Die());
            foreach (Damager dmgr in damagers) {
                dmgr.active = false;
            }
        }
    }
}
