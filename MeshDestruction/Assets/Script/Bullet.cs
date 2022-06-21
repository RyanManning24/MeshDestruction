using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float _forceAppliedToCut = 10f;
    [SerializeField] private bool horizontalCut = false;

    private bool isActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        //apply destruction
        if (horizontalCut && collision.gameObject.GetComponent<Sliceable>() && isActive)
        {
            if (collision.gameObject.TryGetComponent<Renderer>(out Renderer objectHit))
                isActive = false;
            //apply everything but horizontal
            //get impact point
            //create the start and end points 
        }
        else if (!horizontalCut && collision.gameObject.GetComponent<Sliceable>() && isActive)
        {
            //apply cutting vertical
            if (collision.gameObject.TryGetComponent<Renderer>(out Renderer objectHit))
            {
                isActive = false;

                Vector3 planeYZ = new Vector3(0, objectHit.bounds.size.y, objectHit.bounds.size.z);

                Vector3 collisionPoint = collision.contacts[0].point;

                float pointA = collisionPoint.y - planeYZ.y;
                Vector3 vectorA = new Vector3(collisionPoint.x, pointA, collisionPoint.z);

                float pointB = collisionPoint.y + planeYZ.y;
                Vector3 vectorB = new Vector3(collisionPoint.x, pointB, collisionPoint.z);

                Vector3 endVector = new Vector3(collisionPoint.x, collisionPoint.y, collisionPoint.z + planeYZ.z);

                Vector3 side1 = vectorB - vectorA;
                Vector3 side2 = vectorB - endVector;

                Vector3 normal = Vector3.Cross(side1, side2).normalized;

                Vector3 transformedNormal = ((Vector3)(collision.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;


                Plane plane = new Plane();
                plane.SetNormalAndPosition(transformedNormal, planeYZ);

                var direction = Vector3.Dot(Vector3.up, transformedNormal);

                if (direction < 0)
                {
                    plane = plane.flipped;
                }

                GameObject[] slices = Slicer.Slice(plane, collision.gameObject);
                Destroy(collision.gameObject);

                Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
                Vector3 newNormal = transformedNormal + Vector3.up * _forceAppliedToCut;
                rigidbody.AddForce(newNormal, ForceMode.Impulse);
            }
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

        //Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
        //Vector3 newNormal = transformedNormal + Vector3.up * _forceAppliedToCut;
        //rigidbody.AddForce(newNormal, ForceMode.Impulse);
    }

    public void SetIsActive(bool active)
    {
        isActive = active;
    }

}
