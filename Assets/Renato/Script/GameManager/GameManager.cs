using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inspectPanel;

    void Update()
    {
        if(InspectObject.instance.inspectMode)
            inspectPanel.SetActive(true);

        else
            inspectPanel.SetActive(false);
    }
}
