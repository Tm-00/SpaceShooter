using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float Health = 50f;

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if(Health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        enemyAI enemy = GetComponent<enemyAI>();
        if(enemy != null)
        {
            enemy.killEnemy();
        }

        ScoreManager.scoreCount += 1;
        Destroy(gameObject);
        
    }
}
