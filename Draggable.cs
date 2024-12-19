using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private TargetZone currentTargetZone = null; // Объект TargetZone для текущего Target

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset.z = 0;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Проверка на наличие совместимого Target
        if (currentTargetZone != null && currentTargetZone.matchingObjectName == gameObject.name)
        {
            transform.position = currentTargetZone.transform.position;
            currentTargetZone.isOccupied = true; // Обновляем статус TargetZone при правильном совпадении
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var targetZone = other.GetComponent<TargetZone>();
        // Проверка на совпадение имен
        if (targetZone != null && targetZone.matchingObjectName == gameObject.name)
        {
            currentTargetZone = targetZone;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var targetZone = other.GetComponent<TargetZone>();
        if (targetZone != null && targetZone == currentTargetZone)
        {
            currentTargetZone = null;
            targetZone.isOccupied = false; // Сбрасываем статус TargetZone при выходе из неё
        }
    }
}
