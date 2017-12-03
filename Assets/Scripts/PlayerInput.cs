using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public bool LeftPressed;
    public bool RightPressed;
    public bool UpPressed;
    public bool DownPressed;

    public bool ActionPressed;
    public bool MenuPressed;

    public bool Active;

    private void Update()
    {
        LeftPressed = ( Convert.ToInt32(Input.GetKey(KeyCode.A)) - Convert.ToInt32(Input.GetKey(KeyCode.D)) ) > 0;
        RightPressed = (Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A))) > 0;
        UpPressed = (Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S))) > 0;
        DownPressed = (Convert.ToInt32(Input.GetKey(KeyCode.S)) - Convert.ToInt32(Input.GetKey(KeyCode.W))) > 0;

        ActionPressed = Input.GetKey(KeyCode.Space);
        MenuPressed = Input.GetKey(KeyCode.Escape);
    }
}
