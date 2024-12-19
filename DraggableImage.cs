using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    public TargetZone assignedTarget; // Определенный Target для этой фигуры

    private bool isOverTarget = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Если CanvasGroup не добавлен, добавляем его автоматически
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Ищем Canvas для корректного перетаскивания UI-элемента
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Отключаем блокировку лучей, чтобы было легче перемещать объект
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Перемещаем объект в зависимости от позиции указателя
        if (parentCanvas != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, eventData.position, parentCanvas.worldCamera, out pos);
            rectTransform.localPosition = pos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Если фигура была над своим Target при отпускании
        if (isOverTarget && assignedTarget != null)
        {
            // Устанавливаем позицию по центру целевого объекта
            rectTransform.position = assignedTarget.GetComponent<RectTransform>().position;
            assignedTarget.isOccupied = true;
        }
        else
        {
            // Если не над Target, возвращаем на исходную позицию (можно добавить возвращение, если нужно)
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что фигура пересекает свой определенный Target
        if (collision.gameObject == assignedTarget.gameObject)
        {
            isOverTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Если фигура покидает Target
        if (collision.gameObject == assignedTarget.gameObject)
        {
            isOverTarget = false;
        }
    }
}
