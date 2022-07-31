using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voronoiDestruction : MonoBehaviour
{

    #region singleton
    private static voronoiDestruction instance;
    public static voronoiDestruction Instance { get { return instance; } }
    #endregion

    public Vector2Int imageDimensions;
    public int regionAmount;
    public bool drawByDistance = false;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Sprite.Create((drawByDistance ? GetDiagramByDistance() : GetDiagram()), new Rect(0,0,imageDimensions.x,imageDimensions.y),Vector2.one * 0.5f);
    }
    Texture2D GetDiagramByDistance()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];
        Color[] Regions = new Color[regionAmount];
        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDimensions.x), (Random.Range(0, imageDimensions.y)));
        }
        Color[] pixelColors = new Color[imageDimensions.x * imageDimensions.y];
        float[] distances = new float[imageDimensions.x * imageDimensions.y];
        for (int x = 0; x < imageDimensions.x; x++)
        {
            for (int y = 0; y < imageDimensions.x; y++)
            {
                int index = x * imageDimensions.x + y;
                distances[index] = Vector2.Distance(new Vector2Int(x,y), centroids[GetClosestCentroidIndex(new Vector2Int(x,y), centroids)]);
            }
        }
        float maxDist = GetMaxDistance(distances);
        for(int i = 0; i < distances.Length; i++)
        {
            float colourValue = distances[i] / maxDist;
            pixelColors[i] = new Color(colourValue, colourValue, colourValue, 1f);
        }

        return GetImageFromColourArray(pixelColors);
    }
    float GetMaxDistance(float[] distances)
    {
        float maxDist = float.MinValue;
        for(int i = 0; i < distances.Length; i++)
        {
            if(distances[i] > maxDist)
            {
                maxDist = distances[i];
            }
        }
        return maxDist;
    }
    Texture2D GetDiagram()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];
        Color[] Regions = new Color[regionAmount];
        for(int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDimensions.x), (Random.Range(0, imageDimensions.y)));
            Regions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
        Color[] pixelColors = new Color[imageDimensions.x * imageDimensions.y];
        for(int x = 0; x < imageDimensions.x; x++)
        {
            for (int y = 0; y < imageDimensions.x; y++)
            {
                int index = x * imageDimensions.x + y;
                pixelColors[index] = Regions[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)];

            }
        }
        return GetImageFromColourArray(pixelColors);
    }
    int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
    {
        float smallesDist = float.MaxValue;
        int index = 0;
        for(int i = 0; i < centroids.Length; i++)
        {
            if(Vector2.Distance(pixelPos, centroids[i]) < smallesDist)
            {
                smallesDist = Vector2.Distance(pixelPos, centroids[i]);
                index = i;
            }
        }
        return index;
    }
    Texture2D GetImageFromColourArray(Color[] pixelColours)
    {
        Texture2D tex = new Texture2D(imageDimensions.x, imageDimensions.y);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels(pixelColours);
        tex.Apply();
        return tex;
    }

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

    public static void VoronoiDestruction(GameObject target, Vector3 impactPoint, float velocity)
    {
        //\[D= \left( {\sum_i^n \left|{a_i - b_i}\right |^p } \right)^{1/p}\]
        //D = +-triangleX to the power P + same but y all to the power of 1/p Minkowski distance
        //Manhattan distance is cheaper 

        //get the point of impact

        //calculate how hard the impact was 

        //apply destruction using voronoi 


    }

    public void VoronoiDiagram()
    {

    }

    public static bool SimpleCut(Transform target, Vector3 pos)
    {
        
        //Base Destruction 
        //Store objects posistion and scale
        Vector3 position = new Vector3(pos.x,target.position.y,target.position.z);
        Vector3 targetScale = target.localScale;


        float distance = Vector3.Distance(target.position, position);
        if (distance >= targetScale.x / 2) return false;

        //create the left and right objects position
        Vector3 leftPoint = target.position - Vector3.right * targetScale.x / 2;
        Vector3 rightPoint = target.position + Vector3.right * targetScale.x / 2;

        //store material
        Material mat = target.GetComponent<MeshRenderer>().sharedMaterial;
        Destroy(target.gameObject);

        //Create Right side
        GameObject rightSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightSideObj.transform.position = (rightPoint + pos) / 2;
        float rightWidth = Vector3.Distance(position, rightPoint);
        rightSideObj.transform.localScale = new Vector3(rightWidth, targetScale.y, targetScale.z);
        rightSideObj.AddComponent<Rigidbody>().mass = 100f;
        rightSideObj.AddComponent<Sliceable>();
        rightSideObj.GetComponent<MeshRenderer>().material = mat;

        //Create Left side
        GameObject leftSideObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftSideObj.transform.position = (leftPoint + position) / 2;
        float leftWidth = Vector3.Distance(position,leftPoint);
        leftSideObj.transform.localScale = new Vector3(leftWidth, targetScale.y, targetScale.z);
        leftSideObj.AddComponent<Rigidbody>().mass = 100f;
        leftSideObj.AddComponent<Sliceable>();
        leftSideObj.GetComponent<MeshRenderer>().material = mat;
        

        return true;
    }

}
