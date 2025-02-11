using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //calls the inputmanager 
    [SerializeField]
    //private InputManager inputManager;


    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;


    // variable for fire rate
    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    // variables for gun damage 
    public float damage = 10f;


    // variables for gun range
    public float range = 100f;


    // max ammunition and current amount of ammo in magazine
    public int maxMagAmmo = 10;
    private int currentMagAmmo;


    // max ammo the player is carrying and current amount
    public int maxAmmo = 30;
    private int currentMaxAmmo;


    // delay before player can shoot again
    private float reloadTime = 1f;


    // bool function made so that the gun reloads on when it needs to be
    private bool isReloading;


    // references the camera needed for raycast
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        // references the input manager at the start of the game
        //inputManager = GetComponent<InputManager>();

        // set the current magazine of the gun to max 
        currentMagAmmo = maxMagAmmo;

        // set the max ammo held 
        currentMaxAmmo = maxAmmo;

        
    }

    // makes sure the weapon isn't caught in a reloading loop
    private void OnEnable()
    {
        isReloading = false;
    }



    // Update is called once per frame
    void Update()
    {
        ScoreManager.ammoCount = currentMagAmmo;
        ScoreManager.maxAmmoCount = maxAmmo;
        // won't call other functions if the gun is reloading
        if (isReloading)
            return;

        // checks if the weapons has run out of ammo
        if (maxAmmo != 0)
        {
            if (currentMagAmmo <= 0)
            {
                //calls the reload coroutine
                StartCoroutine(Reload());
                return;
            } 
            else if (Input.GetKeyDown(reloadKey)) 
            {
                //calls the reload coroutine if r key is pressed
                StartCoroutine(Reload());
                return;
            }
        }


        //checks if the player has left clicked
        if (Input.GetKey(fireKey) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/fireRate;

            if (currentMagAmmo != 0){
                Shoot();
            }
            else
            {
                return;
            }
   
        }
    }

    // allows other functions to be paused so that this can be executed
    // coroutine that lets the user reload the gun
    IEnumerator Reload()
    {
        //sets isreloading to true will stop the gun from doing anything else aside from reloading
        isReloading = true;
        Debug.Log("reloading");

        animator.SetBool("Reloading", true);

        // pauses other fucntions for "reloadtime" amount of seconds
        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        //resets the gun to have max ammo
        currentMagAmmo = maxMagAmmo;
        maxAmmo -= maxMagAmmo;
        ScoreManager.maxAmmoCount = maxAmmo;
        Debug.Log(maxAmmo);


        //sets isreloading to false so the gun can resume normal usage
        isReloading = false;
    }

    public void AddAmmo(int ammoAmount)
    {
        maxAmmo = Mathf.Min(maxAmmo + ammoAmount);
        Debug.Log("NEW MAX AMMO = " + maxAmmo);
    }

    void Shoot()
    {
        muzzleFlash.Play();
        currentMagAmmo--;

        Debug.Log(currentMagAmmo);
        ScoreManager.ammoCount = currentMagAmmo;


        //calls the raycast function and assigns it to "hit"
        RaycastHit hit;


        //function to spawn a raycast from the camera then check if it is "hitting" a specific object
        //if it is then perform the function
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            Debug.Log(hit.transform.name);


            Target target = hit.transform.GetComponent<Target>();


            if (target != null )
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}


