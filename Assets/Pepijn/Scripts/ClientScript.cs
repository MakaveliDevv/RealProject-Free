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
    [SerializeField] GameObject wallCam, floorCam;

    public string nextSceneName;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (clientName == "Wall")
        {
            NetworkManager.StartHost();
        }
        else if (clientName == "Floor")
        {
            NetworkManager.StartClient();
        }

        CameraSetup();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Voeten2")
        {
            FootstepSceneSetup();
        }

        if (SceneManager.GetActiveScene().name == "Lorena (Scene 1)")
        {
            FirstSceneSetup();
        }

        if (SceneManager.GetActiveScene().name == "Canvas")
        {
            CanvasSetup();
        }
    }

    void Update()
    {
        //DEV SHORTCUT
        if (Input.GetKeyDown(KeyCode.P))
        {
            //if (IsServer)
            //{
            string m_SceneName = "";
            if (SceneManager.GetActiveScene().name == "Lorena (Scene 1)")
            {
                m_SceneName = "Voeten2";
            }
            if (SceneManager.GetActiveScene().name == "Voeten2")
            {
                m_SceneName = "Canvas";
            }
                var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
                if (status != SceneEventProgressStatus.Started)
                {
                    Debug.LogWarning($"Failed to load {m_SceneName} " +
                            $"with a {nameof(SceneEventProgressStatus)}: {status}");
                }
            //}
        }
    }

    void CameraSetup()
    {
        floorCam = GameObject.Find("FloorCam");
        wallCam = GameObject.Find("Main Camera");

        if (floorCam != null && wallCam != null)
        {
            if (clientName == "Wall")
            {
                //Camera.main.transform.position = new Vector3(0, 0, -10);
                wallCam.gameObject.SetActive(true);
                floorCam.gameObject.SetActive(false);
            }
            else if (clientName == "Floor")
            {
                //Camera.main.transform.position = new Vector3(0, -26.8f, -10);
                floorCam.gameObject.SetActive(true);
                wallCam.gameObject.SetActive(false);
            }
        }
    }

    void FootstepSceneSetup()
    {
        CameraSetup();
    }

    void FirstSceneSetup()
    {
        CameraSetup();
    }

    void CanvasSetup()
    {
        CameraSetup();
    }
}
