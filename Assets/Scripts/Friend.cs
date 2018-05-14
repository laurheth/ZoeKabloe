using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour {
    public int FriendIndex;
    public bool isfree;
    float time;
    GameObject Player;
    Animator animator;
    Rigidbody rb;
    AudioSource audioSource;
    bool distressconveyed;
    public AudioClip freesound;
    public AudioClip[] sadsnd;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        distressconveyed = false;
        audioSource = GetComponent<AudioSource>();
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
            if (!distressconveyed && Mathf.Abs(Player.transform.position.x-transform.position.x)<5) {
                audioSource.PlayOneShot(sadsnd[Mathf.RoundToInt(Random.Range(0, sadsnd.Length))]);
                distressconveyed = true;
            }
            return;
        }
        time += Time.deltaTime;
        animator.SetBool("Forward",true);
        //rb.AddTorque(Vector3.Cross(transform.forward, Vector3.left),ForceMode.VelocityChange);
        //rb.AddForce(transform.forward, ForceMode.Acceleration);
        //Vector3 forward = transform.forward;
        //forward = Vector3.Slerp(forward, Vector3.left, Time.deltaTime);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,
                                                 Quaternion.LookRotation(Vector3.back),
                                                 90*Time.deltaTime));
        rb.MovePosition(rb.position+2*transform.forward * Time.deltaTime);
        if (time>14) {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * 20 * Time.deltaTime, ForceMode.VelocityChange);
        }

        if (time>20) {
            Destroy(gameObject);
        }
	}

    public void Free() {
        GameManager.instance.AddFree(FriendIndex);
        isfree = true;
        transform.SetParent(null);
        rb.isKinematic = false;
        audioSource.PlayOneShot(freesound);
    }
}
