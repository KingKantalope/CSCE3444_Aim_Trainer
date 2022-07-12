using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    // references
    private Transform firePoint;
    private InventoryHandler inventory;

    [Header("Fire Action")]
    [SerializeField] private float fireRateBase;
    [SerializeField] private bool isFullAuto;
    [SerializeField] private GameObject projectile;
    private bool canFire;
    private bool isTriggered;
    private bool isTriggerReset;

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

    public virtual void StartFiring()
    {
        // let gun know that it should fire when it next can
        isTriggered = true;
        // fire if gun is ready and has ammo
        if (canFire)
            Fire();
    }

    public virtual void StopFiring()
    {
        // let gun know it shouldn't fire
        isTriggered = false;
        isTriggerReset = true;
    }

    protected virtual void Fire()
    {
        // only fire if ammo is available
        if (magCurrent > 0)
        {
            // spawn projectile in correct orientation
            Instantiate(projectile, firePoint);
            // provent gun from firing immediately
            canFire = false;
            if (!isFullAuto)
                isTriggerReset = false;
            // start fire delay
            Invoke("ReadyToFire", fireRateBase);
        }
        else
        {
            // stuff to notify empty mag
        }
    }

    protected virtual void ReadytoFire()
    {
        // let gun be able to fire
        canFire = true;
        // fire if the trigger is pulled
        if (isTriggered && isTriggerReset)
            Fire();
    }

    public virtual void StartAltFiring()
    {

    }

    public virtual void StopAltFiring()
    {

    }

    public virtual void Reload()
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

    protected virtual void AddAmmo()
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
