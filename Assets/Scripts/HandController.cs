using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private SteamVR_TrackedObject trackedObject;

    HashSet<Interactable> interactable = new HashSet<Interactable>();

    private Interactable interactingItem;

	// Use this for initialization
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (controller.GetPressDown(grip))
        {
            float minDistance = float.MaxValue;
            float distance;

            foreach (Interactable item in interactable)
            {
                distance = (item.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    interactingItem = item;
                }
            }

            if (interactingItem)
            {
                if (interactingItem.IsInteracting())
                {
                    interactingItem.EndInteraction(this);
                }

                interactingItem.StartInteraction(this);
            }
        }

        if (controller.GetPressUp(grip) && interactingItem != null)
        {
            interactingItem.EndInteraction(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable collider = other.GetComponent<Interactable>();
        if (collider)
        {
            interactable.Add(collider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable collider = other.GetComponent<Interactable>();
        if (collider)
        {
            interactable.Remove(collider);
        }
    }
}
