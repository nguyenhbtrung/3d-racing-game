using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Camera panomaricCamera;
    [SerializeField] private GameObject panomaricMinimap;
    [SerializeField] private Transform panomaricDestination;

    private RacerWaypointFollower playerWaypointFollower;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerWaypointFollower = player.GetComponent<RacerWaypointFollower>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchMinimap();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerWaypointFollower.BackOnTrack();
        }
    }

    private void SwitchMinimap()
    {
        if (!panomaricMinimap.activeInHierarchy)
        {
            panomaricCamera.transform.SetPositionAndRotation(minimapCamera.transform.position, minimapCamera.transform.rotation);
            panomaricCamera.orthographicSize = minimapCamera.orthographicSize;
            StartCoroutine(MinimapToPanomaric());
        }
        else
        {
            panomaricCamera.transform.SetPositionAndRotation(minimapCamera.transform.position, minimapCamera.transform.rotation);
            panomaricCamera.orthographicSize = 1020.0f;
            
            StartCoroutine(MinimapFromPanomaric());
        }
    }

    private IEnumerator MinimapToPanomaric()
    {
        float duration = 0.5f;
        float timeElapsed = 0.0f;
        Vector3 startPos = minimapCamera.transform.position;
        Vector3 endPos = panomaricDestination.position;
        Quaternion startRot = minimapCamera.transform.rotation;
        Quaternion endRot = panomaricDestination.rotation;
        float startCamSize = minimapCamera.orthographicSize * 2f;
        float endCamSize = 1020.0f;
        panomaricMinimap.SetActive(true);
        while (timeElapsed <= duration)
        {
            float k = timeElapsed / duration;
            k = (k > 1) ? 1 : k;
            panomaricCamera.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, k), Quaternion.Lerp(startRot, endRot, k));
            panomaricCamera.orthographicSize = Mathf.Lerp(startCamSize, endCamSize, k);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panomaricCamera.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, 1), Quaternion.Lerp(startRot, endRot, 1));
        panomaricCamera.orthographicSize = Mathf.Lerp(startCamSize, endCamSize, 1);

    }

    private IEnumerator MinimapFromPanomaric()
    {
        float duration = 0.5f;
        float timeElapsed = 0.0f;
        Vector3 startPos = panomaricDestination.position;
        Vector3 endPos = minimapCamera.transform.position;
        Quaternion startRot = panomaricDestination.rotation;
        Quaternion endRot = minimapCamera.transform.rotation;
        float startCamSize = 1020.0f;
        float endCamSize = minimapCamera.orthographicSize * 2f;
        while (timeElapsed <= duration)
        {
            float k = timeElapsed / duration;
            k = (k > 1) ? 1 : k;
            panomaricCamera.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, k), Quaternion.Lerp(startRot, endRot, k));
            panomaricCamera.orthographicSize = Mathf.Lerp(startCamSize, endCamSize, k);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panomaricCamera.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, 1), Quaternion.Lerp(startRot, endRot, 1));
        panomaricCamera.orthographicSize = Mathf.Lerp(startCamSize, endCamSize, 1);
        panomaricMinimap.SetActive(false);
    }

    public void BackToMenuScene()
    {
        Time.timeScale = 1;
        MapLoader.Instance.LoadMap("Menu");
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
