using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Unity.Netcode;

public class ClientScript : NetworkBehaviour
{
    public static ClientScript instance;
    public string clientName;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Footsteps")
        {
            FootstepSceneSetup();
        }
        if (clientName == "Wall")
        {
            NetworkManager.StartHost();
        }
        else if (clientName == "Floor")
        {
            NetworkManager.StartClient();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //if (IsServer)
            //{
                string m_SceneName = "Canvas";
                var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
                if (status != SceneEventProgressStatus.Started)
                {
                    Debug.LogWarning($"Failed to load {m_SceneName} " +
                            $"with a {nameof(SceneEventProgressStatus)}: {status}");
                }
            //}
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
