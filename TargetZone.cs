using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public bool isOccupied = false;
    public string matchingObjectName; // Имя совместимой фигуры

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка: является ли объект совместимым с этим Target
        if (other.name == matchingObjectName)
        {
            isOccupied = true;
            Debug.Log($"{other.name} корректно встал в {gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == matchingObjectName)
        {
            isOccupied = false;
            Debug.Log($"{other.name} покинул зону {gameObject.name}");
        }
    }
}
