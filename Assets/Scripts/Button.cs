using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public GameObject PushMe;
    public GameObject Door;
    ButtonDoor doorscript;
    Material ButtonLight;
    bool ison;
    //bool permaon;
	// Use this for initialization
	void Start () {
        ButtonLight = PushMe.GetComponent<Renderer>().material;
        doorscript = Door.GetComponent<ButtonDoor>();
	}

    public bool IsOn() {
        return ison;
    }

	private void OnTriggerEnter(Collider other)
	{
        ison = true;
        doorscript.AddButton();
        ButtonLight.EnableKeyword("_EMISSION");
	}

	private void OnTriggerExit(Collider other)
	{
        ison = false;
        doorscript.AddButton(-1);
        ButtonLight.DisableKeyword("_EMISSION");
	}
}
