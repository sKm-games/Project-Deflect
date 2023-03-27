using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    [SerializeField] private GameController gameController;      
    [SerializeField] private int lifeTime;
    
    [SerializeField] private ParticleSystem explosion;
    
    private CameraController cameraController;
    private Rigidbody2D rb;
    private CircleCollider2D circleCol;
    private bool removeOnce;    

    private void Awake()
    {
        cameraController = Camera.main.transform.GetComponent<CameraController>();                 
        rb = this.GetComponent<Rigidbody2D>();
        circleCol = this.GetComponent<CircleCollider2D>();
    }

    public void Launch(Transform sp, float speed)
    {        
        this.gameObject.SetActive(true);
        rb.transform.position = sp.transform.position;
        rb.velocity = sp.transform.up * speed;
        removeOnce = true;
        circleCol.enabled = true;
        StartCoroutine(IELifeTime());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            gameController.GetScoreController.UpdateScore(false);
            TargetObject b = col.transform.GetComponent<TargetObject>();
            b.TakeDamage();
            Explosion();
            RemoveProjectile();            
        }
        else if (col.transform.tag == "Blocker")
        {
            gameController.GetScoreController.UpdateScore(true);
            gameController.GetSoundController.PlaySFX("deflect");         
        }
        else if (col.transform.tag == "Finish")
        {
            RemoveProjectile();
            Explosion();
        }
        else if (col.transform.tag == "Walls")
        {
            RemoveProjectile();
        }        
    }

    private void RemoveProjectile()
    {
        this.gameObject.SetActive(false);
        if (!this.gameObject.activeSelf && removeOnce)
        {
            circleCol.enabled = false;
            gameController.GetProjectilesController.UpdateActiveProjectiles();
            this.transform.position = new Vector3(-50, 0, 0);
            removeOnce = false;
        }
    }

    private void Explosion()
    {
        cameraController.StartShake();
        EffectObject e = gameController.GetProjectilesController.GetEffectObject();
        e.transform.position = this.transform.position;
        e.gameObject.SetActive(true);
        e.DoEffect(gameController.GetSoundController);
    }

    IEnumerator IELifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        RemoveProjectile();
    }
}
