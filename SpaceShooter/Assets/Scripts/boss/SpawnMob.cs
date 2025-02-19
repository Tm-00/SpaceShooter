using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnMob : MonoBehaviour
{
    //will allow us to refer to a gameobject in the editor
    public GameObject enemy;
    //variable to store x position
    public int xPos;
    //variable to store z position
    public int zPos;
    //variable to indicate how many mobs we want to spawn
    public int enemyCount;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(enemySpawn());
    }

    //function to spawn the mobs
    IEnumerator enemySpawn()
    {
        //will iterate through the loop until "< x" amount of mobs have spawned
        while (enemyCount < 10)
        {
            // chooses a random number between the given values 
            xPos = Random.Range(-35, -28);
            zPos = Random.Range(30, 0);
            // spawns the mobs at the given vector 3 position 
            Instantiate(enemy, new Vector3(xPos, 16, zPos), Quaternion.identity);
            // will wait 0.1 seconds before spawning each mob
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

}
