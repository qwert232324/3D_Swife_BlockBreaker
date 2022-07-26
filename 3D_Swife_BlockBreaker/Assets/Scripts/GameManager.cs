using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Floor 
{
    // 본인의 위치 정보와 블럭 25개를 한 층으로 가지고 있는 Floor 클래스 생성
    public Transform floorPos;
    public GameObject[] blocks = new GameObject[25];
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // singleton
    public Floor[] floors = new Floor[9]; // 9층을 만든다
    public int stage = 1; // 현재 stage == score
    private int idx = 0; // 층의 변환을 도와주는 index 변수

    public Vector3 currPoint; // Main Ball에서 받아올 발사 위치와
    public Vector3 currPos;   // 그 값을 전달받을 변수들
    public GameObject ballPrefab; // Ball_Light의 프리팹
    public Ball mainBall; // Main Ball

    public List<GameObject> objPool = new List<GameObject>(); // ball들을 만들어줄 Object Pool

    public Text score; // SCORE 출력 Text
    public Text best; // BEST 출력 Text
    public int bestScore = 1; // PlayerPref으로부터 BestScore를 받아올 변수

    public GameObject panelPause; // Pause 버튼을 눌렀을 때 나올 panel
    public GameObject panelResult; // GameOver가 됐을 때 나올 panel
    public Image pauseImg; // Pause 버튼 이미지
    public Sprite[] img = new Sprite[2]; // Pause<->Play 를 반복할 이미지 2개
    public Image soundImg; // Sound 버튼 이미지
    public Text timeTxt; // X1, X2 배수를 나타낼 Text

    private bool isPaused = false; // 멈춰 있는 상태인지를 나타냄
    private bool isX2 = false; // 게임에 배속이 걸려 있는 상태인지를 나타냄
    private bool isMuted = false; // 음소거 되어있는 상태인지를 나타냄

    public UnityEvent Reset; // RE 버튼에 사용할 Event


    void Awake()
    {        
        if (instance == null) instance = this; // singleton
        GameObject ballPool = new GameObject("Ball Pool"); // ball들을 담을 GameObject 생성

        for (int i = 0; i < 100; i++)
        {
            // ball 100개를 생성 후, 비활성화 시킨 후에 오브젝트 풀에 넣는다.
            GameObject obj = Instantiate(ballPrefab, ballPool.transform);
            obj.name = "Ball_" + i.ToString();
            obj.SetActive(false);
            objPool.Add(obj);
        }

        if (PlayerPrefs.HasKey("Score"))
        {
            // 로컬 저장소에 Score라는 Key를 가지고 있을 경우, bestScore에 해당 숫자를 가져온다.
            bestScore = PlayerPrefs.GetInt("Score");
        }
    }

    void Start()
    {
        for (int i = 0; i < floors[0].blocks.Length; i++)
        {
            // 맨 위 Floor의 블럭들을 30% 확률로 활성화 시킨다.
            floors[0].blocks[i].gameObject.SetActive(Random.value > 0.7f);
        }
        best.text = "BEST : " + bestScore; // 최고점수 나타내기
    }

    public GameObject GetBall()
    {
        // Object Pool 에서 비활성화된 Object들을 보내주는 함수
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i].activeSelf == false)
            {
                return objPool[i];
            }
        }
        return null;
    }
    public void ShootingBall(Transform tr)
    {
        // Main Ball로 부터 받은 값을 토대로 공들을 발사한다.
        currPoint = tr.right;
        currPos = tr.position;
        SoundManager.instance.PlaySound("shoot");
        StartCoroutine(ShootingBall()); // 공들을 발사하는 IEnumerator
    }

    IEnumerator ShootingBall()
    {
        for (int i = 1; i < stage; i++)
        {
            // 현재 stage의 수 만큼 0.1초마다 공을 생성해 발사한다.
            yield return new WaitForSeconds(0.1f);
            GameObject obj = GetBall();
            obj.transform.position = currPos;
            obj.SetActive(true);
        }
    }
    public bool canNext()
    {
        // 다음 스테이지로 넘어갈 수 있는지를 판단하는 함수
        if (!mainBall.isBased) return false; // 1번 조건 : Main Ball은 바닥에 있어야한다.
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i].activeInHierarchy) return false; // 2번 조건 : 모든 ball 들이 비활성화 되어 있어야 한다.
        }
        return true;
    }

    public void NextStage()
    {
        // Floor의 위치가 제일 아래일 경우, 해당 Floor에 블럭이 아직 남아 있다면 GameOver를 출력.
        // 블럭이 남아 있지 않다면, 해당 Floor를 맨 위로 올리고 다른 Floor들을 한 층씩 내리게 된다.        
        for (int i = 0; i < floors.Length; i++)
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

        stage++; // 점수 증가

        // idx를 토대로 도출한 맨 윗층에 30% 확률로 블럭이 생성되도록 한다.
        for (int i = 0; i < floors[8-idx].blocks.Length; i++)
        {
            floors[8 - idx].blocks[i].gameObject.SetActive(Random.value > 0.7f);
        }
        if (++idx == 9) idx = 0;
        
        // SCORE와 BEST를 업데이트 해준다.
        score.text = "SCORE : " + stage;
        if (bestScore < stage) bestScore = stage;
        best.text = "BEST : " + bestScore;

        // MainBall의 가이드라인을 다시 보여주고, Shooting이 끝났음을 알림
        mainBall.line.enabled = true;
        mainBall.isShoot = false;
    }

    public void OnPauseClicked() // 일시정지 버튼을 클릭했을 때
    {
        isPaused = !isPaused; // Paused의 값을 반대로 바꾼다
        panelPause.SetActive(isPaused); // 값에 따라 Panel을 활성화 / 비활성화
        Time.timeScale = isPaused ? 0.0f : (isX2 ? 2.0f : 1.0f); // 값에 따라 timeScale을 0으로 하거나, isX2에 따라 1 or 2로 변환
        pauseImg.sprite = isPaused ? img[0] : img[1]; // 값에 따라 정지/재생 이미지 변환
        SoundManager.instance.PlaySound("click"); // 클릭 효과음 재생
    }
    public void OnMuteClicked() // Mute 버튼을 클릭했을 때
    {
        isMuted = !isMuted; // Muted의 값을 반대로 바꾼다
        soundImg.enabled = isMuted; // 값에 따라 이미지를 활성화 / 비활성화 (X자 표시가 생기고 안생기고의 차이)
        SoundManager.instance.Mute(isMuted); // 값에 따라 SoundManager를 통해 Mute / UnMute
        SoundManager.instance.PlaySound("click"); // 클릭 효과음 재생
    }
    public void OnTimeClicked() // 시간 배수 버튼을 클릭했을 때
    {
        isX2 = !isX2; // 배수의 값을 반대로 바꾼다.
        Time.timeScale = isX2 ? 2.0f : 1.0f; // 2배면 timeScale을 2, 1배면 1로 설정
        timeTxt.text = isX2 ? "X1" : "X2"; // 버튼의 글씨를 X1, X2로 설정
        SoundManager.instance.PlaySound("click"); // 클릭 효과음 재생
    }
    public void OnResetClicked() // RE 버튼을 클릭했을 때
    {
        Reset.Invoke(); // MainBall이 가지고 있는 Reset 함수를 해당 이벤트에 넣은 뒤, 이를 실행 시킨다.
        StopAllCoroutines(); // 공 발사 Coroutine을 중단시킨다.
        // 모든 objPool에 있는 공들을 비활성화 한다.
        for (int i = 0; i < 100; i++)
        {
            objPool[i].SetActive(false);
        }
        NextStage(); // 다음 스테이지로 강제 이동
        SoundManager.instance.PlaySound("click"); // 클릭 효과음 재생
    }
    public void OnRestartClicked() // ReStart 버튼을 클릭했을 때
    {
        stage = 0; // 점수 초기화
        // 모든 블럭들 초기화
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                floors[i].blocks[j].SetActive(false);
            }
        }
        if (panelResult.activeSelf) panelResult.SetActive(false); // 결과 Panel 비활성화
        OnPauseClicked(); // 일시정지 해제
        OnResetClicked(); // 모든 공들 초기화 및 다음 스테이지로 이동
    }

    public void OnExitClicked() // Exit 버튼을 클릭했을 때
    {
        OnPauseClicked(); // 일시정지 해제
        SceneManager.LoadScene("Title"); // Title Scene으로 이동
    }

    void GameOver() // 게임오버가 되었을 때
    {
        PlayerPrefs.SetInt("Score", bestScore); // bestScore가 갱신되었을 경우 해당 점수를 로컬 저장소에 저장
        isPaused = true; // 일시정지
        panelResult.transform.GetChild(1).GetComponent<Text>().text = "RESULT : " + stage; // 결과 출력
        panelResult.SetActive(true); // 결과 Panel 활성화
    }
}