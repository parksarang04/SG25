using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Manager : MonoBehaviour
{
    public GameObject sPanel;
    public GameObject Buttons;

    
    void Start()
    {
        Buttons.SetActive(true);
        sPanel.SetActive(false);
    }

    
    void Update()
    {
        Esc();
    }

    public void StartButton()
    {
        SceneManager.LoadScene("BuildMainScene");
    }   

    public void SettingButton()
    {
        Time.timeScale = 0;
        Buttons.SetActive(false);
        sPanel.SetActive(true);

        FindObjectOfType<AudioManager>().UpdateToggleListenersInPanel(sPanel);
        FindObjectOfType<AudioManager>().UpdateButtonListenersInPanel(sPanel);
    }

    

    public void Esc()
    {
        if(sPanel == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                sPanel.SetActive(false);
                Buttons.SetActive(true);
            }
        }
    }
}
