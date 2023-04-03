using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private List<Sprite> buildingSprites;
    private SpriteRenderer spriteRenderer;    
    private List<UnityEngine.Rendering.Universal.Light2D> lights;
    private List<ParticleSystem> fireParticles;
    private int fireIndex;

    [SerializeField] int health;
    private int activeHealth;
    private bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }    
    private BoxCollider2D targetCollider;
    private bool doOnce;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        lights = new List<Light2D>(this.transform.GetComponentsInChildren<Light2D>());     
        targetCollider = GetComponent<BoxCollider2D>();
    }

    public void Init()
    {
        fireParticles = new List<ParticleSystem>(this.transform.GetComponentsInChildren<ParticleSystem>());

        foreach (ParticleSystem p in fireParticles)
        {
            p.gameObject.SetActive(false);
        }

        fireIndex = fireParticles.Count;
        activeHealth = health;
    }

    public void TakeDamage()
    {
        if (!doOnce)
        {
            gameController.GetSoundController.PlaySFX("fire");
            doOnce = true;
        }
        if (activeHealth <= 0)
        {
            return;
        }

        activeHealth--;

        fireIndex--;
        fireParticles[fireIndex].gameObject.SetActive(true);
        fireIndex--;
        fireParticles[fireIndex].gameObject.SetActive(true);

        if (activeHealth <= 0)
        {
            if (DebugSystem.InfinitLifeStatic)
            {
                return;
            }

            RemoveBuilding();
            return;
        }

    }

    void RemoveBuilding()
    {
        //animations of something....
        isDead = true;
        lights[2].gameObject.SetActive(false);
        lights[3].gameObject.SetActive(false);
        lights[4].gameObject.SetActive(false);
        lights[5].gameObject.SetActive(false);

        fireParticles[0].gameObject.SetActive(true);
        fireParticles[1].gameObject.SetActive(true);
        fireParticles[2].gameObject.SetActive(false);
        fireParticles[3].gameObject.SetActive(false);
        fireParticles[4].gameObject.SetActive(false);
        fireParticles[5].gameObject.SetActive(false);

        targetCollider.enabled = false;

        gameController.UpdateLifes(-1);
        spriteRenderer.sprite = buildingSprites[1];
    }

    public void ResetTagets()
    {
        foreach (ParticleSystem p in fireParticles)
        {
            p.gameObject.SetActive(false);
        }

        fireIndex = fireParticles.Count;
        activeHealth = health;

        foreach (Light2D l in lights)
        {
            l.gameObject.SetActive(true);
        }

        spriteRenderer.sprite = buildingSprites[0];

        gameController.GetSoundController.StopLoopSFX("fire");

        targetCollider.enabled = true;

        isDead = false;

        doOnce = false;
        
    }
}
