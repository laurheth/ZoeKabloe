using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

    GameObject[] Rotors;
    Rigidbody[] rotrb;
    Rigidbody mainrb;
    //Quaternion[] rots;
    Vector3[] pos;
    Vector3 targetpos;
    Vector3 horizpos;
    GameObject Player;
    GameObject Driver;
    public float forceperrot;
    public float rotratechange;
    float baserot;
    float minrot;
    float maxrot;
    float[] rotpos;
    float[] rotrates;
    float[] targrotrates;
    bool active;
	// Use this for initialization
	void Start () {
        active = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        Driver = transform.Find("LandLord").gameObject;
        //rotationrate = 15;
        mainrb = GetComponent<Rigidbody>();
        baserot = mainrb.mass * Physics.gravity.magnitude/(4f*forceperrot);
        minrot = baserot / 1.2f;
        maxrot = baserot * 1.2f;
        Rotors = new GameObject[4];
        //rots = new Quaternion[4];
        pos = new Vector3[4];
        rotrates = new float[4];
        targrotrates = new float[4];
        rotpos = new float[4];
        rotrb = new Rigidbody[4];
        for (int i = 1; i < 5;i++) {
            Rotors[i - 1] = transform.Find("Armature/Arm" + i + "/Rotor" + i).gameObject;
            rotrb[i - 1] = Rotors[i - 1].GetComponent<Rigidbody>();
            //rots[i - 1] = Rotors[i - 1].transform.rotation;
            pos[i - 1] = Rotors[i - 1].transform.position-transform.position;
            rotrates[i - 1] = baserot;//rotationrate;
            targrotrates[i - 1] = baserot;
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!active) { 
            if (Mathf.Abs(Player.transform.position.x-transform.position.x)<15) {
                active = true;
            }
            return;
        }
        targetpos = Player.transform.position+Vector3.up*5;
        horizpos = targetpos-transform.position;

        horizpos[1] = 0;
        Driver.transform.rotation = Quaternion.LookRotation(horizpos);
        for (int i = 0; i < 4;i++) {

            // Vertical position
            if (targetpos.y > transform.position.y && mainrb.velocity[1]<1)
            {
                targrotrates[i] = maxrot;
            }
            else if ( targetpos.y < transform.position.y && mainrb.velocity[1] > -1 ) {
                targrotrates[i] = minrot;
            }
            else {
                if (mainrb.velocity[1] > 0.5)
                {
                    targrotrates[i] = minrot;
                }
                else if (mainrb.velocity[1] < -0.5) {
                    targrotrates[i] = maxrot;
                }
                else {
                    targrotrates[i] = baserot;
                }
            }

            // Stability?
            targrotrates[i] -= 10 * rotratechange * Vector3.Dot(Vector3.up, (transform.rotation * pos[i]));

            if (rotrb[i].velocity[1]>mainrb.velocity[1]+0.5) {
                targrotrates[i] -= rotratechange;
            }
            else if (rotrb[i].velocity[1] < mainrb.velocity[1] - 0.5){
                targrotrates[i] += rotratechange;
            }

            // Horizontal position

            if (Vector3.Dot(horizpos.normalized, mainrb.velocity) < 4 || horizpos.magnitude>10)
            {
                targrotrates[i] -= rotratechange * Vector3.Dot(horizpos.normalized, (transform.rotation * pos[i]));
            }

            // Adjust to target rotation rate
            if (rotrates[i] > targrotrates[i]) { rotrates[i] -= rotratechange; }
            else { rotrates[i] += rotratechange; }
            // Enforce limits
            if (rotrates[i] > maxrot) { rotrates[i] = maxrot; }
            else if (rotrates[i] < minrot) { rotrates[i] = minrot; }

            //rotrates[i] = rotationrate;
            rotpos[i] += rotrates[i] * Time.deltaTime;
            //rots[i] *= Quaternion.Euler(-rotrates[i]*Time.deltaTime,0,0);
            //Rotors[i].transform.rotation = rots[i];Quaternion.Euler(Vector3.left*rotpos[i])*
            rotrb[i].MoveRotation(transform.rotation* Quaternion.Euler(-Vector3.forward*90)*Quaternion.Euler(Vector3.left * rotpos[i]));
            rotrb[i].MovePosition(transform.rotation*pos[i] + transform.position);
        }

	}

	private void FixedUpdate()
	{
        if (active)
        {
            for (int i = 0; i < 4; i++)
            {
                mainrb.AddForceAtPosition(transform.up * rotrates[i] * forceperrot, transform.position + transform.rotation * pos[i]);
            }
        }
	}
}
