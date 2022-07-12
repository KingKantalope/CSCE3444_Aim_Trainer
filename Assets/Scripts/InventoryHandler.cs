using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject[] weapons = new GameObject[4]; // weapon1 = 0, weapon2 = 1, sidearm = 2, backup = 3
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
    [SerializeField] private GameObject fragObject;
    [SerializeField] private GameObject moverObject;
    [SerializeField] private GameObject causticObject;
    [SerializeField] private int reserveFrag;
    [SerializeField] private int reserveMover;
    [SerializeField] private int reserveCaustic;
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
            if (weapons[2] || (weapons[0] && weapons[1]))
                canSwap = true;
        }

        Debug.Log("canSwap: " + canSwap);

        // find available grenades in inventory
        if (reserveFrag > 0)
            activeGrenade = 0;
        else if (reserveMover > 0)
            activeGrenade = 1;
        else if (reserveCaustic > 0)
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
        // swap between weapons 1 & 2 when button is tapped
        // swap to/from sidearm when button is held
        if (isSwapping) // if the player is holding down the swap button
        {
            // add to hold time if less than the minimum required to swap to sidearm
            if (currentSwapTime < minimumSwapTime)
                currentSwapTime += Time.deltaTime;
            else // if it is greater, end swapping and swap to/from sidearm
            {
                isSwapping = false;

                if (activeWeapon == 2) // is sidearm
                    nextWeapon = 0; // to weapon 1
                else
                    nextWeapon = 2; // to sidearm

                SwapWeapon(); // perform weapon swap
            }
        }
        else if (currentSwapTime > 0.0f) // let go of swap button early
        {
            if (activeWeapon == 0) // is weapon 1
                nextWeapon = 1; // to weapon 2
            else
                nextWeapon = 0; // to weapon 1

            SwapWeapon(); // perform weapon swap
        }

        // reload or use
        
    }

    // Start firing active Weapon
    public void StartFireWeapon()
    {
        weapons[activeWeapon].GetComponent<Gun>().StartFiring();
    }

    // Stop firing active weapon
    public void StopFireWeapon()
    {
        weapons[activeWeapon].GetComponent<Gun>().StopFiring();
    }

    // start alt-firing active weapon
    public void StartAltFireWeapon(bool isToggle)
    {
        weapons[activeWeapon].GetComponent<Gun>().StartAltFiring(isToggle);
    }

    // stop alt-firing active weapon
    public void StopAltFireWeapon(bool isToggle)
    {
        weapons[activeWeapon].GetComponent<Gun>().StopAltFiring(isToggle);
    }

    // stop alt-firing active weapon
    private void ReloadWeapon()
    {
        weapons[activeWeapon].GetComponent<Gun>().Reload();
    }

    // start hold reload
    public void StartReload()
    {
        isHoldingReload = true;
    }

    // start hold reload
    public void StopReload()
    {
        isHoldingReload = false;
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
        {
            nextWeapon = 2;
            SwapWeapon();
        }
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
        // check the other two grenade types in order for available
        switch (activeGrenade)
        {
            case 0:
                if (reserveMover > 0) // next grenade
                    activeGrenade = 1;
                else if (reserveCaustic > 0) // next, next grenade
                    activeGrenade = 2;
                else if (reserveFrag == 0) // no grenades
                    activeGrenade = 3;
                break;
            case 1:
                if (reserveCaustic > 0) // next grenade
                    activeGrenade = 2;
                else if (reserveFrag > 0) // next, next grenade
                    activeGrenade = 0;
                else if (reserveMover == 0) // no grenades
                    activeGrenade = 3;
                break;
            case 2:
                if (reserveFrag > 0) // next grenade
                    activeGrenade = 0;
                else if (reserveMover > 0) // next, next grenade
                    activeGrenade = 1;
                else if (reserveCaustic == 0) // no grenades
                    activeGrenade = 3;
                break;
            default:
                if (reserveFrag > 0) // frag grenade
                    activeGrenade = 0;
                else if (reserveMover > 0) // mover charge
                    activeGrenade = 1;
                else if (reserveCaustic == 0) // caustic canister
                    activeGrenade = 2;
                else
                    activeGrenade = 3; // no grenades
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
