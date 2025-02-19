using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour
{
    public int healAmount = 25;
    public float healCD = 1f;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            var healPlayer = other.GetComponent<PlayerHealth>();
            if (healPlayer != null)
            {
                healPlayer.Heal(healAmount);
                //Destroy(gameObject);
            }
        }
    }
}
