using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PhysicsHandler movePhysics;
    private AimInterpreter aimInt;
    private PlayerControls controls;
    private InventoryHandler inventory;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movePhysics = GetComponent<PhysicsHandler>();
        aimInt = GetComponent<AimInterpreter>();
    }

    private void Awake()
    {
        controls = new PlayerControls();

        // Update script on what direction and magnitude to move
        controls.FirstPersonStuff.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.FirstPersonStuff.Move.canceled += ctx => moveInput = Vector2.zero;

        // Update script on whether the player wants to fire
        controls.FirstPersonStuff.Shoot.performed += ctx => inventory.StartFireWeapon();
        controls.FirstPersonStuff.Shoot.canceled += ctx => inventory.StopFireWeapon();

        // Update script on whether the player wants to alt-fire
        controls.FirstPersonStuff.Aim.performed += ctx => inventory.StartAltFireWeapon();
        controls.FirstPersonStuff.Aim.canceled += ctx => inventory.StopAltFireWeapon();

        // Update script on whether the player wants to use equipment
        controls.FirstPersonStuff.Reload.performed += ctx => inventory.StartReload();
        controls.FirstPersonStuff.Reload.canceled += ctx => inventory.StopReload();

        // Update script on whether the player wants to swap weapons
        controls.FirstPersonStuff.SwapWeapon.performed += ctx => inventory.StartSwappingWeapon();
        controls.FirstPersonStuff.SwapWeapon.canceled += ctx => inventory.StopSwappingWeapon();

        // Update script on whether the player wants to use equipment
        controls.FirstPersonStuff.SwapSidearm.performed += ctx => inventory.SwapToSidearm();

        // Update script on whether the player wants to use equipment
        controls.FirstPersonStuff.NextGrenade.performed += ctx => inventory.NextGrenade();

        // Update script on whether the player wants to use equipment
        controls.FirstPersonStuff.ThrowGrenade.performed += ctx => inventory.ThrowGrenade();

        // Update script on whether the player wants to use equipment
        controls.FirstPersonStuff.Equipment.performed += ctx => inventory.ThrowEquipment();

        // Update script on whether the player wants to sprint
        controls.FirstPersonStuff.Jump.performed += ctx => movePhysics.StartJump();
        controls.FirstPersonStuff.Jump.canceled += ctx => movePhysics.StopJump();
    }

    private void OnEnable()
    {
        controls.FirstPersonStuff.Enable();
    }

    private void OnDisable()
    {
        controls.FirstPersonStuff.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
            movePhysics.LateralMove(moveInput);
    }
}
