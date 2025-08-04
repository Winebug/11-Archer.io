using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("카메라 범위 Y")]
    public float minY = 0f;        // 첫 방 Y 시작
    public float roomHeight = 20f; // 방 높이
    public float maxY;             // 동적으로 조정 가능

    [Header("BossRoom X 범위")]
    public float bossMinX = -25f;  // 보스룸 왼쪽 한계
    public float bossMaxX = 15f;   // 보스룸 오른쪽 한계

    private float fixedX = -12.23f; // 원래대로 X 고정값 롤백
    private bool isBossRoom = false;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        if (isBossRoom)
        {
            // BossRoom: X도 Clamp 범위에서 움직임
            float clampedX = Mathf.Clamp(smoothedPosition.x, bossMinX, bossMaxX);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
        else
        {
            // 일반 방: X를 고정 (-12.23)
            transform.position = new Vector3(fixedX, clampedY, transform.position.z);
        }
    }

    public void SetBossRoom(bool value)
    {
        isBossRoom = value;
    }
}
