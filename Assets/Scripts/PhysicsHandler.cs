using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    // component reference
    private Rigidbody rb;
    private Rigidbody owner;

    [Header("Ground Reference")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.8f;
    [SerializeField] private LayerMask groundMask;
    private GameObject ground;
    private float groundVelocity;

    [Header("Jump and Gravity")]
    [SerializeField] private float defaultGravityMagnitude = 16f;
    [SerializeField] private float jumpHeight = 4.5f;
    [SerializeField] private Vector3 defaultGravityDirection = Vector3.down;
    [SerializeField] private Quaternion defaultOrientation = Quaternion.identity;
    private bool isJumping = false;
    private bool canJump = true;
    private float jumpVelocity;
    private bool isGrounded;

    [Header("Lateral Movement")]
    [SerializeField] private float maxMoveSpeed = 8.0f;
    [SerializeField] private float groundTimeToMaxMove = 0.1f;
    [SerializeField] private float airTimeToMaxMove = 2.0f;
    [SerializeField] private int defaultWeight = -1;
    private Vector3 velocityLocal;
    private Vector3 velocityLatWorld;
    private Vector3 velocityVertWorld;
    private float groundAccel;
    private float airAccel;
    private bool isMoving;

    [Header("Friction")]
    [SerializeField] private float defaultFriction = 1.0f;
    private float surfaceMultiplier;

    [Header("PhysicsVolume Stuff")]
    [SerializeField] private float maxAngularVelocity;
    [SerializeField] private int weightThreshold;
    private List<PhysicsVolume> PhysicsVolumes = new List<PhysicsVolume>();
    private bool isMagnetized = false;

    private void Awake()
    {
        groundAccel = maxMoveSpeed / groundTimeToMaxMove;
        airAccel = maxMoveSpeed / airTimeToMaxMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpVelocity = Mathf.Sqrt(2 * jumpHeight * defaultGravityMagnitude);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // check if grounded UPDATE TO ALSO CHECK FOR PROPS/PHYSICS
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // get upward velocity of object stood on


        // canJump
        if (isGrounded)
        {
            canJump = true;
            if (isMoving)
                rb.drag = 0.0f;
            else
                rb.drag = defaultFriction;
            isMoving = false;
        }
        else
        {
            canJump = false;
            rb.drag = 0.0f;
        }

        // get velocity in local space
        velocityLocal = transform.InverseTransformDirection(rb.velocity);
        velocityLatWorld = transform.TransformDirection(new Vector3(velocityLocal.x, 0f, velocityLocal.z));
        velocityVertWorld = transform.TransformDirection(new Vector3(0f, velocityLocal.y, 0f));

        // default gravity
        // if no zones to reference, use default gravity and orientation
        if (isMagnetized)
        {
            // let magnetic boots deal with pull and orientation
        }
        else if(PhysicsVolumes.Count == 0)
        {
            // accelerate in defaultDirection
            rb.AddForce(defaultGravityDirection * defaultGravityMagnitude, ForceMode.Acceleration);
            // accelerate to defaultOrientation
            
        }
        else
        {
            // accelerate in direction of lowest weight PhysicsVolume
            PhysicsVolumes[0].pullBody(rb);
            // accelerate to new orientation
            PhysicsVolumes[0].reorientBody(rb);
        }
    }

    // just add jumpvelocity
    public void StartJump()
    {
        isMoving = true;

        if (canJump && !isJumping)
        {
            isJumping = true;
            rb.velocity = (transform.up * jumpVelocity) + velocityLatWorld; // add velocity of object underneath
            //rb.AddForce((transform.up * jumpVelocity) - velocityVertWorld, ForceMode.Impulse);
        }
            
    }

    public void StopJump()
    {
        isJumping = false;
    }

    // called by PlayerController when receiving lateral movement input
    public void LateralMove(Vector3 input)
    {
        isMoving = true;

        float accel;
        // check if grounded
        if (!isGrounded)
            accel = airAccel;
        else
            accel = groundAccel;

        // cap off influence with countermovement
        if (velocityLatWorld.magnitude >= maxMoveSpeed)
            rb.AddForce(-velocityLatWorld.normalized * accel * input.magnitude, ForceMode.Acceleration);

        // add input forces
        rb.AddForce(input.x * transform.right * accel, ForceMode.Acceleration);
        rb.AddForce(input.y * transform.forward * accel, ForceMode.Acceleration);
    }

    // add PhysicsVolume to list
    public void AddPhysicsVolume(PhysicsVolume volume)
    {
        // go through each PhysicsVolume and compare it's relationship to newPhysics, back to front
        for (int i = (PhysicsVolumes.Count -1); i < 0; i--)
        {
            // compare weight to existing ones, add if 
            if (PhysicsVolumes[i].getWeight() < volume.getWeight())
            {
                // add to list in correct place, after next lowest value
                PhysicsVolumes.Insert(i, volume);
                // end for loop as Volume has been placed
                i = -1;
            }
        }
    }

    // remove PhysicsVolume to list
    public void RemovePhysicsVolume(PhysicsVolume volume)
    {
        // remove PhysicsVolume from list
        PhysicsVolumes.Remove(volume);
    }
}
