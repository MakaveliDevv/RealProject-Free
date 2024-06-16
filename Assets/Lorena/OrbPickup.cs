using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrbPickup : MonoBehaviour
{
    private bool isActive = false;
    public GameObject buttonImage;

    void Start()
    {
        buttonImage.SetActive(false);
    }

    void Update()
    {
        if (isActive == true && Input.GetKeyDown(KeyCode.E))
        {
            buttonImage.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("im in");
            buttonImage.SetActive(true);
            isActive = true;
        }
    }
}
