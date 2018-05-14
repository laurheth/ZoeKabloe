using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    GameObject Player;
    public GameObject HealthBar;
    public GameObject FriendText;
    public GameObject Cover;
    Text friendtext;
    RectTransform hptransform;
    public int startx=-30;
    public int[] CheckPoints;
    public float transitionspeed;
    int deaths;
    int friends;
    float quithold;
    int bosshp;
    //List<GameObject> FriendList;
    List<int> FriendInds;

    public GameObject BossName;
    public GameObject BossHealthBar;

    AudioSource audioSource;
    public AudioClip level1mus;
    public AudioClip robbitbossmus;
    public AudioClip level2mus;
    public AudioClip dronebossmus;
    public AudioClip partymus;
    public AudioClip successsnd;
    int currentclip;
	private void Awake()
	{
       
        // Is singleton
        if (instance==null) {
            instance = this;
        }
        else if (instance!= this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        deaths = 0;
        currentclip = -1;

        Player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startpos = Player.transform.position;
        startpos[0] = startx;
        Player.transform.position = startpos;
        hptransform = HealthBar.GetComponent<RectTransform>();
        friendtext = FriendText.GetComponent<Text>();
        friends = 0;
        //FriendList = new List<GameObject>();
        FriendInds = new List<int>();
        audioSource = GetComponent<AudioSource>();
        SetMusic(0);
        //Debug.Log(audioSource);
        DoSetup();
	}

	public void DoSetup() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startpos = Player.transform.position;
        startpos[0] = startx;
        Player.transform.position = startpos;
        //BossName.SetActive(false);
        DeactivateBossBar();
        //Debug.Log(startpos);

        StartCoroutine(OpenScreen());
    }

    public IEnumerator OpenScreen(bool open=true) {
        float res = GetComponent<RectTransform>().sizeDelta[1];
        RectTransform cover = Cover.GetComponent<RectTransform>();
        Debug.Log(res);
        float openrate = (res / 800f) * transitionspeed;
        int sign;
        float target;

        if (!open) { 
            sign = -1;
            target = 0;
            cover.sizeDelta = new Vector2(cover.sizeDelta[0], -res);
        }
        else {
            sign = 1;
            target = -res;
            cover.sizeDelta = new Vector2(cover.sizeDelta[0], 0);
        }
        if (!open)
        {
            Cover.GetComponent<Image>().enabled = true;
        }
        while (sign*cover.sizeDelta[1]>target) {
            cover.sizeDelta = new Vector2(cover.sizeDelta[0],cover.sizeDelta[1]-sign*openrate*Time.deltaTime);
            //Debug.Log(cover.sizeDelta);
            yield return null;
        }
        if (open)
        {
            Cover.GetComponent<Image>().enabled = false;
        }
        //StartCoroutine(OpenScreen(!open));
    }

    public void UpdateHP(int hp, int maxhp)
    {
        float size = (100f * hp) / maxhp;
        if (size < 0) { size = 0f; }
        hptransform.sizeDelta = new Vector2(size, 10);
    }

    public void AddFree(int index) {
        FriendInds.Add(index);
        friends++;
        friendtext.text = " x " + friends;
    }

    public bool IsFreed(int index) {
        return FriendInds.Contains(index);
    }

    public void Restart() {
        //startx = (int)Player.transform.position.x;
        Debug.Log(startx);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DoSetup();
        deaths++;
    }

    public void ActivateBossBar(string bossname,int hp) {
        BossName.SetActive(true);
        BossName.GetComponent<Text>().text = bossname;
        bosshp = hp;
        UpdateBossBar(hp);
    }

    public void DeactivateBossBar() {
        BossName.SetActive(false);
    }

    public void UpdateBossBar(int hp) {
        if (hp<=0) {
            DeactivateBossBar();
            return;
        }
        float size = (185f * hp) / bosshp;
        if (size < 0) { size = 0f; }
        BossHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(size, 10);
    }

    public void CheckRescued() {
        GameObject[] friends = GameObject.FindGameObjectsWithTag("Friend");
        foreach (GameObject thisfriend in friends) {
            if (thisfriend.GetComponent<FreeFriend>()!=null) {
                if (!IsFreed(thisfriend.GetComponent<FreeFriend>().FriendID)) {
                    Destroy(thisfriend); // Remove non-free friends from the brunch place :(
                }
            }
        }
    }

    public IEnumerator RunEpilogue() {
        yield return new WaitForSeconds(20);
        yield return OpenScreen(false);
        SceneManager.LoadScene("Epilogue");
    }

    public void SetStartx(int newsx) {
        // No moving backwards
        if (newsx < startx) { return; }
        // Step forward through checkpoints until highest one below current position
        // Does not assume sorting
        for (int i = 0; i < CheckPoints.Length;i++) {
            if (CheckPoints[i] < newsx && CheckPoints[i]>startx) { startx = CheckPoints[i]; }
        }
    }

    public IEnumerator DeadBossNextMusic(AudioSource bosssnd, int nextmus) {
        SetMusic(-1);
        float timepassed = 0f;
        while (bosssnd.isPlaying && timepassed<10) {
            timepassed += Time.deltaTime;
            yield return null;
        }
        audioSource.PlayOneShot(successsnd);
        timepassed = 0f;
        while (audioSource.isPlaying && timepassed < 10)
        {
            timepassed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SetMusic(nextmus);
    }

    public void SetMusic(int track) {
        if (track == currentclip) { return; }
        switch (track) {
            default:
            case -1:
                audioSource.clip = null;
                audioSource.Stop();
                Debug.Log("Stopping music?");
                return;
            case 0:
                audioSource.clip = level1mus;
                break;
            case 1:
                audioSource.clip = robbitbossmus;
                break;
            case 2:
                audioSource.clip = level2mus;
                break;
            case 3:
                audioSource.clip = dronebossmus;
                break;
            case 4:
                audioSource.clip = partymus;
                break;
        }
        currentclip = track;
        Debug.Log("play mus?");
        audioSource.Play();
    }

	private void Update()
	{
        if (Input.GetButton("Cancel"))
        {
            quithold += Time.deltaTime;
            Debug.Log(quithold);
            if (quithold > 2)
            {
                //Debug.Log("Quit?");
                Application.Quit();
            }
        }
        else
        {
            quithold = 0;
        }
        //if ()
	}

}
