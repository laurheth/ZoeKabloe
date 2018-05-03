using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {
    public int FriendIndex;
    public bool isfree;
    float time;
    Animator animator;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        time = 0f;
        isfree = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (GameManager.instance.IsFreed(FriendIndex)) {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isfree) {
            return;
        }
        time += Time.deltaTime;
        animator.SetBool("Forward",true);
        //rb.AddTorque(Vector3.Cross(transform.forward, Vector3.left),ForceMode.VelocityChange);
        //rb.AddForce(transform.forward, ForceMode.Acceleration);
        //Vector3 forward = transform.forward;
        //forward = Vector3.Slerp(forward, Vector3.left, Time.deltaTime);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,
                                                 Quaternion.LookRotation(Vector3.left),
                                                 90*Time.deltaTime));
        rb.MovePosition(rb.position+2*transform.forward * Time.deltaTime);
        if (time>4) {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * 20 * Time.deltaTime, ForceMode.VelocityChange);
        }

        if (time>10) {
            Destroy(gameObject);
        }
	}

    public void Free() {
        GameManager.instance.AddFree(FriendIndex);
        isfree = true;
        transform.SetParent(null);
        rb.isKinematic = false;
    }
}
