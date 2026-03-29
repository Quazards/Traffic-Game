using System;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    public static event EventHandler OnMinigameStart;
    public static event EventHandler OnMinigameStop;
}
