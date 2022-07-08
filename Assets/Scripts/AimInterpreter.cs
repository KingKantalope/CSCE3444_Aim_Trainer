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
        moveAim(lookInputMouse);

        // gamepad acceleration
        if (gamepadInput)
        {
            // get modifier relative to input magnitude
            aimAccelModGamepad += (((aimMaxGamepad - 1.0f) * Time.fixedDeltaTime) * aimAccelGamepad); // accelerate aim
            float max = 1 + (lookInputGamepad.magnitude * (aimMaxGamepad - 1)); // get upper accel limit
            aimAccelModGamepad = Mathf.Clamp(aimAccelModGamepad, 1.0f, max); // clamp acceleration between 1 and said max
        }
        else aimAccelModGamepad = 1.0f; // reset look acceleration of gamepad

        // gamepad aim with acceleration modifier
        moveAim(lookInputGamepad * aimAccelModGamepad);
    }

    private void moveAim(Vector2 input)
    {
        Vector2 aim = new Vector2(input.x * aimSensX, input.y * aimSensY) * Time.fixedDeltaTime; // get amount to move for next frame
        xRotation -= aim.y; // apply up/down camera rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // don't let player look beyond straight down and straight up
        playerBody.Rotate(Vector3.up * aim.x); // apply left/right player rotation
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // clamp up/down rotation from going to high or low
    }
}
