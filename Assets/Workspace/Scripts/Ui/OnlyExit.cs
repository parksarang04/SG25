using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButtonScript : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnExitButtonClick()
    {
        StartCoroutine(DelayedQuit());
    }

    IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(audioManager.clickSound.length + 0.1f);

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
