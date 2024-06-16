using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManag : MonoBehaviour
{
    public PlayerInputAction inputPlayer;

    private void Awake()
    {
        inputPlayer = new PlayerInputAction();
    }

    private void OnEnable() 
    {
        inputPlayer.Enable();
    }

    private void OnDisable()
    {
        inputPlayer.Disable();
    }
}
