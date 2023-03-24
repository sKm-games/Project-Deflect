using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_script : MonoBehaviour
{
    private List<Transform> spawnPoints;
    private List<Rigidbody2D> enemies;
    [SerializeField] bool allowNew;
    [SerializeField] Transform buildingHolder;
    private List<Building_script> buildings;
    [SerializeField] float launchSpeed;
    [SerializeField] float aimdelay;
    [SerializeField] float shootdelay;
    [SerializeField] List<int> enemyCount;
    [SerializeField] GameController_script gameController;
    int shoots;

    int pt = 0;
    int index = 0;

    AudioSource shootEffect;
    
    void Start()
    {
        spawnPoints = new List<Transform>(this.transform.GetChild(0).GetComponentsInChildren<Transform>());
        spawnPoints.RemoveAt(0);
        enemies = new List<Rigidbody2D>(this.transform.GetChild(1).GetComponentsInChildren<Rigidbody2D>());
        foreach (Rigidbody2D rb in enemies)
        {
            rb.gameObject.SetActive(false);
        }

        buildings = new List<Building_script>(buildingHolder.GetComponentsInChildren<Building_script>());
        shootEffect = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            enemyCount.Shuffle();
            SpawnEnemy(enemyCount[0]);
        }
    }

    public void FindEnemies(int a)
    {
        if (a >= enemyCount.Count)
        {
            gameController.Winner();
            return;
        }

        int amount = enemyCount[a];
        SpawnEnemy(amount);

    }

    public void SpawnEnemy(int a)
    {
        IEnumerator s = IESpawnEnemies(a);
        StartCoroutine(s);
    }    
    
    IEnumerator IESpawnEnemies(int a)
    {
        List<Transform> activeSpawn = new List<Transform>(spawnPoints);
        activeSpawn.Shuffle();
        shoots = 0;
        for (int i = 0; i < a; i++)
        {
            if (activeSpawn.Count == 0)
            {
                activeSpawn = new List<Transform>(spawnPoints);
                activeSpawn.Shuffle();
            }

            activeSpawn.Shuffle();

            Transform spawn = activeSpawn[0];
            activeSpawn.RemoveAt(0);

            if (buildings.Count == 0)
            {
                yield break;
            }

            IEnumerator s = IESpawnEnemy(spawn);            
            StartCoroutine(s);
            
            yield return new WaitForSeconds((shootdelay*1.5f));
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
        buildings.Shuffle();
        if (buildings[0].Dead)
        {
            buildings.RemoveAt(0);
            buildings.Shuffle();
        }
        if (buildings.Count == 0)
        {
            //no more buildings
            yield break;
        }
        Transform target = buildings[0].transform;
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
        //float r = Random.Range((aimdelay / 2), (aimdelay * 2));
        while (alpha < 1)
        {
            //alpha += (Time.deltaTime * r);
            alpha += (Time.deltaTime * aimdelay);
            c = new Color(c.r, c.g, c.b, alpha);
            lr.startColor = c;
            lr.endColor = c;
            yield return null;
        }

        lr.startWidth = 0.075f;
        lr.endWidth = 0.075f;
        yield return new WaitForSeconds(shootdelay/2);

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
        foreach (Rigidbody2D e in enemies)
        {
            if (!e.gameObject.activeInHierarchy)
            {
                return e;
            }
        }
        if (!allowNew)
        {
            return null;
        }

        Rigidbody2D ne = Instantiate(enemies[0], enemies[0].transform.parent);
        enemies.Add(ne);

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
}
