using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject howToPlay;

    public void OnPlayClick()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void OnHowToPlayClick()
    {
        howToPlay.SetActive(!howToPlay.activeSelf);
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }
}
