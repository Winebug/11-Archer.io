using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButtonVFX : MonoBehaviour, IPointerClickHandler
{
    public ParticleSystem clickEffect;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickEffect != null)
        {
            clickEffect.Play();
            clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}