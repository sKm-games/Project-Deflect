using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem effectParticles;
    [SerializeField] private float lifeTime;

    public void DoEffect()
    {
        effectParticles.Play();
        ReferencesController.GetSoundController.PlaySFX("explosion");
        Invoke("RemoveEffect", lifeTime);
    }

    public void RemoveEffect()
    {
        effectParticles.Stop();
        this.gameObject.SetActive(false);
    }
}
