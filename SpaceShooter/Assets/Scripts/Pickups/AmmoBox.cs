using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 20;
    public float addAmmoCD = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gun gun = other.GetComponent<Gun>();
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                //Destroy(gameObject);
            }
        }
    }
}
