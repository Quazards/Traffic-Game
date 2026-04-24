using System;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    public static event EventHandler OnMinigameStart;
    public static event EventHandler OnMinigameStop;
    public static EventHandler OnMinigameWin;
    public static EventHandler OnMinigameLose;

    public bool hasBeenPlayed = false;

    public abstract void SetupMinigame();
    public abstract void SetupUI();
    public abstract void CloseMinigame();

    public void MinigameWin()
    {
        OnMinigameWin?.Invoke(this, EventArgs.Empty);
        OnMinigameStop?.Invoke(this, EventArgs.Empty);
    }

    public void MinigameLose()
    {
        OnMinigameLose?.Invoke(this, EventArgs.Empty);
        OnMinigameStop?.Invoke(this, EventArgs.Empty);
    }
}
