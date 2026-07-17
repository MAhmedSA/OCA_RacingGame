using System;
using UnityEngine;

public interface ISerialReader
{
    event Action<string> OnMessageReceived;
    void Open();
    void Close();
}
