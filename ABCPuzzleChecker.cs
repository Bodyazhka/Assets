using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class ABCPuzzleChecker : MonoBehaviour
{
    public DraggableImage[] draggableObjects; // Список всех фигур с DraggableImage
    public TargetZone[] targetZones;          // Список всех Target
    public Color completedColor; // Цвет, который примут Target при успешном завершении

    public Image Square;
    public Image A2;
    public Image B2;
    public Image C2;
    public Image AB;
    public Image BA;
    public Image AC;
    public Image CA;
    public Image BC;
    public Image CB;
    public GameObject KV;
    public Image AllTargets;
    public GameObject ABC;

    public TextMeshProUGUI TextKvadr;
    public TextMeshProUGUI TextAB;
    public TextMeshProUGUI Text2AB;
    public TextMeshProUGUI TextBA;
    public TextMeshProUGUI TextAC;
    public TextMeshProUGUI Text2AC;
    public TextMeshProUGUI TextCA;
    public TextMeshProUGUI TextBC;
    public TextMeshProUGUI Text2BC;
    public TextMeshProUGUI TextCB;
    public TextMeshProUGUI TextRavn;
    public TextMeshProUGUI TextPlus1;
    public TextMeshProUGUI TextPlus2;
    public TextMeshProUGUI TextPlus3;
    public TextMeshProUGUI TextPlus4;
    public TextMeshProUGUI TextPlus5;
    public TextMeshProUGUI TextFormule;

    public Vector2 targetPositionSquare;
    public Vector2 targetPositionA2;
    public Vector2 targetPositionB2;
    public Vector2 targetPositionC2;
    public Vector2 targetPositionAB;
    public Vector2 targetPositionBA;
    public Vector2 FirstPositionAC;
    public Vector2 FirstPositionCA;
    public Vector2 FirstPositionBC;
    public Vector2 FirstPositionCB;
    public Vector2 targetPositionAC;
    public Vector2 targetPositionCA;
    public Vector2 targetPositionBC;
    public Vector2 targetPositionCB;
    public Vector2 NextPositionBA;
    public Vector2 NextPositionCA;
    public Vector2 NextPositionCB;


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
        StartCoroutine(MoveSquare(Square, targetPositionSquare));
        StartCoroutine(MoveSquare(A2, targetPositionA2));
        StartCoroutine(MoveSquare(B2, targetPositionB2));
        StartCoroutine(MoveSquare(C2, targetPositionC2));
        StartCoroutine(MoveSquare(AB, targetPositionAB));
        StartCoroutine(MoveSquare(BA, targetPositionBA));
        StartCoroutine(MoveSquare(AC, FirstPositionAC));
        StartCoroutine(MoveSquare(CA, FirstPositionCA));
        StartCoroutine(MoveSquare(BC, FirstPositionBC));
        StartCoroutine(MoveSquare(CB, FirstPositionCB));
        StartCoroutine(Show(TextKvadr));
        StartCoroutine(Show(TextRavn));
        StartCoroutine(Show(TextPlus1));
        StartCoroutine(Show(TextPlus2));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Hide(TextBA));
        StartCoroutine(RotateImageCoroutine(BA, 90));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(BA));
        StartCoroutine(MoveSquare(BA, NextPositionBA));
        StartCoroutine(Show(Text2AB));
        StartCoroutine(Show(TextPlus3));
        TextAB.enabled = false;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(AC, targetPositionAC));
        StartCoroutine(MoveSquare(CA, targetPositionCA));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Hide(TextCA));
        StartCoroutine(RotateImageCoroutine(CA, 90));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(CA));
        StartCoroutine(MoveSquare(CA, NextPositionCA));
        StartCoroutine(Show(Text2AC));
        StartCoroutine(Show(TextPlus4));
        TextAC.enabled = false;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveSquare(BC, targetPositionBC));
        StartCoroutine(MoveSquare(CB, targetPositionCB));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Hide(TextCB));
        StartCoroutine(RotateImageCoroutine(CB, 90));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Hide(CB));
        StartCoroutine(MoveSquare(CB, NextPositionCB));
        StartCoroutine(Show(Text2BC));
        StartCoroutine(Show(TextPlus5));
        TextBC.enabled = false;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Show(TextFormule));
        // -7.333   -3.558  -0.945  0.539   3.996   7.484
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
