using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private bool horizontalCut = false;



    private void OnTriggerEnter(Collider other)
    {
        //apply destruction
        if(horizontalCut)
        {
            //apply everything but horizontal
            //get impact point
            //create the start and end points 
        }
        else if(horizontalCut)
        {
            //apply cutting vertical
        }

        /*_triggerExitTipPosition = _tip.transform.position;

        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        //Get the point perpendicular to the triangle above which is the normal
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }*/
        //create plane 
        Plane plane = new Plane();

        if(other.TryGetComponent<Renderer>(out Renderer objectHit))
        {
            Vector3 planeYZ = new Vector3(0,objectHit.bounds.size.y, objectHit.bounds.size.z);

            //Vector3 side1 = ;
            //Vector3 side2 = ;

            //Vector3 planeNormal = Vector3.Cross()

            //plane.SetNormalAndPosition();
        }

        GameObject[] slices = Slicer.Slice(plane, other.gameObject);
        Destroy(other.gameObject);

        //Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
        //Vector3 newNormal = transformedNormal + Vector3.up * _forceAppliedToCut;
        //rigidbody.AddForce(newNormal, ForceMode.Impulse);
    }
}
