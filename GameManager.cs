using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool, bool> OnStart;

    public void OnStartButton()
    {
        OnStart?.Invoke(true, false);
    }
}
