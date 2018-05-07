using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour {

    public int Damage;
    public bool active;

	private void Start()
	{
        active = true;
	}

	protected void OnCollisionStay(Collision collision)
	{
        Debug.Log("Hit");
        Debug.Log(active);
        if (active && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Zoe>().GetHit(Damage);
        }
	}

}
