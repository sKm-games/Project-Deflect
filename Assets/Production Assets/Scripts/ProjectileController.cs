using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameController gameController;    
    private DifficultyController difficultyController;
    private LevelController levelController;

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
        difficultyController = this.GetComponent<DifficultyController>();
        levelController = this.GetComponent<LevelController>();        

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
        //calc aim offesets        
        Transform target = levelController.GetTarget();
        //Debug.Log($"ProjectileController: {sp.transform.name} to {target.name}");
        float offset = Random.Range(-aimOffset, aimOffset);
        float x = target.position.x + offset;

        //x += sp.transform.position.x < 0 ? 2 : -2;

        offset = Random.Range(-aimOffset, aimOffset);
        float y = target.position.y + offset;

        //y += sp.transform.position.y < 0 ? 2 : -2;

        //get target pos
        Vector3 targetPos = new Vector3(x, y, 0);

        DifficultyDataClass data = difficultyController.GetCurrentDifficulty;

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
        yield return new WaitForSeconds(data.ShotTime);

        sp.UpdateLaserAlpha(0);

        ProjectileObject p = GetProjectile();
        p.Launch(sp.transform, data.ProjectileSpeed);

        gameController.GetSoundController.PlaySFX("shoot");

        sp.IsReady = true;
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
            levelController.Invoke("LevelOver", difficultyController.GetCurrentDifficulty.NextLevelDelay);
        }
    }

}

