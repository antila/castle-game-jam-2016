using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputNew;
using System.Collections;

public class WinScreen : MonoBehaviour {
    public Camera playerCamera;
    public float originalDepth;
    public Rect originalCameraRect;
    public Rect currentCameraRect;
    public bool showingWinner = false;

    // Use this for initialization
    void OnEnable() {
        StartCoroutine(ReleaseControls());
    }
    void OnDisable()
    {
        showingWinner = false;
    }

    // Update is called once per frame
    void Update () {
        if (showingWinner) {
            currentCameraRect.x = Mathf.Lerp(currentCameraRect.x, 0.0f, Time.deltaTime * 5f);
            currentCameraRect.y = Mathf.Lerp(currentCameraRect.y, 0.0f, Time.deltaTime * 5f);
            currentCameraRect.width = Mathf.Lerp(currentCameraRect.width, 1.0f, Time.deltaTime * 5f);
            currentCameraRect.height = Mathf.Lerp(currentCameraRect.height, 1.0f, Time.deltaTime * 5f);
            playerCamera.rect = currentCameraRect;
        }
    }

    IEnumerator ReleaseControls() {

        yield return new WaitForSeconds(1);
        GetComponentInChildren<Button>().Select();

        var mouse = Mouse.current;
        if (mouse != null)
            mouse.cursor.isLocked = false;

    }

    public void ShowWinner(MultiplayerManager.PlayerInfo winner) {
        var playerCameras = FindObjectsOfType<Camera>();
        playerCamera = playerCameras[0];
        originalDepth = playerCamera.depth;
        playerCamera.depth = 9f;
        originalCameraRect = playerCamera.rect;
        currentCameraRect = originalCameraRect;
        showingWinner = true;
    }
}
