using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PickUp : MonoBehaviour
{
    public float distanceToPickup = 0.3f;
    bool handClosed = false;
    public LayerMask pickable;

    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.LeftHand;

    Rigidbody heldObject;

    void FixedUpdate()
    {
        if (SteamVR_Actions.default_GrabGrip.GetState(hand))
            handClosed = true;
        else
            handClosed = false;
        if (!handClosed)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, distanceToPickup, pickable);
            if (colliders.Length > 0)
                heldObject = colliders[0].transform.root.GetComponent<Rigidbody>();
            else
                heldObject = null;
        }

        else
        {
            if(heldObject)
            {
                heldObject.velocity = (transform.position - heldObject.transform.position) / Time.fixedDeltaTime;

                heldObject.maxAngularVelocity = 20;
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(heldObject.transform.rotation);
                Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
                eulerRot *= 0.95f;
                eulerRot *= Mathf.Deg2Rad;
                heldObject.angularVelocity = eulerRot / Time.fixedDeltaTime;
            }
        }
    }

 
}
