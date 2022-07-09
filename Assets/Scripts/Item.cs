using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Basic Identifiers")]
    [SerializeField] private new string name;
    [SerializeField] private GameObject model;
    [SerializeField] private int id;
    [SerializeField] private ItemType type;

    public virtual void PickUp()
    {

    }

    public virtual void Dropped()
    {

    }
}

// describes the types of items that can exist
// 
// NormalWeapon: weapon goes into either of the normal slots
// Sidearm: weapon goes into sidearm slot
// TempWeapon: weapon does not occupy slot, like a turret or enemy drop
// Equipment: goes into equipment slot
// Grenade: adds to grenade reserves, not actually stored
// Quest: adds to quest item list
// Interactable: environmental buttons and such, because of reload vs interact
public enum ItemType
{
    NormalWeapon,
    Sidearm,
    TempWeapon,
    Equipment,
    Grenade,
    Quest,
    Interactable
}