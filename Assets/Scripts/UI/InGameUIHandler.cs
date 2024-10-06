using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void BackToMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        optionsPanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        optionsPanel.SetActive(false);
    }

}
