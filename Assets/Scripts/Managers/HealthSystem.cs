using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    public static Action OnZeroHealth;

    [Header("Reference")]
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [Header("Settings")]
    public int CurrentHealth { get; set; } = 4;
    [SerializeField] private Image[] hearts;

    public bool onZeroHealth = false;
    private bool canLoseHealth = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        PostMinigameUI.OnPostGameTimerEnd += DisableLoseHealth;
        MinigameBase.OnMinigameLose += EnableLoseHealth;
        PostMinigameUI.OnPostGameHalfWay += LoseHealth;
        PostMinigameUI.OnPostGameThreeFourths += CheckLoseCondition;
        GameManager.OnGameStart += ResetHealth;
    }

    private void OnDisable()
    {
        PostMinigameUI.OnPostGameTimerEnd -= DisableLoseHealth;
        MinigameBase.OnMinigameLose -= EnableLoseHealth;
        PostMinigameUI.OnPostGameHalfWay -= LoseHealth;
        PostMinigameUI.OnPostGameThreeFourths -= CheckLoseCondition;
        GameManager.OnGameStart -= ResetHealth;
    }

    private void CheckHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < CurrentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    private void EnableLoseHealth(object sender, System.EventArgs e)
    {
        canLoseHealth = true;
    }

    private void DisableLoseHealth()
    {
        canLoseHealth = false;
    }

    private void LoseHealth()
    {
        if(canLoseHealth)
        {
            ChangeHeartAmount(-1);
        }
    }

    private void CheckLoseCondition()
    {
        if (CurrentHealth <= 0)
        {
            onZeroHealth = true;
            OnZeroHealth?.Invoke();
        }
    }

    public void ChangeHeartAmount(int amount)
    {
        CurrentHealth += amount;
        CheckHearts();
    }

    public void ResetHealth()
    {
        CurrentHealth = 4;
        onZeroHealth = false;
        CheckHearts();
    }

}
