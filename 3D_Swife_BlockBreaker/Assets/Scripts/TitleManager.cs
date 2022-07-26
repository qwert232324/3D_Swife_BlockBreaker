using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject howToPlay; // 가이드 이미지
    public void OnPlayClick()
    {
        // Play Scene으로 이동
        SceneManager.LoadScene("PlayScene"); 
    }
    public void OnHowToPlayClick()
    {
        // 가이드 이미지를 껐다 켰다 반복
        howToPlay.SetActive(!howToPlay.activeSelf);
    }
    public void OnQuitClick()
    {
        // 게임 종료
        Application.Quit();
    }
}
