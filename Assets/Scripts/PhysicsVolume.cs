using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsVolume : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField] private float pullForce = 16f;
    [SerializeField] private float reorientAngVel = 2 * Mathf.PI;
    [SerializeField] private Vector3 pullDir = Vector3.down;
    [SerializeField] private Quaternion orientationDir = Quaternion.identity;
    [SerializeField] private int weight = 0;

    public void pullBody(Rigidbody Body)
    {
        // pull Body to target
        Body.AddForce(pullForce * pullDir, ForceMode.Acceleration);
    }

    public void reorientBody(Rigidbody Body)
    {
        // reorient Body to point top towards target

    }

    public int getWeight()
    {
        return weight;
    }
}
