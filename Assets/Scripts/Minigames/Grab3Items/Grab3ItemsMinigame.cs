using System;
using UnityEngine;

public class Grab3ItemsMinigame : MinigameBase
{
    public static Action OnItemOverlap;

    [Header("references")]
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private RectTransform SIMAnchor;
    [SerializeField] private RectTransform STNKAnchor;
    [SerializeField] private RectTransform KUBAnchor;
    [SerializeField] private RectTransform SIMObject;
    [SerializeField] private RectTransform STNKObject;
    [SerializeField] private RectTransform KUBObject;

    private float grabbedItemsCount = 0;

    private bool hasWonMinigame = false;

    private void OnEnable()
    {
        OnItemOverlap += IncrementItemCount;
        PostMinigameUI.OnPostGameTimerEnd += ResetMinigame;
        PostMinigameUI.OnPostGameHalfWay += CloseMinigame;
    }

    private void OnDisable()
    {
        OnItemOverlap -= IncrementItemCount;
        PostMinigameUI.OnPostGameTimerEnd -= ResetMinigame;
        PostMinigameUI.OnPostGameHalfWay -= CloseMinigame;
        GameManager.OnTimerFinish -= LoseMinigame;
    }

    private void Start()
    {
        //SetupMinigame();
    }

    private void IncrementItemCount()
    {
        grabbedItemsCount++;

        if(grabbedItemsCount == 3)
        {
            hasWonMinigame = true;
            MinigameWin();
            PostMinigameUI.Instance.OpenWinScreen();
            GameManager.Instance.SetTimerToZero();
        }
    }

    private void LoseMinigame()
    {
        if (hasWonMinigame) return;
        MinigameLose();
        PostMinigameUI.Instance.OpenLoseScreen();

        //Debug.Log("lose minigame");
    }

    private void ResetMinigame()
    {
        grabbedItemsCount = 0;
        SIMObject.position = SIMAnchor.position;
        STNKObject.position = STNKAnchor.position;
        KUBObject.position = KUBAnchor.position;

        SIMObject.gameObject.SetActive(true);
        STNKObject.gameObject.SetActive(true);
        KUBObject.gameObject.SetActive(true);

        hasWonMinigame = false;
    }

    public override void SetupMinigame()
    {
        this.enabled = true;
        GameManager.OnTimerFinish += LoseMinigame;

        ResetMinigame();

        hasWonMinigame = false;
    }

    public override void SetupUI()
    {
        minigameUI.SetActive(true);
    }

    public override void CloseMinigame()
    {
        minigameUI.SetActive(false);
        this.enabled = false;
    }
}
