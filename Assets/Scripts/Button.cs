using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public GameObject PushMe;
    public GameObject Door;
    public Color OnColor;
    public Color OffColor;
    //public GameObject manhole;
    public bool IsSticky;
    ButtonDoor doorscript;
    Manhole manholescript;
    Material ButtonLight;
    bool ison;

    AudioSource audioSource;
    public AudioClip buttonpush;

    //bool permaon;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        ButtonLight = PushMe.GetComponent<Renderer>().material;
        doorscript = Door.GetComponent<ButtonDoor>();
        if (doorscript==null) {
            manholescript = Door.GetComponent<Manhole>();
        }
        ButtonLight.SetColor("_EmissionColor", OffColor);
	}

    public bool IsOn() {
        return ison;
    }

	private void OnTriggerStay(Collider other)
	{
        //if (other.tag == "Door") { return; }
        //Debug.Log(other.name);
        if (!ison) {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(buttonpush);
            }
            if (doorscript != null)
            {
                doorscript.AddButton();
            }
            else if (manholescript != null)
            {
                manholescript.AddButton();
            }
        }
        ison = true;
        ButtonLight.SetColor("_EmissionColor", OnColor);
	}

	private void OnTriggerExit(Collider other)
	{
        if (IsSticky) { return; }
        if (ison)
        {
            if (doorscript != null)
            {
                doorscript.AddButton(-1);
            }
        }
        ison = false;
        ButtonLight.SetColor("_EmissionColor", OffColor);//DisableKeyword("_EMISSION");
	}
}
