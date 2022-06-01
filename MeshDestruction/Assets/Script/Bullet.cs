using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private SphereCollider sphereCollider;


    private void OnTriggerEnter(Collider other)
    {
        //apply Destruction
        voronoiDestruction.Instance.BaseDestruction(other.gameObject);
    }
}
