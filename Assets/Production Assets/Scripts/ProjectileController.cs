using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private int activeProjectiles;
    [SerializeField] private float aimOffset;
    [SerializeField] private Transform projectileHolder;
    [SerializeField] private List<ProjectileObject> projectilesList;
    [SerializeField] bool allowNewProjectile;

    [SerializeField] private Transform ExplosionHolder;
    [SerializeField] private List<EffectObject> explosionsList;
    [SerializeField] bool allowNewExplosion;

    void Start()
    {
        projectilesList = new List<ProjectileObject>(projectileHolder.GetComponentsInChildren<ProjectileObject>());

        foreach (ProjectileObject rb in projectilesList)
        {
            rb.gameObject.SetActive(false);
        }

        explosionsList = new List<EffectObject>(ExplosionHolder.GetComponentsInChildren<EffectObject>());
        
        foreach (EffectObject e in explosionsList)
        {
            e.gameObject.SetActive(false);
        }
    }

    public void DoLaunchProjectile(SpawnpointObject sp)
    {
        if (sp == null)
        {
            DebugSystem.UpdateDebugText("ProjectileController: DoLaunchProjectile: sp == null");
            return;
        }
        StartCoroutine(IELaunchProjectile(sp));
    }

    IEnumerator IELaunchProjectile(SpawnpointObject sp)
    {
        if (sp == null)
        {
            DebugSystem.UpdateDebugText("ProjectileController: IELaunchProjectile: sp == null");
            yield break;
        }
        activeProjectiles++;

        //get target pos
        Vector3 targetPos = FindAimOffsett(sp);

        DifficultyDataClass data = ReferencesController.GetDifficultyController.GetCurrentDifficulty;

        sp.SetAimInfo(data.AimLaserSize, data.AimLaserColor, targetPos);

        float alpha = 0;
        float timer = 0;

        while (timer < data.AimTime)
        {
            float alphaIncrease = 1 / data.AimTime;
            alpha += alphaIncrease * Time.deltaTime;
            timer += (Time.deltaTime);            
            sp.UpdateLaserAlpha(alpha);
            yield return null;
        }

        sp.SetShootInfo(data.ShotLaserSize, data.ShotLaserColor);

        ReferencesController.GetSoundController.PlaySFX("shoot");
        
        yield return new WaitForSeconds(data.ShotTime);

        sp.UpdateLaserAlpha(0);

        ReferencesController.GetSettingsController.CheckVibration(VibrationTriggerEnums.Shot);
        ProjectileObject p = GetProjectile();
        p.Launch(sp.transform, data.ProjectileSpeed);

        //gameController.GetSoundController.PlaySFX("shoot");

        sp.IsReady = true;
    }

    private Vector3 FindAimOffsett(SpawnpointObject sp)
    {
        //calc aim offesets        
        Transform target = ReferencesController.GetLevelController.GetTarget();

        float minOffset = sp.transform.position.x <= -3 ? 0 : -aimOffset;
        float maxOffset = sp.transform.position.x >= 3 ? 0 : aimOffset;

        //float offset = Random.Range(-aimOffset, aimOffset);
        float offset = Random.Range(minOffset, maxOffset);
        float x = target.position.x + offset;

        //offset = Random.Range(-aimOffset, aimOffset);

        offset = Random.Range(minOffset, maxOffset);
        float y = target.position.y + offset;

        return new Vector3(x, y, 0);
    }

    private ProjectileObject GetProjectile()
    {
        foreach (ProjectileObject p in projectilesList)
        {
            if (!p.gameObject.activeInHierarchy)
            {
                return p;
            }
        }
        if (!allowNewProjectile)
        {
            Debug.LogWarning("ProjectileController: No avaible projectile and not allow to make more");
            return null;
        }

        ProjectileObject np = Instantiate(projectilesList[0], projectilesList[0].transform.parent);
        projectilesList.Add(np);

        return np;
    }

    public EffectObject GetEffectObject()
    {
        foreach (EffectObject e in explosionsList)
        {
            if (!e.gameObject.activeInHierarchy)
            {
                return e;
            }
        }
        if (!allowNewExplosion)
        {
            Debug.LogWarning("ProjectileController: No avaible explosion and not allow to make more");
            return null;
        }

        EffectObject ne = Instantiate(explosionsList[0], explosionsList[0].transform.parent);
        explosionsList.Add(ne);

        return ne;
    }

    private int GetRandom(int max, int previus)
    {
        int r = Random.Range(0, max);
        if (r == previus)
        {
            r = r == 0 ? r++ : r--;
        }
        return r;
    }

    public void UpdateActiveProjectiles()
    {
        activeProjectiles--;
        if (activeProjectiles <= 0)
        {
            float d = ReferencesController.GetDifficultyController.GetCurrentDifficulty.NextLevelDelay;
            ReferencesController.GetLevelController.Invoke("LevelOver", d);
        }
    }

    public void StopProgress()
    {
        StopAllCoroutines();
        activeProjectiles = 0;

        foreach (ProjectileObject po in projectilesList)
        {
            po.RemoveProjectile();
        }

        foreach (EffectObject eo in explosionsList)
        {
            eo.RemoveEffect();
        }
    }

}

