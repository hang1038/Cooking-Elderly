using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public Rigidbody rigidbody;

    private bool interacting;

    private float speed = 20000f;
    private Vector3 positionDelta;

    private float angularSpeed = 400f;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;

    private HandController hand;

    private Transform interactionPoint;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        speed /= rigidbody.mass;
        angularSpeed /= rigidbody.mass;
    }
	
	// Update is called once per frame
	void Update () {
		if (hand && interacting)
        {
            positionDelta = hand.transform.position - interactionPoint.position;
            this.rigidbody.velocity = positionDelta * speed * Time.fixedDeltaTime;

            rotationDelta = hand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
            {
                angle -= 360;
            }

            this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * angularSpeed;
        }
	}

    public void StartInteraction(HandController hand)
    {
        this.hand = hand;
        interactionPoint.position = hand.transform.position;
        interactionPoint.rotation = hand.transform.rotation;
        interactionPoint.SetParent(transform, true);
        interacting = true;
    }

    public void EndInteraction(HandController hand)
    {
        if (this.hand == hand)
        {
            this.hand = null;
            interacting = false;
        }
    }

    public bool IsInteracting()
    {
        return interacting;
    }
}
