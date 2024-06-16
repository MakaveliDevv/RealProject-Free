using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraLook : MonoBehaviour
{
    public InputManag inputManager;
    public float mouseSens = 25f;
    public PlayerController player;

    private float xRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = inputManager.inputPlayer.InputAction.MouseX.ReadValue<float>() * mouseSens;
        float mouseY = inputManager.inputPlayer.InputAction.MouseY.ReadValue<float>() * mouseSens;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
    }
}
