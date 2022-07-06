using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private bool isPriority;

    public abstract void Interact();

    private void OnTriggerEnter(Collider collision)
    {
        // add to player's Interactable list
        if (isPriority)
        {
            collision.GetComponent<InteractManager>().BeginOverlapWithPriority(this);
        }
        else
        {
            collision.GetComponent<InteractManager>().BeginOverlap(this);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        collision.GetComponent<InteractManager>().EndOverlap(this);
    }
}
