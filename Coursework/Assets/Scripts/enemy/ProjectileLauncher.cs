using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var healthComponent = other.GetComponent<PlayerHealth>();
            if (healthComponent != null )
            {
                healthComponent.TakeDamage(5);
            }
        }
    }


}
