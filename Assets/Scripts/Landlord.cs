using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landlord : MonoBehaviour {

    public Drone drone;
	// Use this for initialization
	//void Start () {

	//}

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag!="DroneOkay") {
            GetHit(2);
        }
	}

	// Update is called once per frame
	public void GetHit(int dmg) {
        drone.GetHit(dmg);
    }

    public IEnumerator Die()
    {
        GameObject tophat = transform.Find("Armature/Head/Head_end/tophat").gameObject;
        tophat.GetComponent<Rigidbody>().isKinematic = false;
        tophat.transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent(null);
        GetComponent<Rigidbody>().velocity = transform.up * 10;
        //GetComponent<ParticleSystem>().Play();
        Vector3 size = transform.localScale;

        for (int i = 0; i < 150; i++)
        {
            if (i==20) {
                gameObject.layer = 10;
            }
            if (i < 100)
            {
                size[1] /= 1.05f;
            }
            else
            {
                size /= 1.1f;
            }
            transform.localScale = size;
            yield return null;
        }
        Destroy(gameObject);
    }

}
