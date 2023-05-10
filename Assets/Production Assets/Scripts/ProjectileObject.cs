using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{     
    [SerializeField] private int lifeTime;
    
    [SerializeField] private ParticleSystem explosion;
    
    private Rigidbody2D rb;
    private CircleCollider2D circleCol;
    private bool removeOnce;
    private bool deflected;
    private bool runOnce;

    private void Awake()
    {           
        rb = this.GetComponent<Rigidbody2D>();
        circleCol = this.GetComponent<CircleCollider2D>();
    }

    public void Launch(Transform sp, float speed)
    {
        runOnce = false;
        deflected = false;
        this.gameObject.SetActive(true);
        rb.transform.position = sp.transform.position;
        rb.velocity = sp.transform.up * speed;
        removeOnce = true;
        circleCol.enabled = true;
        StartCoroutine(IELifeTime());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player" && !runOnce)
        {
            runOnce = true;
            ReferencesController.GetScoreController.UpdateScore(false);
            TargetObject b = col.transform.GetComponent<TargetObject>();
            b.TakeDamage();            
            Explosion();
            RemoveProjectile();

            if (deflected)
            {
                ReferencesController.GetAchievementController.CheckOtherAchievements("FriendlyFire", 1);
            }

            ReferencesController.GetSettingsController.CheckVibration(VibrationTriggerEnums.Hit);
        }
        else if (col.transform.tag == "Blocker")
        {
            ReferencesController.GetScoreController.UpdateScore(true);
            ReferencesController.GetSoundController.PlaySFX("deflect");

            deflected = true;

            ReferencesController.GetSettingsController.CheckVibration(VibrationTriggerEnums.Hit);
        }
        else if (col.transform.tag == "Finish")
        {
            RemoveProjectile();
            Explosion();

            ReferencesController.GetSettingsController.CheckVibration(VibrationTriggerEnums.Hit);
        }
        else if (col.transform.tag == "Walls")
        {
            RemoveProjectile();
        }        
    }

    public void RemoveProjectile()
    {
        this.gameObject.SetActive(false);
        if (!this.gameObject.activeSelf && removeOnce)
        {
            circleCol.enabled = false;
            ReferencesController.GetProjectileController.UpdateActiveProjectiles();
            this.transform.position = new Vector3(-50, 0, 0);
            removeOnce = false;
        }
    }

    private void Explosion()
    {
        ReferencesController.GetCameraController.StartShake();
        EffectObject e = ReferencesController.GetProjectileController.GetEffectObject();
        e.transform.position = this.transform.position;
        e.gameObject.SetActive(true);
        e.DoEffect();
    }

    IEnumerator IELifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        RemoveProjectile();
    }
}
