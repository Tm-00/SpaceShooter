using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class waveContent
    {
       [SerializeField][NonReorderable] GameObject[] monsterSpawner;

        public GameObject[] GetMonsterSpawnList()
        {
            return monsterSpawner;
        }
    }

    [SerializeField][NonReorderable]
    waveContent[] waves;

    int currentWave = 0;
    public float spawnRange = 10;
    public List<GameObject> currentMonster;




    // Start is called before the first frame update
    void Start()
    {
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave < waves.Length && currentMonster.Count == 0)
        {
            currentWave++;
            SpawnWave();
        }
    }


    void SpawnWave()
    {
        if (currentWave < waves.Length && waves[currentWave].GetMonsterSpawnList() != null)
        {
            // checks what wave is supposed to spawn checks how many mobs that list indicates to spawn then does so
            foreach (GameObject monsterPrefab in waves[currentWave].GetMonsterSpawnList())
            {
                GameObject newSpawn = Instantiate(monsterPrefab, FindSpawnLocation(), Quaternion.identity);
                currentMonster.Add(newSpawn);

                enemyAI monster = newSpawn.GetComponent<enemyAI>();
                monster.SetSpawner(this);
            }
        }
        else
        {
            Debug.Log("Wave index out of bounds or monster spawn list is null!");
            return;
        }
    }

    Vector3 FindSpawnLocation()
    {
        Vector3 Spawnpoint;
        

        float xLoc = Random.Range(-spawnRange, spawnRange) + transform.position.x;
        float zLoc = Random.Range(-spawnRange, spawnRange) + transform.position.z;
        float yLoc = transform.position.y;

        Spawnpoint = new Vector3 (xLoc, yLoc, zLoc);

        if(Physics.Raycast(Spawnpoint, Vector3.down,10))
        {
            return Spawnpoint;
        }
        else
        {
            return FindSpawnLocation();
        }
    }
}
