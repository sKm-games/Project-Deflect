using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private List<Sprite> buildingSprites;
    private SpriteRenderer spriteRenderer;    
    private List<UnityEngine.Rendering.Universal.Light2D> lights;
    private List<ParticleSystem> fireParticles;
    private int fireIndex;

    [SerializeField] int health;
    bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }
    private AudioSource fireEffect;
    private BoxCollider2D targetCollider;


    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        lights = new List<UnityEngine.Rendering.Universal.Light2D>(this.transform.GetComponentsInChildren<UnityEngine.Rendering.Universal.Light2D>());

        fireEffect = GetComponent<AudioSource>();
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
    }

    public void TakeDamage()
    {
        if (!fireEffect.isPlaying)
        {
            fireEffect.Play();
        }
        if (health <= 0)
        {
            return;
        }

        health--;

        fireIndex--;
        fireParticles[fireIndex].gameObject.SetActive(true);
        fireIndex--;
        fireParticles[fireIndex].gameObject.SetActive(true);

        if (health <= 0)
        {
            if (DebugController.infiniteLife)
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

        fireParticles[2].gameObject.SetActive(false);
        fireParticles[3].gameObject.SetActive(false);
        fireParticles[4].gameObject.SetActive(false);
        fireParticles[5].gameObject.SetActive(false);

        targetCollider.enabled = false;

        gameController.UpdateLifes(-1);
        spriteRenderer.sprite = buildingSprites[1];
    }
}
