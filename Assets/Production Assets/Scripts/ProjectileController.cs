using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] GameController gameController;

    [SerializeField] private List<Transform> spawnPointList;
    [SerializeField] bool allowNew;
    [SerializeField] private List<Rigidbody2D> projectilesList;    
    [SerializeField] private List<TargetObject> targetsList;
    [SerializeField] float launchSpeed;
    [SerializeField] float aimdelay;
    [SerializeField] float shootdelay;
    [SerializeField] List<int> levelInfo;

    private int shoots;
    private int pt = 0;
    private int index = 0;

    private AudioSource shootEffect;

    void Start()
    {        
        foreach (Rigidbody2D rb in projectilesList)
        {
            rb.gameObject.SetActive(false);
        }
        shootEffect = GetComponent<AudioSource>();
        gameController.SetLifes(targetsList.Count);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            levelInfo.Shuffle();
            SpawnEnemy(levelInfo[0]);
        }
    }

    public void FindEnemies(int a)
    {     
        if (a >= levelInfo.Count)
        {
            gameController.Winner();
            return;
        }

        int amount = levelInfo[a];
        SpawnEnemy(amount);
    }

    public void SpawnEnemy(int a)
    {        
        IEnumerator s = IESpawnEnemies(a);
        StartCoroutine(s);
    }

    IEnumerator IESpawnEnemies(int a)
    {     
        List<Transform> activeSpawn = new List<Transform>(spawnPointList);
        activeSpawn.Shuffle();
        shoots = 0;
        for (int i = 0; i < a; i++)
        {
            if (activeSpawn.Count == 0)
            {
                activeSpawn = new List<Transform>(spawnPointList);
                activeSpawn.Shuffle();
            }

            activeSpawn.Shuffle();

            Transform spawn = activeSpawn[0];
            activeSpawn.RemoveAt(0);

            if (targetsList.Count == 0)
            {
                yield break;
            }

            IEnumerator s = IESpawnEnemy(spawn);
            StartCoroutine(s);

            yield return new WaitForSeconds((shootdelay * 1.5f));
        }

        while (shoots < a)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        gameController.RoundOver();
    }

    IEnumerator IESpawnEnemy(Transform spawnPoint)
    {
        targetsList.Shuffle();
        if (targetsList[0].Dead)
        {
            targetsList.RemoveAt(0);
            targetsList.Shuffle();
        }
        if (targetsList.Count == 0)
        {
            //no more buildings
            yield break;
        }
        Transform target = targetsList[0].transform;
        float offset = Random.Range(-0.25f, 0.25f);
        float x = target.position.x + offset;

        x += spawnPoint.position.x < 0 ? 2 : -2;

        offset = Random.Range(-0.25f, 0.25f);
        float y = target.position.y + offset;

        y += spawnPoint.position.y < 0 ? 2 : -2;

        Vector3 targetPos = new Vector3(x, y, 0);
        pt = index;

        LineRenderer lr = spawnPoint.GetComponent<LineRenderer>();
        Color c = lr.startColor;
        c = new Color(c.r, c.g, c.b, 0);
        lr.startColor = c;
        lr.endColor = c;

        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;

        lr.SetPosition(0, spawnPoint.position);
        lr.SetPosition(1, targetPos);

        float alpha = 0;

        while (alpha < 1)
        {
            alpha += (Time.deltaTime * aimdelay);
            c = new Color(c.r, c.g, c.b, alpha);
            lr.startColor = c;
            lr.endColor = c;
            yield return null;
        }

        lr.startWidth = 0.075f;
        lr.endWidth = 0.075f;
        yield return new WaitForSeconds(shootdelay / 2);

        c = new Color(c.r, c.g, c.b, 0);
        lr.startColor = c;
        lr.endColor = c;

        Vector2 direction = targetPos - spawnPoint.position;

        spawnPoint.transform.up = direction;

        Rigidbody2D enemy = PoolEnemy();
        enemy.transform.position = spawnPoint.position;
        enemy.gameObject.SetActive(true);
        enemy.velocity = spawnPoint.up * launchSpeed;
        shootEffect.PlayOneShot(shootEffect.clip);
        shoots++;
    }

    private Rigidbody2D PoolEnemy()
    {
        foreach (Rigidbody2D p in projectilesList)
        {
            if (!p.gameObject.activeInHierarchy)
            {
                return p;
            }
        }
        if (!allowNew)
        {
            return null;
        }

        Rigidbody2D np = Instantiate(projectilesList[0], projectilesList[0].transform.parent);
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

