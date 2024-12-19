using UnityEngine;

public class TriggerTester : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{gameObject.name} вошел в триггер с {other.name}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"{gameObject.name} вышел из триггера с {other.name}");
    }
}
