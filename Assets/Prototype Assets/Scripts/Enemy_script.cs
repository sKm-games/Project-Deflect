using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDPrototype
{
    public class Enemy_script : MonoBehaviour
    {
        [SerializeField] private int score;
        [SerializeField] private GameController_script gameController;
        [SerializeField] private ParticleSystem explosion;
        private CamShake_script camShake;
        private AudioSource blockerHitEffect;

        private void Start()
        {
            camShake = Camera.main.transform.GetComponent<CamShake_script>();
            blockerHitEffect = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.tag == "Player")
            {
                gameController.UpdateScore(-score);
                Building_script b = col.transform.GetComponent<Building_script>();
                b.TakeDamage();
                this.gameObject.SetActive(false);
                Explosion();
            }
            else if (col.transform.tag == "Blocker")
            {
                gameController.UpdateScore(score);
                blockerHitEffect.Play();
                //this.gameObject.SetActive(false);
            }
            else if (col.transform.tag == "Finish")
            {
                this.gameObject.SetActive(false);
                Explosion();
            }
            else if (col.transform.tag == "Walls")
            {
                this.gameObject.SetActive(false);
            }
        }

        void Explosion()
        {
            camShake.StartShake();
            ParticleSystem p = Instantiate(explosion);
            p.transform.position = this.transform.position;
            p.gameObject.SetActive(true);
        }
    }
}
