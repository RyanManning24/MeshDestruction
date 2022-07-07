using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voronoiDestruction : MonoBehaviour
{

    #region singleton
    private static voronoiDestruction instance;
    public static voronoiDestruction Instance { get { return instance; } }
    #endregion

    private void Awake()
    {
        #region singleton awake
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        #endregion
    }

    public void BaseDestruction(GameObject target)
    {
        //WORK OUT EVERYTHING
    }

    public static void BaseMeshCut(GameObject target)
    {

    }

    public static void VoronoiDestruction(GameObject target)
    {
        //\[D= \left( {\sum_i^n \left|{a_i - b_i}\right |^p } \right)^{1/p}\]
        //D = +-triangleX to the power P + same but y all to the power of 1/p Minkowski distance
        //Manhattan distance is cheaper 

        //get the point of impact
        //calculate how hard the impact was 
        //apply destruction using voronoi 
    }

    public static bool SimpleCut(Transform target, Vector3 pos)
    {
        //Base Destruction TEST THIS 
        Vector3 position = new Vector3(pos.x,target.position.y,target.position.z);
        Vector3 targetScale = target.localScale;
        float distance = Vector3.Distance(target.position, position);
        if (distance >= targetScale.x / 2) return false;

        Vector3 leftPoint = target.position - Vector3.right * targetScale.x / 2;
        Vector3 rightPoint = target.position + Vector3.right * targetScale.x / 2;
        Material mat = target.GetComponent<MeshRenderer>().sharedMaterial;
        Destroy(target.gameObject);

        GameObject rightSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightSideObj.transform.position = (rightPoint + pos) / 2;
        float rightWidth = Vector3.Distance(position, rightPoint);
        rightSideObj.transform.localScale = new Vector3(rightWidth, targetScale.y, targetScale.z);
        rightSideObj.AddComponent<Rigidbody>().mass = 100f;
        rightSideObj.GetComponent<MeshRenderer>().material = mat;

        GameObject leftSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftSideObj.transform.position = (leftPoint + position) / 2;
        float leftWidth = Vector3.Distance(position,leftPoint);
        leftSideObj.transform.localScale = new Vector3(leftWidth, targetScale.y, targetScale.z);
        leftSideObj.AddComponent<Rigidbody>().mass = 100f;
        leftSideObj.GetComponent<MeshRenderer>().material = mat;

        return true;
    }

}
