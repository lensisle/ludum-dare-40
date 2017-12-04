using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public bool LeftPressed;
    public bool RightPressed;
    public bool UpPressed;
    public bool DownPressed;

    public bool Active;

    private void Update()
    {
        LeftPressed = ( Convert.ToInt32(Input.GetKey(KeyCode.LeftArrow)) - Convert.ToInt32(Input.GetKey(KeyCode.RightArrow)) ) > 0;
        RightPressed = (Convert.ToInt32(Input.GetKey(KeyCode.RightArrow)) - Convert.ToInt32(Input.GetKey(KeyCode.LeftArrow))) > 0;
        UpPressed = (Convert.ToInt32(Input.GetKey(KeyCode.UpArrow)) - Convert.ToInt32(Input.GetKey(KeyCode.DownArrow))) > 0;
        DownPressed = (Convert.ToInt32(Input.GetKey(KeyCode.DownArrow)) - Convert.ToInt32(Input.GetKey(KeyCode.UpArrow))) > 0;
    }
}
