using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMoveManager : MonoBehaviour
{
    public GameObject SettingPanel;

    public void StartScene()
    {
        SceneManager.LoadScene("BuildMainScene");
    }

    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
}
