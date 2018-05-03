using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public GameObject PushMe;
    public GameObject Door;
    public bool IsSticky;
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

	private void OnTriggerStay(Collider other)
	{
        if (!ison) {
            doorscript.AddButton();
        }
        ison = true;
        ButtonLight.EnableKeyword("_EMISSION");
	}

	private void OnTriggerExit(Collider other)
	{
        if (IsSticky) { return; }
        if (ison)
        {
            doorscript.AddButton(-1);
        }
        ison = false;
        ButtonLight.DisableKeyword("_EMISSION");
	}
}
