using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Floor
{
    public Transform floorPos;
    public GameObject[] blocks = new GameObject[25];
}
public class GameManager : MonoBehaviour
{
    public UnityEvent Reset;

    public Image pauseImg;
    public Sprite[] img = new Sprite[2];
    public Image soundImg;
    public Text timeTxt;

    private bool isPaused = false;
    private bool isX2 = false;
    private bool isMuted = false;
    public AudioListener mainListener;
    public AudioSource audioSource;
    public AudioSource BGM;
    public AudioClip shootSound;
    public AudioClip breakSound;
    public AudioClip clickSound;

    public static GameManager instance;
    public Floor[] floors = new Floor[9];
    public int stage = 1;
    public Vector3 currPoint;
    public bool isShoot = false;
    public bool isBased = true;
    private int idx = 0;

    public GameObject ballPrefab;
    public List<GameObject> objPool = new List<GameObject>();

    public Text score;
    public Text best;
    public int bestScore = 1;
    public GameObject panelPause;
    public GameObject panelResult;

    public Ball mainBall;

    void Awake()
    {
        BGM = Camera.main.GetComponent<AudioSource>();
        if (instance == null) instance = this;
        audioSource = GetComponent<AudioSource>();
        GameObject ballPool = new GameObject("Ball Pool");

        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(ballPrefab, ballPool.transform);
            obj.name = "Ball_" + i.ToString();
            obj.SetActive(false);
            objPool.Add(obj);
        }

        if (PlayerPrefs.HasKey("Score"))
        {
            bestScore = PlayerPrefs.GetInt("Score");
        }

    }

    void Start()
    {
        shootSound = Resources.Load<AudioClip>("ShootSound");
        breakSound = Resources.Load<AudioClip>("BreakSound");
        clickSound = Resources.Load<AudioClip>("ClickSound");
        for (int i = 0; i < floors[0].blocks.Length; i++)
        {
            floors[0].blocks[i].gameObject.SetActive(Random.value > 0.7f);
        }
        best.text = "BEST : " + bestScore;
    }

    public GameObject GetBall()
    {
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i].activeSelf == false)
            {
                return objPool[i];
            }
        }
        return null;
    }
    public void NextStage()
    {

        for(int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorPos.position.y < 0.1f)
            {
                for(int j = 0; j < floors[i].blocks.Length; j++)
                {
                    if (floors[i].blocks[j].activeSelf)
                    {
                        GameOver();
                        return;
                    }                    
                }
                floors[i].floorPos.position = new Vector3(0, 4.8f, 0);
                continue;
            }
            floors[i].floorPos.position = Vector3.Lerp(floors[i].floorPos.position, floors[i].floorPos.position + Vector3.up * -0.6f, 1f);
        }

        stage++;

        for (int i = 0; i < floors[8-idx].blocks.Length; i++)
        {
            floors[8 - idx].blocks[i].gameObject.SetActive(Random.value > 0.7f);
        }
        if (++idx == 9) idx = 0;
        
        score.text = "SCORE : " + stage;
        if (bestScore < stage) bestScore = stage;
        best.text = "BEST : " + bestScore;

        mainBall.line.enabled = true;
    }

    public void OnPauseClicked()
    {
        isPaused = !isPaused;
        panelPause.SetActive(isPaused ? true : false);
        Time.timeScale = isPaused ? 0.0f : (isX2 ? 2.0f : 1.0f);
        pauseImg.sprite = isPaused ? img[0] : img[1];
        audioSource.PlayOneShot(clickSound);
    }
    public void OnMuteClicked()
    {
        isMuted = !isMuted;
        soundImg.enabled = isMuted ? true : false;
        audioSource.mute = isMuted ? true : false;
        BGM.mute = isMuted ? true : false;
        audioSource.PlayOneShot(clickSound);
    }
    public void OnTimeClicked()
    {
        isX2 = !isX2;
        Time.timeScale = isX2 ? 2.0f : 1.0f;
        timeTxt.text = isX2 ? "X1" : "X2";
        audioSource.PlayOneShot(clickSound);
    }
    public void OnResetClicked()
    {
        Reset.Invoke();
        isShoot = false;
        for (int i = 0; i < 100; i++)
        {
            objPool[i].SetActive(false);
        }
        NextStage();
        audioSource.PlayOneShot(clickSound);
    }
    public void OnRestartClicked()
    {
        stage = 0;
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                floors[i].blocks[j].SetActive(false);
            }
        }
        if (panelResult.activeSelf) panelResult.SetActive(false);
        OnPauseClicked();
        OnResetClicked();
    }

    public void OnExitClicked()
    {
        OnPauseClicked();
        SceneManager.LoadScene("Title");
    }
    void GameOver()
    {
        PlayerPrefs.SetInt("Score", bestScore);
        isPaused = true;
        panelResult.transform.GetChild(1).GetComponent<Text>().text = "RESULT : " + stage;
        panelResult.SetActive(true);
    }
}