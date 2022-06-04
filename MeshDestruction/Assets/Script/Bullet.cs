using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private SphereCollider sphereCollider;


    private void OnTriggerEnter(Collider other)
    {
        //apply Destruction
        if (other.gameObject.layer == 10)
        {
            voronoiDestruction.SimpleCut(other.gameObject.transform, other.gameObject.transform.position);
        }
    }
}
