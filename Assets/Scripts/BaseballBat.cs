using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour {

    public GameObject zoe;
    Zoe zoescript;
    Animator animator;

	// Use this for initialization
	void Start () {
        animator = zoe.GetComponent<Animator>();
        zoescript = zoe.GetComponent<Zoe>();
	}

	/*public void OnCollisionEnter(Collision collision)
	{
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Swing")) {
            Rigidbody rb = collision.rigidbody;
            if (rb != null) {
                rb.AddForce((Vector3.up-zoe.transform.right) * zoescript.swingforce, ForceMode.Impulse);
            }
        }
	}*/
}
