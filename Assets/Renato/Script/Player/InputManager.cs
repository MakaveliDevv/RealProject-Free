using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManag : MonoBehaviour
{
    public PlayerInputAction InputPlayer;
    public MovementController MovementController;

    private void Awake()
    {
        InputPlayer = new PlayerInputAction();
        MovementController = new MovementController();
    }

    private void OnEnable() 
    {
        InputPlayer.Enable();
        MovementController.Enable();
    }

    private void OnDisable()
    {
        InputPlayer.Disable();
        MovementController.Disable();
    }
}
