using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameController gameController;    
    private DifficultyController difficultyController;
    private LevelController levelController;
    
    
    [SerializeField] private Transform projectileHolder;
    [SerializeField] private List<ProjectileObject> projectilesList;
    [SerializeField] bool allowNewProjectile;

    private AudioSource shootEffectSource;   

    void Start()
    {
        difficultyController = this.GetComponent<DifficultyController>();
        levelController = this.GetComponent<LevelController>();
        shootEffectSource = this.GetComponent<AudioSource>();

        projectilesList = new List<ProjectileObject>(projectileHolder.GetComponentsInChildren<ProjectileObject>());

        foreach (ProjectileObject rb in projectilesList)
        {
            rb.gameObject.SetActive(false);
        }        
    }

    public void DoLaunchProjectile(SpawnpointObject sp)
    {
        StartCoroutine(IELaunchProjectile(sp));
    }

    IEnumerator IELaunchProjectile(SpawnpointObject sp)
    {
        //calc aim offesets        
        Transform target = levelController.GetTarget();
        Debug.Log($"ProjectileController: {sp.transform.name} to {target.name}");
        float offset = Random.Range(-0.25f, 0.25f);
        float x = target.position.x + offset;

        x += sp.transform.position.x < 0 ? 2 : -2;

        offset = Random.Range(-0.25f, 0.25f);
        float y = target.position.y + offset;

        y += sp.transform.position.y < 0 ? 2 : -2;

        //get target pos
        Vector3 targetPos = new Vector3(x, y, 0);

        DifficultyDataClass data = difficultyController.GetCurrentDifficulty;

        sp.SetAimInfo(data.AimLaserSize, data.AimLaserColor, targetPos);

        float alpha = 0;

        while (alpha < 1)
        {
            alpha += (Time.deltaTime * data.AimTime);
            sp.UpdateLaserAlpha(alpha);
            yield return null;
        }

        sp.SetShootInfo(data.ShotLaserSize, data.ShotLaserColor);
        yield return new WaitForSeconds(data.ShotTime);

        sp.UpdateLaserAlpha(0);

        ProjectileObject p = GetProjectile();
        p.Launch(sp.transform, data.ProjectileSpeed);

        shootEffectSource.PlayOneShot(data.ShootClip);

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

    private int GetRandom(int max, int previus)
    {
        int r = Random.Range(0, max);
        if (r == previus)
        {
            r = r == 0 ? r++ : r--;
        }
        return r;
    }

}

