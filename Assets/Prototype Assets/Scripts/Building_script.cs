using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDPrototype
{
    public class Building_script : MonoBehaviour
    {
        [SerializeField] List<Sprite> buildingSprites;
        private SpriteRenderer spriteRenderer;
        [SerializeField] GameController_script gameController;
        List<UnityEngine.Rendering.Universal.Light2D> lights;
        List<ParticleSystem> fire;
        [SerializeField] int hp;
        bool dead;
        public bool Dead
        {
            get
            {
                return dead;
            }
        }
        AudioSource fireEffect;
        BoxCollider2D collider;


        private void Start()
        {
            spriteRenderer = this.GetComponent<SpriteRenderer>();
            lights = new List<UnityEngine.Rendering.Universal.Light2D>(this.transform.GetComponentsInChildren<UnityEngine.Rendering.Universal.Light2D>());
            fire = new List<ParticleSystem>(this.transform.GetComponentsInChildren<ParticleSystem>());
            fireEffect = GetComponent<AudioSource>();
            collider = GetComponent<BoxCollider2D>();

            foreach (ParticleSystem p in fire)
            {
                p.gameObject.SetActive(false);
            }

            //spriteRenderer.sprite = buildingSprites[spriteRef];
        }

        public void TakeDamage()
        {
            if (!fireEffect.isPlaying)
            {
                fireEffect.Play();
            }
            if (hp <= 0)
            {
                return;
            }
            hp--;
            fire[hp].gameObject.SetActive(true);
            hp--;
            fire[hp].gameObject.SetActive(true);

            if (hp <= 0)
            {
                RemoveBuilding();
                return;
            }

        }

        void RemoveBuilding()
        {
            //animations of something....
            dead = true;
            lights[2].gameObject.SetActive(false);
            lights[3].gameObject.SetActive(false);
            lights[4].gameObject.SetActive(false);
            lights[5].gameObject.SetActive(false);

            fire[2].gameObject.SetActive(false);
            fire[3].gameObject.SetActive(false);
            fire[4].gameObject.SetActive(false);
            fire[5].gameObject.SetActive(false);

            collider.enabled = false;

            gameController.UpdateLifes(-1);
            spriteRenderer.sprite = buildingSprites[1];
        }
    }
}
