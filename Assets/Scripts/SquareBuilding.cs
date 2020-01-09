using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBuilding : MonoBehaviour
{
    public GameObject prefab;

    public float sizeMax;
    public float widthMax;
    public float heightMax;
    public float numberSquare;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        for (int i = 0; i < numberSquare; i++)
        {
            float sizeActual = sizeMax - (i * Random.Range(0.0f, 1 / numberSquare) * sizeMax);

            float widthActual;
            if (Random.Range(0, 1) == 0)
                widthActual = widthMax * Random.Range(-.8f, -.2f);
            else
                widthActual = widthMax * Random.Range(.2f, .8f);

            float heighActual;
            if (Random.Range(0, 1) == 0)
                heighActual = heightMax * Random.Range(-.8f, -.2f);
            else
                heighActual = heightMax * Random.Range(.2f, .8f);

            GameObject cube = Instantiate(prefab, gameObject.transform.position + new Vector3(heightMax / 2, sizeActual / 2, widthMax / 2), Quaternion.identity, gameObject.transform);

            cube.transform.localScale = new Vector3(heighActual, sizeActual, widthActual);

            cube.GetComponent<MeshRenderer>().material.color = color;

            Mesh mesh = cube.GetComponent<MeshFilter>().sharedMesh;
            Vector2[] uv = mesh.uv;

            uv[4] = new Vector2(0, 0);
            uv[5] = new Vector2(0, 0);
            uv[8] = new Vector2(0, 0);
            uv[9] = new Vector2(0, 0);

            mesh.uv = uv;

            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParameter(float sM, float wM, float hM, float nS, Color c)
    {
        sizeMax = sM;
        widthMax = wM;
        heightMax = hM;
        numberSquare = nS;
        color = c;
    }
}
