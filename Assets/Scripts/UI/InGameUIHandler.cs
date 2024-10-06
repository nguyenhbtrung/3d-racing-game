using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIHandler : MonoBehaviour
{
    public void BackToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
