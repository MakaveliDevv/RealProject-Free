using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ClientScript : MonoBehaviour
{
    public static ClientScript instance;
    public string clientName;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Footsteps")
        {
            FootstepSceneSetup();
        }
    }

    void FootstepSceneSetup()
    {
        if (clientName == "Wall")
        {
            Camera.main.transform.position = new Vector3(0, 0, -10);
        }
        else if (clientName == "Floor")
        {
            Camera.main.transform.position = new Vector3(0, -26.8f, -10);
        }
    }
}
