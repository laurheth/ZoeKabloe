using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
    public GameObject Fade2Black;
    public GameObject PlotText;
    public GameObject LoadingText;
    bool plotting;
    Image fade2black;
    Color fadecolor;
    Text plottext;
    float quithold;
    string plot;
    public float letterspersecond;
    float seconds;
    bool loading;
    int numletters;
    // Use this for initialization
    void Start()
    {
        loading = false;
        plotting = false;
        fade2black = Fade2Black.GetComponent<Image>();
        fadecolor = fade2black.color;
        plottext = PlotText.GetComponent<Text>();
        plot = plottext.text;
        plottext.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape)) {
            quithold += Time.deltaTime;
            if (quithold>2) {
                Application.Quit();
            }
        }
        else {
            quithold = 0;
        }
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
                if (Input.anyKey && !loading)
                {
                    numletters = plot.Length;
                    seconds = plot.Length / letterspersecond+1;
                    StartCoroutine(Loading(LoadingText));
                }
            }
        }
	}

    IEnumerator Loading(GameObject ldtxt) {
        ldtxt.SetActive(true);
        Text loadtext = ldtxt.GetComponent<Text>();
        string thetext = loadtext.text;
        int minpos = loadtext.text.Length - 3;
        int showtil=minpos;
        float timeelapsed=0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        while (!asyncLoad.isDone) {
            timeelapsed += Time.deltaTime;
            if (timeelapsed>0.5) {
                showtil++;
                if (showtil > loadtext.text.Length) { showtil = minpos; }
                timeelapsed -= 0.5f;
                loadtext.text = thetext.Substring(0, showtil);
            }
            yield return null;
        }
    }
}
