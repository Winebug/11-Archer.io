using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Player
    public float smoothSpeed = 5f;  // 부드러운 추적 속도
    public Vector3 offset;  // 카메라 오프셋 (Player 위쪽이나 살짝 뒤쪽)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}