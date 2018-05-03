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
    Text friendtext;
    RectTransform hptransform;
    public int startx=-30;
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
        float size = (185f * hp) / bosshp;
        if (size < 0) { size = 0f; }
        BossHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(size, 10);
    }

}
