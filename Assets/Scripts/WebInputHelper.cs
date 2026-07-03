using UnityEngine;

// Small helper to request pointer lock on click and provide a basic touch fallback reminder.
[RequireComponent(typeof(Camera))]
public class WebInputHelper : MonoBehaviour
{
    void Start()
    {
        // On WebGL, ensure cursor is visible and instruct user
#if UNITY_WEBGL
        Debug.Log("WebGL: Click the canvas to lock pointer and control the camera.");
#endif
    }

    void Update()
    {
#if UNITY_WEBGL
        // Request pointer lock on first mouse click
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // Unlock on Escape
        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
#endif
    }
}
