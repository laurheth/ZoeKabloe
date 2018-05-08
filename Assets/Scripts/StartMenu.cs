using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
    public GameObject Fade2Black;
    public GameObject PlotText;
    bool plotting;
    Image fade2black;
    Color fadecolor;
    Text plottext;
    string plot;
    public float letterspersecond;
    float seconds;
    int numletters;
    // Use this for initialization
    void Start()
    {
        plotting = false;
        fade2black = Fade2Black.GetComponent<Image>();
        fadecolor = fade2black.color;
        plottext = PlotText.GetComponent<Text>();
        plot = plottext.text;
        plottext.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey && !plotting) {
            plotting = true;
            //SceneManager.LoadScene("Game");
        }
        else if (plotting) {
            if (fadecolor[3] < 1)
            {
                fadecolor[3] += 0.3f * Time.deltaTime;
                fade2black.color = fadecolor;
            }
            else {
                seconds += Time.deltaTime;
                numletters = Mathf.CeilToInt(letterspersecond * seconds);
                if (numletters >= plot.Length) { numletters = plot.Length; }
                plottext.text = plot.Substring(0, numletters);
                if (Input.anyKey) {
                    SceneManager.LoadScene("Game");
                }
            }
        }
	}
}
