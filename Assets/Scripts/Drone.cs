using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

    GameObject[] Rotors;
    Rotor[] rotorscripts;
    Rigidbody[] rotrb;
    Rigidbody mainrb;
    //Quaternion[] rots;
    Vector3[] pos;
    Vector3 targetpos;
    Vector3 horizpos;
    GameObject Player;
    GameObject Driver;
    Landlord landlord;
    public GameObject Door;
    public float forceperrot;
    public float rotratechange;
    public float[] XLimits;
    //public float[] ZLimits;
    public int HitPoints;
    public int dmg;
    float baserot;
    float minrot;
    float maxrot;
    float[] rotpos;
    float[] rotrates;
    float[] targrotrates;
    float[] chgperrot;
    ParticleSystem[] particles;
    bool active;
    bool alive;
    AudioSource audioSource;
    public AudioClip awakensnd;
    public AudioClip diesnd;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        alive = true;
        active = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        Driver = transform.Find("LandLord").gameObject;
        landlord = Driver.GetComponent<Landlord>();
        landlord.drone = this;
        //rotationrate = 15;
        mainrb = GetComponent<Rigidbody>();
        baserot = mainrb.mass * Physics.gravity.magnitude/(4f*forceperrot);
        rotratechange = baserot/2f;
        minrot = baserot / 1.2f;
        maxrot = baserot * 1.2f;
        Rotors = new GameObject[4];
        rotorscripts = new Rotor[4];
        //rots = new Quaternion[4];
        pos = new Vector3[4];
        rotrates = new float[4];
        targrotrates = new float[4];
        chgperrot = new float[4];
        particles = new ParticleSystem[4];
        rotpos = new float[4];
        rotrb = new Rigidbody[4];
        for (int i = 1; i < 5;i++) {
            Rotors[i - 1] = transform.Find("Armature/Arm" + i + "/Rotor" + i).gameObject;
            rotorscripts[i - 1] = Rotors[i - 1].GetComponent<Rotor>();
            rotorscripts[i - 1].Damage = dmg;
            //Rotors[i - 1].GetComponent<Rotor>().Damage = dmg;
            rotrb[i - 1] = Rotors[i - 1].GetComponent<Rigidbody>();
            //rots[i - 1] = Rotors[i - 1].transform.rotation;
            pos[i - 1] = Rotors[i - 1].transform.position-transform.position;
            rotrates[i - 1] = baserot;//rotationrate;
            targrotrates[i - 1] = 0;//baserot;
            chgperrot[i - 1] = rotratechange;
            particles[i - 1] = Rotors[i - 1].GetComponent<ParticleSystem>();
        }
	}

    // Update is called once per frame
    void LateUpdate()
    {
        if (alive)
        {
            if (!active)
            {
                if (Mathf.Abs(Player.transform.position.x - transform.position.x) < 15)
                {
                    audioSource.PlayOneShot(awakensnd);
                    GameManager.instance.ActivateBossBar("THE SLIMELORD", HitPoints);
                    active = true;
                }
                return;
            }
            targetpos = Player.transform.position + Vector3.up * 5;
            if (targetpos[0] < XLimits[0]) { targetpos[0] = XLimits[0]; }
            if (targetpos[0] > XLimits[1]) { targetpos[0] = XLimits[1]; }
            horizpos = targetpos - transform.position;

            horizpos[1] = 0;
            Driver.transform.rotation = Quaternion.LookRotation(horizpos);

            //if (horizpos.magnitude < 5) { targetpos -= Vector3.up * 4; }

            if (targetpos[0] < XLimits[0]) { targetpos[0] = XLimits[0]; horizpos[0] = XLimits[0]; }
            if (targetpos[0] > XLimits[1]) { targetpos[0] = XLimits[1]; horizpos[0] = XLimits[1]; }

            for (int i = 0; i < 4; i++)
            {

                if (rotrates[i] > minrot/1.2f)
                {
                    rotorscripts[i].active = true;
                }
                else
                {
                    rotorscripts[i].active = false;
                }

                // Vertical position
                if (targetpos.y > transform.position.y && mainrb.velocity[1] < 0.5)
                {
                    targrotrates[i] = maxrot;
                }
                else if (targetpos.y < transform.position.y && mainrb.velocity[1] > -0.5)
                {
                    targrotrates[i] = minrot;
                }
                else
                {
                    if (mainrb.velocity[1] > 0.25)
                    {
                        targrotrates[i] = minrot;
                    }
                    else if (mainrb.velocity[1] < -0.25)
                    {
                        targrotrates[i] = maxrot;
                    }
                    else
                    {
                        targrotrates[i] = baserot;
                    }
                }

                // Stability?
                float anglefloat = Vector3.Dot(Vector3.up, (transform.rotation * pos[i]));
                float velfloat = Vector3.Cross(mainrb.angularVelocity, (transform.rotation * pos[i]))[1];
                //if (stabfloat > 0 && rotrb[i].velocity[1] < 0) { stabfloat = 1; }
                //if (stabfloat < 0 && rotrb[i].velocity[1] > 0) { stabfloat = 1; }

                targrotrates[i] -= rotratechange * anglefloat * 0.3f+rotratechange*velfloat*0.5f;// - mainrb.angularVelocity

                /*if (transform.up.y > 0)
                {
                    targrotrates[i] -= rotratechange*stabfloat*0.3f;
                }
                else
                {
                    targrotrates[i] += rotratechange * stabfloat * 0.3f;//10 * rotratechange * stabfloat;
                }*/

                //targrotrates[i] += rotratechange * (rotrb[i].velocity[1])*0.5f;

                //Debug.Log(targrotrates);
                /*if (rotrb[i].velocity[1] > mainrb.velocity[1] + 0.5)
                {
                    targrotrates[i] -= 2*rotratechange;
                }
                else if (rotrb[i].velocity[1] < mainrb.velocity[1] - 0.5)
                {
                    targrotrates[i] += 2*rotratechange;
                }*/

                // Horizontal position

                if (Vector3.Dot(horizpos.normalized, mainrb.velocity) < 4 || horizpos.magnitude > 10)
                {
                    targrotrates[i] -= 0.1f*rotratechange * Vector3.Dot(horizpos.normalized, (transform.rotation * pos[i]));
                }
                /*
            else {
                targrotrates[i] += rotratechange * Vector3.Dot(mainrb.velocity.normalized, (transform.rotation * pos[i]))/4f;
            }*/
            }
        }
        // Adjust to target rotation rate
        for (int i = 0; i < 4; i++)
        {
            if (rotrates[i] > targrotrates[i]) {
                rotrates[i] -= chgperrot[i]*Time.deltaTime;
                if (rotrates[i] < targrotrates[i]) {
                    rotrates[i] = targrotrates[i];
                }
            }
            else {
                rotrates[i] += chgperrot[i]*Time.deltaTime;
                if (rotrates[i] > targrotrates[i])
                {
                    rotrates[i] = targrotrates[i];
                }
            }

            // Enforce limits
            /*if (rotrates[i] > maxrot) { rotrates[i] = maxrot; }
            else if (rotrates[i] < minrot) { rotrates[i] = minrot; }*/

            //rotrates[i] = rotationrate;
            rotpos[i] += Mathf.Max(0, rotrates[i]) * Time.deltaTime;
            //rots[i] *= Quaternion.Euler(-rotrates[i]*Time.deltaTime,0,0);
            //Rotors[i].transform.rotation = rots[i];Quaternion.Euler(Vector3.left*rotpos[i])*
            rotrb[i].MoveRotation(transform.rotation * Quaternion.Euler(-Vector3.forward * 90) * Quaternion.Euler(Vector3.left * rotpos[i]));
            rotrb[i].MovePosition(transform.rotation * pos[i] + transform.position);
        }


    }

	private void FixedUpdate()
	{
        if (active)
        {
            for (int i = 0; i < 4; i++)
            {
                mainrb.AddForceAtPosition(transform.up * Mathf.Max(0,rotrates[i]) * forceperrot, transform.position + transform.rotation * pos[i]);
            }
        }
	}

    public void HitRotor(Rotor rotor, Collision collision) {
        for (int i = 0; i < 4;i++) {
            if (rotorscripts[i]==rotor) {
                rotrates[i] -= rotratechange*collision.rigidbody.mass;
                if (rotrates[i]<-rotratechange*4) {
                    rotrates[i] = -rotratechange * 4;
                }
                chgperrot[i] /= 1.1f;
                var emission=particles[i].emission;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant+0.5f);
                break;
            }
        }
    }

    public void GetHit(int damage) {
        HitPoints -= damage;
        /*minrot /= 1.1f;
        maxrot *= 1.1f;*/
        GameManager.instance.UpdateBossBar(HitPoints);
        if (HitPoints <= 0 && alive)
        {
            audioSource.PlayOneShot(diesnd);
            Door.GetComponent<ButtonDoor>().AddButton();
            alive = false;
            rotratechange /= 20f;
            for (int i = 0; i < 4;i++) {
                targrotrates[i] = -10;
                chgperrot[i] = rotratechange;
            }

            StartCoroutine(landlord.Die());
            foreach (Rotor dmgr in rotorscripts)
            {
                dmgr.active = false;
                dmgr.Damage = 0;
            }
            //Door.GetComponent<ButtonDoor>().AddButton(1);
        }
    }

}
