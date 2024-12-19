using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class FourthLevel : MonoBehaviour
{
    public DraggableImage[] draggableObjects; // Список всех фигур с DraggableImage
    public TargetZone[] targetZones;          // Список всех Target
    public Color completedColor; // Цвет, который примут Target при успешном завершении

    public Image Square;
    public Image A3;
    public Image B3;
    public Image dvAdvB;
    public Image ABdv;
    public Image AdvB;
    public Image dvABdv;
    public GameObject KV;
    public GameObject All;
    public GameObject AllTargets;
    public GameObject ABC;
    public GameObject FullFormule;

    public TextMeshProUGUI Text2A2B;
    public TextMeshProUGUI Text3A2B;
    public TextMeshProUGUI Text2AB2;
    public TextMeshProUGUI Text3AB2;
    public TextMeshProUGUI TextA2B;
    public TextMeshProUGUI TextAB2;
    public TextMeshProUGUI TextRavn;
    public TextMeshProUGUI TextPlus1;
    public TextMeshProUGUI TextPlus2;
    public TextMeshProUGUI TextPlus3;
    public TextMeshProUGUI TextFormule;
    public TextMeshProUGUI AplsB3;
    public TextMeshProUGUI AplsB2;
    public TextMeshProUGUI A2pls2AB;


    public Vector2 FirsttargetPositionSquare;
    public Vector2 targetPositionSquare;
    public Vector2 targetPositionA3;
    public Vector2 targetPosition2A2B;
    public Vector2 targetPositionA2B;
    public Vector2 targetPosition2AB2;
    public Vector2 targetPositionAB2;
    public Vector2 TargetPositionB3;
    public Vector2 FirstPosition2AB2;
    public Vector2 FirstPositionAB2;
    public Vector2 FirstPositionB3;
    public Vector2 NextPositionA2B;
    public Vector2 NextPositionAB2;


    private bool puzzleComplete = false;
    private bool flag = true;
    public float moveDuration;
    public float fadeDuration;


    void Update()
    {
        if(flag){
            StartCoroutine(ShowFullFormule());
            flag = false;
        }
        
        if (!puzzleComplete && CheckAllTargetsOccupied())
        {
            puzzleComplete = true;
            StartCoroutine(Move());
        }
        
    }

    private IEnumerator ShowFullFormule(){
        
        StartCoroutine(Show(AplsB3));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Show(AplsB2));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Show(A2pls2AB));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(FullFormule));
        StartCoroutine(MoveSquare(Square, FirsttargetPositionSquare));
        yield return new WaitForSeconds(1.0f);
        FullFormule.SetActive(false);
        StartCoroutine(Show(All));
    }


    private IEnumerator Move(){
        yield return new WaitForSeconds(1.0f);
        AllTargets.gameObject.SetActive(false);
        KV.gameObject.SetActive(true);
        KV.GetComponent<SpriteRenderer>().color = completedColor;
        DisableDraggableScripts();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Show(ABC));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(ABC));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(Square, targetPositionSquare));
        StartCoroutine(MoveSquare(A3, targetPositionA3));
        StartCoroutine(MoveSquare(dvAdvB, targetPosition2A2B));
        StartCoroutine(MoveSquare(AdvB, targetPositionA2B));
        StartCoroutine(MoveSquare(dvABdv, FirstPosition2AB2));
        StartCoroutine(MoveSquare(ABdv, FirstPositionAB2));
        StartCoroutine(MoveSquare(B3, FirstPositionB3));
         yield return new WaitForSeconds(2.0f);
        TextRavn.gameObject.SetActive(true);
        TextPlus1.gameObject.SetActive(true);
        StartCoroutine(Show(TextRavn));
        StartCoroutine(Show(TextPlus1));
        StartCoroutine(Hide(TextA2B));
        StartCoroutine(MoveSquare(AdvB, NextPositionA2B));
        StartCoroutine(Hide(AdvB));
        StartCoroutine(Show(Text3A2B));
        Text2A2B.gameObject.SetActive(false);
        TextPlus2.gameObject.SetActive(true);
        StartCoroutine(Show(TextPlus2));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(dvABdv, targetPosition2AB2));
        StartCoroutine(MoveSquare(ABdv, targetPositionAB2));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(TextAB2));
        StartCoroutine(MoveSquare(ABdv, NextPositionAB2));
        StartCoroutine(Hide(ABdv));
        StartCoroutine(Show(Text3AB2));
        Text2AB2.gameObject.SetActive(false);
        TextPlus3.gameObject.SetActive(true);
        StartCoroutine(Show(TextPlus3));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(B3, TargetPositionB3));
        yield return new WaitForSeconds(2.0f);
        TextFormule.gameObject.SetActive(true);
        StartCoroutine(Show(TextFormule));
        // y = 0.066     -3.301 -0.277  1.762   3.77
        // 0.863 sq
    }

    private IEnumerator MoveSquare(Image square, Vector2 targetPosition)
    {
        Vector2 startPosition = square.rectTransform.anchoredPosition; // Получаем начальную позицию по X и Y
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            square.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Устанавливаем точную конечную позицию
        square.rectTransform.anchoredPosition = targetPosition;
    }


    // Проверка, что все Target заняты своими фигурами
    private bool CheckAllTargetsOccupied()
    {
        foreach (var target in targetZones)
        {
            if (!target.isOccupied)
                return false;
        }
        return true;
    }

    // Отключение скриптов DraggableImage на всех фигурах
    private void DisableDraggableScripts()
    {
        foreach (var draggable in draggableObjects)
        {
            draggable.enabled = false;
        }
    }

// Корутина для плавного изменения цвета всех Target (для спрайтов)
private IEnumerator ChangeTargetColors()
{
    float duration = 1.0f; // Длительность изменения цвета
    float elapsed = 0f;

    // Сохраняем начальный цвет всех Target
    Color[] originalColors = new Color[targetZones.Length];
    for (int i = 0; i < targetZones.Length; i++)
    {
        originalColors[i] = targetZones[i].GetComponent<SpriteRenderer>().color;
    }

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        for (int i = 0; i < targetZones.Length; i++)
        {
            SpriteRenderer spriteRenderer = targetZones[i].GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.Lerp(originalColors[i], completedColor, t);
        }

        yield return null;
    }
}

private IEnumerator Show(Object some){
        CanvasGroup CanvasGroup = some.GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0;

        float elapsTime = 0;

        // Плавное появление изображения
        while (elapsTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsTime / fadeDuration);
            CanvasGroup.alpha = alpha;
            elapsTime += Time.deltaTime;
            yield return null;
        }
        // Устанавливаем альфа-канал точно в 1
        CanvasGroup.alpha = 1;
    }

    private IEnumerator Hide(Object some){
        CanvasGroup canvasGroup = some.GetComponent<CanvasGroup>();
        float elapsedTime = 0;

            while (elapsedTime < 1.0f)
            {
                float alpha = Mathf.Lerp(1, 0, elapsedTime / 1.0f);
                canvasGroup.alpha = alpha;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

        // Устанавливаем альфа-канал точно в 0
        canvasGroup.alpha = 0;
    }

    private IEnumerator RotateImageCoroutine(Image targetImage, float angle)
    {
        Quaternion initialRotation = targetImage.rectTransform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, angle);

        yield return new WaitForSeconds(1.0f);

        float elapsed = 0f;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 1.0f);
            targetImage.rectTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        // Устанавливаем точное конечное значение
        targetImage.rectTransform.rotation = targetRotation;
    }

}
