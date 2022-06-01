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

    }


}
