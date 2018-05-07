using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manhole : MonoBehaviour {

    public float Period;
    public GameObject Player;
    public GameObject SlimeObj;
    public float RotateSpeed;
    float TimeSinceOpen;
    public GameObject hinge;
    bool lidopen;
    Rigidbody hingerb;
	// Use this for initialization
	void Start () {
        lidopen = false;
        TimeSinceOpen = 0;
        hingerb = hinge.GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
    public void AddButton() {
        TimeSinceOpen = Period+0.5f;
    }

	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(transform.position.x - Player.transform.position.x) > 10) { return; }
        TimeSinceOpen += Time.deltaTime;
        if (TimeSinceOpen < Period) { return; }
        TimeSinceOpen -= Period;
        OpenLid();
	}

    void OpenLid() {
        if (!lidopen)
        {
            lidopen = true;
            Launch();
            StartCoroutine(RotateCover());
            StartCoroutine(SpawnSlime());
        }
    }

    IEnumerator SpawnSlime() {
        GameObject newslime = Instantiate(SlimeObj,transform.position-2*transform.up,transform.rotation);
        Slime slimescript = newslime.GetComponent<Slime>();
        slimescript.DoInit();
        slimescript.ForceMotion(true);
        float height = 0;
        while (height<2) {
            newslime.transform.position += transform.up * Time.deltaTime;
            height += Time.deltaTime;
            yield return null;
        }
        slimescript.ForceMotion(false);
    }

    IEnumerator RotateCover() {
        Quaternion startrot = transform.rotation;
        Quaternion targrot = transform.rotation;
        float currentrot = 0;
        for (int j = -1; j < 2; j += 2)
        {
            while (currentrot<90)
            //for (int i = 0; i < 45; i++)
            {
                currentrot += RotateSpeed * Time.deltaTime;
                targrot *= Quaternion.Euler(Vector3.forward * j * RotateSpeed*Time.deltaTime);
                hingerb.MoveRotation(targrot);
                yield return null;
            }
            currentrot = 0;
            if (j < 0)
            {
                yield return new WaitForSeconds(2);
            }
        }
        hingerb.MoveRotation(startrot);
        lidopen = false;
    }

    void Launch()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, 1f);
        if (colliders.Length > 0)
        {
            foreach (Collider other in colliders)
            {
                Debug.Log(other);
                if (other.GetComponent<Rigidbody>() != null)
                {
                    other.GetComponent<Rigidbody>().AddForce(12*transform.up+5*transform.right,ForceMode.VelocityChange);
                }

            }
        }
    }

	private void OnTriggerStay(Collider other)
	{
        if (other.tag=="Player") {
            TimeSinceOpen += Time.deltaTime * 2;   
        }
	}
}
