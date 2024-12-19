using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class PuzzleChecker : MonoBehaviour
{
    public DraggableImage[] draggableObjects; // Список всех фигур с DraggableImage
    public TargetZone[] targetZones;          // Список всех Target
    public Color completedColor; // Цвет, который примут Target при успешном завершении

    public Image A2;
    public Image B2;
    public Image AB1;
    public Image AB2;
    public GameObject AllTargets;
    public GameObject KV;
    public GameObject Square;
    public GameObject ABC;

    public TextMeshProUGUI TextKvadr;
    public TextMeshProUGUI TextBA;
    public TextMeshProUGUI Text2AB;
    public TextMeshProUGUI TextAB;
    public TextMeshProUGUI TextRavn;
    public TextMeshProUGUI TextPlus1;
    public TextMeshProUGUI TextPlus2;
    public TextMeshProUGUI TextFormule;


    public Vector2 targetPositionA2;
    public Vector2 targetPositionB2;
    public Vector2 targetPositionAB1;
    public Vector2 targetPositionAB2;
    public Vector2 targetPositionSquare;
    public Vector2 targetPositionBA;
    public Vector2 targetPositionBkv;


    private bool puzzleComplete = false;
    public float moveDuration;
    public float fadeDuration;


    void Update()
    {
        if (!puzzleComplete && CheckAllTargetsOccupied())
        {
            puzzleComplete = true;
            StartCoroutine(DisableDrag());
        }
    }

    private IEnumerator DisableDrag(){
        yield return new WaitForSeconds(1.0f);
        AllTargets.gameObject.SetActive(false);
        KV.SetActive(true);
        KV.GetComponent<SpriteRenderer>().color = completedColor;
        DisableDraggableScripts();
         yield return new WaitForSeconds(1.0f);
         Move();
    }

    private void Move(){
        StartCoroutine(MoveCor());
    }

    private IEnumerator MoveCor(){
        StartCoroutine(Show(ABC));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(ABC));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(A2, targetPositionA2));
        StartCoroutine(MoveSquare(B2, targetPositionB2));
        StartCoroutine(MoveSquare(AB1, targetPositionAB1));
        StartCoroutine(MoveSquare(AB2, targetPositionAB2));
        StartCoroutine(MoveEmpty(Square.transform, targetPositionSquare));
        StartCoroutine(Show(TextKvadr));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(RotateImageCoroutine(AB2, 90));
        StartCoroutine(Hide(TextBA));
        StartCoroutine(Move2());
        
        
    }

    private IEnumerator Move2(){
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(MoveSquare(AB2, targetPositionBA));
        StartCoroutine(Hide(AB2));
        StartCoroutine(Show(Text2AB));
        TextAB.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(B2, targetPositionBkv));
        StartCoroutine(Show(TextRavn));
        StartCoroutine(Show(TextPlus1));
        StartCoroutine(Show(TextPlus2));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Show(TextFormule));
        
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

    private IEnumerator MoveEmpty(Transform squareTransform, Vector2 targetPosition)
{
    Vector2 startPosition = squareTransform.position; // Получаем начальную позицию пустого объекта
    float elapsedTime = 0;

    while (elapsedTime < moveDuration)
    {
        // Плавно перемещаем пустой объект к целевой позиции
        squareTransform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Устанавливаем точную конечную позицию
    squareTransform.position = targetPosition;
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
