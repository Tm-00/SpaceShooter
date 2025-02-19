using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public PlayerMovement pm;



    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        ScoreManager.healthCount = currentHP;
    }
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if(currentHP <= 0)
        {
            if(pm != null)
            {

                pm.enabled = false;

            }
            FindObjectOfType<Manager>().EndGame();
        }
    }


    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        } if (currentHP <= 1)
        {
            currentHP += healAmount;
        }
    
    }
}
