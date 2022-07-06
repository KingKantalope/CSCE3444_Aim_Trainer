using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject[] weapons = new GameObject[3]; // weapon1 = 0, weapon2 = 1, sidearm = 2
    [SerializeField] private GameObject weapon1 = null;
    [SerializeField] private GameObject weapon2 = null;
    [SerializeField] private GameObject sidearm = null;
    [SerializeField] private GameObject backUpWeapon = null;
    private int activeWeapon;
    private int nextWeapon;

    // weapon swapping variables
    private bool canSwap = false;
    private bool isSwapping = false;
    private float currentSwapTime = 0.0f;
    private float minimumSwapTime = 0.5f;

    // reload or interact
    private bool isHoldingReload = false;

    [Header("Grenades")]
    [SerializeField] private GameObject[] grenades = new GameObject[3]; // frag = 0, mover = 1, caustic = 2
    [SerializeField] private GameObject frag;
    [SerializeField] private GameObject mover;
    [SerializeField] private GameObject caustic;
    [SerializeField] private int resFrag;
    [SerializeField] private int resMover;
    [SerializeField] private int resCaustic;
    private int activeGrenade;

    [Header("Equipment")]
    [SerializeField] private GameObject equipment = null;
    private bool hasEquipment = false;

    // Start is called before the first frame update
    void Start()
    {
        
        // find weapon in inventory
        if (weapons[0])
            activeWeapon = 0;
        else if (weapons[1])
            activeWeapon = 1;
        else if (weapons[2])
            activeWeapon = 2;
        else
            activeWeapon = 3; // player's chosen default weapon, is melee only

        Debug.Log("activeWeapon: " + activeWeapon);

        // attach activeWeapon to player arms and call Weapon.Equip()

        // determine if canSwap
        if (activeWeapon < 3)
        {
            if (sidearm || (weapon1 && weapon2))
                canSwap = true;
        }

        Debug.Log("canSwap: " + canSwap);

        // find available grenades in inventory
        if (resFrag > 0)
            activeGrenade = 0;
        else if (resMover > 0)
            activeGrenade = 1;
        else if (resCaustic > 0)
            activeGrenade = 2;
        else
            activeGrenade = 3; // no selected grenade

        Debug.Log("activeGrenade: " + activeGrenade);

        // find equipment
        if (equipment)
            hasEquipment = true;
        else
            hasEquipment = false; // no equipment held

        Debug.Log("hasEquipment: " + hasEquipment);
    }

    // Update is called once per frame
    void Update()
    {
        // weapon swapping
        if (isSwapping)
        {
            if (currentSwapTime < minimumSwapTime)
                currentSwapTime += Time.deltaTime;
            else
            {
                isSwapping = false;

                if (activeWeapon == 2)
                    nextWeapon = 0;
                else
                    nextWeapon = 2;

                SwapWeapon();
            }
        }
        else if (currentSwapTime > 0.0f)
        {
            if (activeWeapon == 0)
                nextWeapon = 1;
            else
                nextWeapon = 0;

            SwapWeapon();
        }

        // reload or use
        
    }

    // Start firing active Weapon
    public void StartFireWeapon()
    {
        weapons[activeWeapon].GetComponent<Weapon>().StartFiring();
    }

    // Stop firing active weapon
    public void StopFireWeapon()
    {
        weapons[activeWeapon].GetComponent<Weapon>().StopFiring();
    }

    // start alt-firing active weapon
    public void StartAltFireWeapon()
    {
        weapons[activeWeapon].GetComponent<Weapon>().StartAltFiring();
    }

    // stop alt-firing active weapon
    public void StopAltFireWeapon()
    {
        weapons[activeWeapon].GetComponent<Weapon>().StopAltFiring();
    }

    // stop alt-firing active weapon
    private void ReloadWeapon()
    {
        weapons[activeWeapon].GetComponent<Weapon>().Reload();
    }

    // start hold reload
    public void StartReload()
    {

    }

    // start hold reload
    public void StopReload()
    {

    }

    // start swapping weapon input
    public void StartSwappingWeapon()
    {
        isSwapping = true;
    }

    // stop swapping weapon input
    public void StopSwappingWeapon()
    {
        isSwapping = false;
    }

    // swap to sidearm
    public void SwapToSidearm()
    {
        if (activeWeapon != 2)
            nextWeapon = 2;
    }
    private void SwapWeapon()
    {

    }

    public void DropWeapon()
    {

    }

    public void PickUpWeapon()
    {

    }

    // 
    public void ThrowGrenade()
    {

    }

    // switch to the next grenade or do nothing
    public void NextGrenade()
    {
        switch (activeGrenade)
        {
            case 1:
                if (resMover > 0)
                    activeGrenade = 2;
                else if (resCaustic > 0)
                    activeGrenade = 3;
                break;
            case 2:
                if (resCaustic > 0)
                    activeGrenade = 3;
                else if (resFrag > 0)
                    activeGrenade = 1;
                break;
            case 3:
                if (resFrag > 0)
                    activeGrenade = 1;
                else if (resMover > 0)
                    activeGrenade = 2;
                break;
        }
    }

    // throw equipment and remove reference in inventory
    public void ThrowEquipment()
    {
        // throw equipment

        // set equipment to null
        equipment = null;

        // set hasEquipment to false
        hasEquipment = false;
    }

    // pick up equipment
    public void PickUpEquipment()
    {

    }

    // drop equipment
    public void DropEquipment()
    {

    }
}
