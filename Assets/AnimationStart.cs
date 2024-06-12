using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    private GameObject cutscene;
    public GameObject textGameObj;

    void Start()
    {
        cutscene = GameObject.Find("Animation");
        cutscene.SetActive(false);
        textGameObj.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "FirstPersonController")
        {
            textGameObj.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                cutscene.SetActive(true);
                textGameObj.SetActive(false);
            }
        }

    }
}
