using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRes : MonoBehaviour
{
    // allows us to see the variable in the editor 
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private PlayerMovement pm;
    [SerializeField]
    private Manager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.transform.position = respawnPoint.transform.position;
            Physics.SyncTransforms();
            pm.enabled = false;
            FindObjectOfType<Manager>().EndGame();
        }
    }
}
