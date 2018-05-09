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
    public float transitionspeed;
    int deaths;
    int friends;
    int bosshp;
    //List<GameObject> FriendList;
    List<int> FriendInds;

    public GameObject BossName;
    public GameObject BossHealthBar;

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
        Player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startpos = Player.transform.position;
        startpos[0] = startx;
        Player.transform.position = startpos;
        hptransform = HealthBar.GetComponent<RectTransform>();
        friendtext = FriendText.GetComponent<Text>();
        friends = 0;
        //FriendList = new List<GameObject>();
        FriendInds = new List<int>();
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

}
