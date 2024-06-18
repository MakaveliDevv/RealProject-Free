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

        floorCam = GameObject.Find("FloorCam");
        wallCam = GameObject.Find("Main Camera");

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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Footsteps")
        {
            FootstepSceneSetup();
        }

        if (SceneManager.GetActiveScene().name == "Lorena (Scene 1)")
        {
            FirstSceneSetup();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //if (IsServer)
            //{
            string m_SceneName = "";
            if (SceneManager.GetActiveScene().name == "Lorena (Scene 1)")
            {
                m_SceneName = "Footsteps";
            }
            if (SceneManager.GetActiveScene().name == "Footsteps")
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

    void FootstepSceneSetup()
    {
        floorCam = GameObject.Find("FloorCam");
        wallCam = GameObject.Find("Main Camera");

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

    void FirstSceneSetup()
    {
        floorCam = GameObject.Find("FloorCam");
        wallCam = GameObject.Find("Main Camera");
        
        if (clientName == "Wall")
        {
            floorCam.gameObject.SetActive(false);
        }
        else if (clientName == "Floor")
        {
            wallCam.gameObject.SetActive(false);
            //Camera.main.SetActive(false);
        }
    }
}
