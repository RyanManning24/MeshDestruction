using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletForce = 100;

    private Camera mainCamera;
    

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        
    }

    [Button("Shoot")]
    public void ShootBall()
    {
        GameObject bullet = Instantiate(bulletPrefab,mainCamera.transform.position,Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * bulletForce);

        //get what the ball hits
        

        Destroy(bullet, 3);
    }

}
