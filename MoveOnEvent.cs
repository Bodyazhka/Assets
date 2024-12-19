using UnityEngine;
using System.Collections;

public class MoveOnEvent : MonoBehaviour
{
    // Целевая позиция, куда будет перемещаться объект
    public Vector2 targetPosition;

    // Время, за которое объект переместится к целевой позиции
    public float moveDuration = 2f;

    // Флаг, указывающий, происходит ли сейчас перемещение
    private bool isMoving = false;

    // Метод, который запускает перемещение
    public void StartMoving()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToPosition(targetPosition, moveDuration));
        }
    }

    // Корутин для плавного перемещения
    private IEnumerator MoveToPosition(Vector2 target, float duration)
    {
        isMoving = true;
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Интерполяция позиции
            transform.position = Vector2.Lerp(startPosition, target, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедитесь, что объект точно достиг целевой позиции
        transform.position = target;
        isMoving = false;
    }

    // Пример: запуск перемещения при нажатии клавиши "M"
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartMoving();
        }
    }
}
