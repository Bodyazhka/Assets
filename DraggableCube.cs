using UnityEngine;

public class DraggableCube : MonoBehaviour
{
    public string targetMagnetName; // Имя или тег объекта, к которому можно примагнититься
    private bool isDragging = false; // Перетаскивается ли куб
    private bool isInMagnetZone = false; // Находится ли куб в зоне триггера
    private Transform magnetZoneTransform; // Ссылка на объект триггера
    private Vector3 offset;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Если мышь отпущена и куб находится в разрешённой зоне примагничивания
        if (isInMagnetZone && magnetZoneTransform != null)
        {
            transform.position = magnetZoneTransform.position; // Примагничиваем куб к центру
            Debug.Log($"Куб примагничен к {magnetZoneTransform.name}!");
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли триггер целевым объектом
        if (other.name == targetMagnetName)
        {
            isInMagnetZone = true; // Куб находится в зоне
            magnetZoneTransform = other.transform; // Сохраняем ссылку на объект триггера
            Debug.Log($"Куб вошёл в разрешённую зону примагничивания: {other.name}!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Сбрасываем состояние, только если выходим из разрешённой зоны
        if (other.name == targetMagnetName)
        {
            isInMagnetZone = false; // Куб вышел из зоны
            magnetZoneTransform = null; // Сбрасываем ссылку на триггер
            Debug.Log($"Куб покинул зону примагничивания: {other.name}!");
        }
    }
}
