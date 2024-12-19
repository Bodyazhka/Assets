using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Для TextMeshProUGUI


public class GameController : MonoBehaviour
{
    public TargetZone[] targetZones;
    public TextMeshProUGUI congratulationsText;
    private bool congratulationsShown = false;

    void Start()
    {
        // Устанавливаем начальную прозрачность текста на 0 (чтобы он был невидим)
        congratulationsText.canvasRenderer.SetAlpha(0f);
    }

    void Update()
    {
        bool allTargetsOccupied = true;
        foreach (var target in targetZones)
        {
            if (!target.isOccupied)
            {
                allTargetsOccupied = false;
                break;
            }
        }

        if (allTargetsOccupied && !congratulationsShown)
        {
            congratulationsShown = true;
            StartCoroutine(ShowCongratulations());
        }
    }

    private IEnumerator ShowCongratulations()
    {
        // Плавное появление текста
        congratulationsText.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(2f);
        
        // Плавное исчезновение текста
        congratulationsText.CrossFadeAlpha(0f, 1f, false);
    }
}
