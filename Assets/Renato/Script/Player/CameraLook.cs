// using UnityEngine;


// public class CameraLook : MonoBehaviour
// {
//     public InputManag inputManager;
//     public float mouseSens = 25f;
//     public PlayerController player;

//     private float xRot = 0;
//     // Start is called before the first frame update
//     void Start()
//     {
//         Cursor.lockState = CursorLockMode.Locked;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         float mouseX = inputManager.InputPlayer.InputAction.MouseX.ReadValue<float>() * mouseSens;
//         float mouseY = inputManager.InputPlayer.InputAction.MouseY.ReadValue<float>() * mouseSens;

//         xRot -= mouseY;
//         xRot = Mathf.Clamp(xRot, -90f, 90f);

//         transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
//         player.transform.Rotate(Vector3.up * mouseX);
//     }
// }
