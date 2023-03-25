using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    
    [SerializeField] private int score;
    [SerializeField] private int lifeTime;
    
    [SerializeField] private ParticleSystem explosion;
    
    private CameraController cameraController;
    private AudioSource blockerHitEffect;
    private Rigidbody2D rb;

    private void Start()
    {
        cameraController = Camera.main.transform.GetComponent<CameraController>();
        blockerHitEffect = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void Launch(Transform sp, float speed)
    {
        this.gameObject.SetActive(true);
        rb.transform.position = sp.transform.position;        
        rb.velocity = sp.transform.up * speed;

        StartCoroutine(IELifeTime());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            gameController.UpdateScore(-score);
            TargetObject b = col.transform.GetComponent<TargetObject>();
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

    private void Explosion()
    {
        cameraController.StartShake();
        ParticleSystem p = Instantiate(explosion);
        p.transform.position = this.transform.position;
        p.gameObject.SetActive(true);
    }

    IEnumerator IELifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        this.gameObject.SetActive(false);
    }
}
