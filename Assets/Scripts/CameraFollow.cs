using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 initialOffset;
    public Quaternion initialRotation;

    void LateUpdate()
    {
        if (target != null)
        {
            // Обновляем позицию камеры относительно цели
            transform.position = target.position + initialOffset;
            // Сохраняем начальный поворот камеры
            transform.rotation = initialRotation;
        }
        else
        {
            // Если цель уничтожена, удаляем этот компонент
            Destroy(this);
        }
    }
}