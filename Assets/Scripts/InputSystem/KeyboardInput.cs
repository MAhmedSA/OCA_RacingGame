using System;
using UnityEngine;

public class KeyboardInput : IInputProvider
{
    public bool PlayButton => Input.GetKeyDown(KeyCode.P);
    public bool BackButton => Input.GetKeyDown(KeyCode.B);
    
}
