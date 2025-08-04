using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] private bool createDustOnWalk = true; // 걷기 중 파티클 생성 여부
    [SerializeField] private ParticleSystem dustParticleSystem; // 사용할 파티클 시스템
    [SerializeField] private ParticleSystem eyeParticleSystem; // 사용할 파티클 시스템

    public void CreateDustParticles()
    {
        // 조건이 true일 때만 실행
        if (createDustOnWalk)
        {
            dustParticleSystem.Stop(); // 이전 재생을 멈추고
            dustParticleSystem.Play(); // 새로 재생

            // eyeParticleSystem이 재생중이 아니라면 새로 재생
            if (!eyeParticleSystem.isPlaying)
            {
                eyeParticleSystem.Play(); // 새로 재생 
            }
        }

        // eyeParticleSystem이 재생중이 아니라면 새로 재생
        if (!eyeParticleSystem.isPlaying)
        {
            eyeParticleSystem.Play(); // 새로 재생 
        }
    }
}