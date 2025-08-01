using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("카메라 범위")]
    public float minY = 0f;        // 첫 방 시작 Y
    public float roomHeight = 20f; // 방 간격
    public float maxY;             // 동적으로 변경됨

    private float fixedX = -12.23f;

    private int currentRoomIndex = 0; // 현재 방 인덱스 (0부터 시작)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // X는 고정, Y는 Clamp
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        transform.position = new Vector3(fixedX, clampedY, transform.position.z);
    }
    public void EnterRoom(int roomIndex)
    {
        currentRoomIndex = roomIndex;
        maxY = minY + (currentRoomIndex + 1) * roomHeight;
    }

}
