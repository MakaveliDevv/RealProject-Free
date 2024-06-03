using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrbPickup : MonoBehaviour
{
    private GameObject orb;
    private bool isActive = false;
    public GameObject textGameObj;

    void Start()
    {
        orb = GameObject.Find("Joint/PlayerCamera/OrbPickup");
        if (orb != null)
            Debug.Log(orb);
        textGameObj.SetActive(false);
        orb.SetActive(false);
    }

    void Update()
    {
        if (isActive == true && Input.GetKeyDown(KeyCode.E))
        {
            orb.SetActive(true);
            textGameObj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "FirstPersonController")
        {
            Debug.Log("im in");
            textGameObj.SetActive(true);
            isActive = true;
        }
    }
}
