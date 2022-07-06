using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimInterpreter : MonoBehaviour
{
    private PlayerControls controls;

    public float aimSensX = 100.0f;
    public float aimSensY = 75.0f;
    public float aimMaxGamepad = 2.0f;
    public float aimAccelGamepad = 1.0f;
    private Vector2 lookInputMouse;
    private Vector2 lookInputGamepad;
    public float aimAccelModGamepad = 1.0f;
    private float xRotation = 0.0f;
    private bool gamepadInput = false;

    public Transform playerBody;
    public Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        controls = new PlayerControls();

        // Update script on what direction and magnitude to change aim angle
        // mouse
        controls.FirstPersonCamera.MouseLook.performed += ctx => lookInputMouse = ctx.ReadValue<Vector2>();
        controls.FirstPersonCamera.MouseLook.canceled += ctx => lookInputMouse = Vector2.zero;

        // gamepad
        controls.FirstPersonCamera.GamepadLook.performed += ctx =>
        {
            lookInputGamepad = ctx.ReadValue<Vector2>();
            gamepadInput = true;
        };
        controls.FirstPersonCamera.GamepadLook.canceled += ctx =>
        {
            lookInputGamepad = Vector2.zero;
            gamepadInput = false;
        };
    }

    private void OnEnable()
    {
        controls.FirstPersonCamera.Enable();
    }

    private void OnDisable()
    {
        controls.FirstPersonCamera.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // mouse aim
        Vector2 aimMouse = new Vector2(lookInputMouse.x * aimSensX, lookInputMouse.y * aimSensY) * Time.fixedDeltaTime;
        xRotation -= aimMouse.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerBody.Rotate(Vector3.up * aimMouse.x);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // gamepad acceleration
        if (gamepadInput)
        {
            aimAccelModGamepad += (((aimMaxGamepad - 1.0f) * Time.fixedDeltaTime) * aimAccelGamepad);
            float max = 1 + (lookInputGamepad.magnitude * (aimMaxGamepad - 1));
            aimAccelModGamepad = Mathf.Clamp(aimAccelModGamepad, 1.0f, max);
        }
        else aimAccelModGamepad = 1.0f;

        // gamepad aim
        Vector2 aimGamepad = new Vector2(lookInputGamepad.x * aimSensX, lookInputGamepad.y * aimSensY) * aimAccelModGamepad * Time.fixedDeltaTime;
        xRotation -= aimGamepad.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerBody.Rotate(Vector3.up * aimGamepad.x);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
