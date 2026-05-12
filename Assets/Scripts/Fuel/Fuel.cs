using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject fuel;
    [SerializeField] private TextMeshProUGUI scoreGainText;
    [SerializeField] private TextMeshProUGUI timeGainedText;

    private bool isEnabled = false;
    private float ScoreGained = 50;
    private float TimeGained = 0.5f;

    public void EnableFuel()
    {
        if (isEnabled) return;
        EnableObject(fuel);
        GameManager.Instance.IncrementFuelCount();
        isEnabled = true;
    }

    public void DisableFuel()
    {
        DisableObject(fuel);
        isEnabled = false;
    }

    public void GainFuel()
    {

        float random = Random.Range(0, 2);

        switch(random)
        {
            case 0f:
                scoreGainText.text = "+" + ScoreGained.ToString() + " Score";
                GameManager.Instance.GainScore(ScoreGained);
                StartCoroutine(EnableThenDisableObject(scoreGainText.gameObject));
                break;
            case 1f:
                timeGainedText.text = "+" + TimeGained.ToString() + " Time";
                GameManager.Instance.GainTime(TimeGained);
                StartCoroutine(EnableThenDisableObject(timeGainedText.gameObject));
                break;
        }

        GameManager.Instance.DecrementFuelCount();
        StartCoroutine(DisableRoutine(fuel));
        isEnabled = false;
    }

    private IEnumerator DisableRoutine(GameObject obj)
    {
        obj.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
    }

    private IEnumerator EnableThenDisableObject(GameObject obj)
    {
        EnableObject(obj);
        yield return new WaitForSeconds(1);
        StartCoroutine(DisableRoutine(obj));
    }

    private void EnableObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InOutSine);
    }

    private void DisableObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.DOScale(new Vector3(0.11f, 0.11f, 0.11f), 0.5f).SetEase(Ease.InOutSine);
    }
}

