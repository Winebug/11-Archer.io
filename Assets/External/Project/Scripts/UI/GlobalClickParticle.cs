using UnityEngine;

public class GlobalClickParticle : MonoBehaviour
{
    public Camera mainCamera;           // 클릭 좌표 변환용 카메라
    public ParticleSystem clickEffect;  // 클릭 이펙트

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭 감지
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

            // 파티클 위치 이동 및 재생
            clickEffect.transform.position = worldPos;
            clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            clickEffect.Play();
        }
    }
}
