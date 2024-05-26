using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public int space = 1;

    public List<Grabable> _Grabables;

    public bool ReturnInventorySpace() 
    {
        return _Grabables.Count < space;
    }
}