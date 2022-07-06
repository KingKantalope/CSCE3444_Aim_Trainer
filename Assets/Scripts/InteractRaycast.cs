using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    private Interactable raycastedObj;
    private List<Interactable> zonedObj;

    private GameObject interactIcon;
    private bool isIconActive;
    private bool doOnce;
    private bool inObjectField;

    private const string interactableTag = "InteractiveObject";

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if (!doOnce)
                {
                    raycastedObj = hit.collider.gameObject.GetComponent<Interactable>();
                    ShowIcon(true);
                }

                isIconActive = true;
                doOnce = true;
            }
        }
        else
        {
            if (isIconActive && !inObjectField)
            {
                ShowIcon(false);
                isIconActive = false;
                doOnce = false;
            }

            raycastedObj = null;
        }
    }

    // Show interact prompt upon being in proximity
    private void ShowIcon(bool on)
    {
        if(on && !doOnce)
        {
            OpenInteractableIcon();
        }
        else
        {
            CloseInteractableIcon();
        }
    }

    private void OpenInteractableIcon()
    {
        interactIcon.SetActive(true);
    }

    private void CloseInteractableIcon()
    {
        interactIcon.SetActive(false);
    }

    // default proximity list add
    public void BeginOverlap(Interactable intProp)
    {
        zonedObj.Add(intProp);
        inObjectField = true;
    }

    // add to front of proximity list for priority
    public void BeginOverlapWithPriority(Interactable intProp)
    {
        zonedObj.Insert(0, intProp);
        inObjectField = true;
    }

    public void EndOverlap(Interactable intProp)
    {
        // remove Interactable from the list
        if (zonedObj.Remove(intProp))
        {
            // set inObjectField to false if list is empty
            if (zonedObj.Count == 0)
            {
                inObjectField = false;
            }
        }
    }

    // interact with object being looked at
    public void Interact()
    {
        if (isIconActive)
        {
            if (!raycastedObj)
            {

            }
            raycastedObj.Interact();
        }
    }
}
