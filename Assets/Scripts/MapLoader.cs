using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapLoader : MonoBehaviour
{
    public static MapLoader Instance;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void LoadMap(string mapName)
    {
        StartCoroutine(LoadAsync(mapName));
    }

    private IEnumerator LoadAsync(string mapName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(mapName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }
    }
}
