using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    public static event EventHandler OnMinigameStart;
    public static event EventHandler OnMinigameStop;
    public static EventHandler OnMinigameWin;
    public static EventHandler OnMinigameLose;

    public bool hasBeenPlayed = false;

    public RectTransform indicatorText;

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

    public virtual void ShowIndicator()
    {
        StartCoroutine(DisplayRoutine());
    }

    private void DisplayIndicator()
    {
        indicatorText.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InOutSine);
    }

    private void HideIndicator()
    {
        indicatorText.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InOutSine);
    }    

    private IEnumerator DisplayRoutine()
    {
        indicatorText.gameObject.SetActive(true);
        DisplayIndicator();
        yield return new WaitForSeconds(2);
        HideIndicator();
        yield return new WaitForSeconds(0.5f);
        indicatorText.gameObject.SetActive(false);
    }
}
