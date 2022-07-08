using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // references
    private Transform firePoint;
    private InventoryHandler inventory;
    [Header("Name and Info")]
    [SerializeField] private string weaponName = "Dingus";
    [SerializeField] private bool isSidearm = false;

    [Header("Fire Action")]
    [SerializeField] private float fireRateBase;
    [SerializeField] private bool isFullAuto;
    [SerializeField] private float fireRateBurst;
    [SerializeField] private int firePerBurst;
    [SerializeField] private GameObject projectile;
    private bool canFire;

    [Header("Ammo")]
    [SerializeField] private int resMax;
    [SerializeField] private int resCurrent;
    [SerializeField] private int magMax;
    private int magCurrent;
    private int resToRemove;

    [Header("Reload")]
    [SerializeField] private float reloadFullAddTime;
    [SerializeField] private float reloadTacticalAddTime;

    [Header("Equip and Stow")]
    [SerializeField] private float stowTime;
    [SerializeField] private float equipTime;

    [Header("Grenades and Horseshoes")]
    [SerializeField] private float throwAndMeleeTime;

    private void Awake()
    {
        inventory = GetComponentInParent<InventoryHandler>(); // get inventory
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFiring()
    {
        if (canFire)
        {

        }
    }

    public void StopFiring()
    {

    }

    public void StartAltFiring()
    {

    }

    public void StopAltFiring()
    {

    }

    public void Reload()
    {
        // check if there is ammo to add
        // otherwise play little animation
        if (resCurrent > 0)
        {
            // can't fire
            canFire = false;

            // ammo to move from reserves to magazine
            resToRemove = magMax - magCurrent;

            // is reload full or tactical
            if (resToRemove == magMax)// full
            {
                // call delay with reloadFullAddTime for addTime
                Invoke("AddAmmo", reloadFullAddTime);
                // have animation ending be trigger for canFire again
            }
            else // tactical
            {
                // call delay with reloadTacticalAddTime
                Invoke("AddAmmo", reloadTacticalAddTime);
                // have animation ending be trigger for canFire again
            }
        }
        else
        {

        }
    }

    private void AddAmmo()
    {
        // add ammo to mag & remove ammo from reserves
        if (resCurrent < resToRemove)
        {
            magCurrent += resCurrent;
            resCurrent = 0;
        }
        else
        {
            magCurrent = magMax;
            resCurrent -= resToRemove;
        }
    }

    private void ReadytoFire()
    {

    }

    public void Equip()
    {
        // reset all non-fire invocations
        CancelInvoke("AddAmmo");
        CancelInvoke("ReadyToFire");
        CancelInvoke("FinalizeStow");

        // play animation and invoke ReadyToFire()
        Invoke("ReadyToFire", equipTime);
    }

    public void Stow()
    {
        // cancel reloading invocations
        CancelInvoke("AddAmmo");
        CancelInvoke("ReadyToFire");

        // play stow animation and invoke weapon swap in inventory
        Invoke("FinalizeStow",stowTime);
    }

    private void FinalizeStow()
    {
        // call swap function in inventoryHandler
    }

    public void ThrowAnimation()
    {
        // cancel reload invocations
        CancelInvoke("AddAmmo");
        CancelInvoke("ReadyToFire");

        // play animation and invoke ReadyToFire()
        Invoke("ReadyToFire",throwAndMeleeTime);
    }

    public void MeleeAnimation()
    {
        // cancel reload invocations
        CancelInvoke("AddAmmo");
        CancelInvoke("ReadyToFire");

        // play animation and invoke ReadyToFire()
        Invoke("ReadyToFire", throwAndMeleeTime);
    }
}
