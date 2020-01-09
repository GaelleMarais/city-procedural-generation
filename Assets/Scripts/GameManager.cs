using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefab;

    public int nbrBuilding = 10;
    public int distBetweenBuilding = 50;

    public float heightMin = 10;
    public float heightMax = 30;

    public float widthMin = 10;
    public float widthMax = 30;

    public float lengthMin = 5;
    public float lengthMax = 100;

    public float numberSquareMin = 1;
    public float numberSquareMax = 10;

    public float probabiltySquare = 0.5f;
    [Range(0f, 1f)]
    public float dispersion;
    [Range(0.01f, 0.10f)]
    public float coeffDistance;

    public Material material;
    private int min_number_meridians = 25;
    private int max_number_meridians = 40;
    private int min_number_slices_skipped = 5;
    private int max_number_slices_skipped = 10;
    private Vector2 size_building = new Vector2(10, 10);

    public bool aligned = false;

    public Vector3 townCenter = new Vector3(0, 0, 0);

    public bool autoUpdate = false;

    private float distMax;
    GameObject city;

    public void Destroy()
    {
        DestroyImmediate(city);
    }

    public void Generate()
    {
        city = new GameObject();

        Center().transform.SetParent(city.transform) ;

        // Distance maximale a partir de laquelle les buildings seront a la taille minimale
        distMax = (Mathf.Sqrt(nbrBuilding) / 2 * distBetweenBuilding) * Mathf.Sqrt(nbrBuilding) / (nbrBuilding * coeffDistance);

        for (int i = -(int)Mathf.Sqrt(nbrBuilding) / 2; i < Mathf.Sqrt(nbrBuilding) / 2; i++)
        {
            for (int j = -(int)Mathf.Sqrt(nbrBuilding) / 2; j < Mathf.Sqrt(nbrBuilding) / 2; j++)
            {
                Vector3 pos;

                if (aligned)
                { // Disposer les buildings sur une grille
                    pos = new Vector3(i * distBetweenBuilding, 0, j * distBetweenBuilding);
                }
                else
                { // Disposer les buildings de maniere aleatoire
                    pos = new Vector3(i * distBetweenBuilding - Random.Range(0.0f, distBetweenBuilding), 0, j * distBetweenBuilding - Random.Range(0.0f, distBetweenBuilding));
                }

                float dist = Vector3.Distance(pos, townCenter);
                float coeff = 1.0f - (0.9f * (dist / distMax));

                float height = Mathf.Max(Random.Range(heightMin, heightMax) * coeff, heightMin);
                float width = Random.Range(widthMin, widthMax);
                float length = Random.Range(lengthMin, lengthMax);

                // Modifier la position pour espacer les buildings quand on s'eloigne du centre
                pos.x = pos.x + (pos.x / coeff) * Random.Range(0f, dispersion);
                pos.z = pos.z + (pos.z / coeff) * Random.Range(0f, dispersion);

                if (Random.Range(0.0f, 1.0f) < probabiltySquare)
                {
                    createSquareBuilding(pos, height, length, width);
                }
                else
                {
                    createRoundBuilding(pos, height, length, width);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject Center()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = townCenter + new Vector3(0, 200, 0);
        sphere.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
        sphere.transform.localScale = sphere.transform.localScale * 10;

        return sphere;
    }

    public void createSquareBuilding(Vector3 pos, float height, float length, float width)
    {
        GameObject building = Instantiate(prefab, pos, Quaternion.identity,city.transform);
        float numberSquare = Random.Range(numberSquareMin, numberSquareMax);
        float dist = pos.magnitude;
        // Color color = new Color( dist / distMax, 0.5f + (dist / distMax) / 2, 0.5f + (dist  / distMax) / 2);
        Color color = Color.HSVToRGB(0.6f, 1 - dist / distMax, 1f, true);
        building.GetComponent<SquareBuilding>().SetParameter(height, width, length, numberSquare,color);
        building.GetComponent<SquareBuilding>().Generate();
    }


    public void createRoundBuilding(Vector3 pos, float height, float length, float width)
    { 
        int number_meridians = Random.Range(min_number_meridians, max_number_meridians);
        int number_slices_skipped = Random.Range(min_number_slices_skipped, max_number_slices_skipped);
        int index = Random.Range(3, number_meridians / 2 - number_slices_skipped - 3);

        GameObject new_building = new GameObject("round building");

        new_building.AddComponent<MeshFilter>();
        new_building.AddComponent<MeshRenderer>();
        new_building.AddComponent<RoundBuilding>();

        new_building.GetComponent<MeshRenderer>().material = material;

        float dist = pos.magnitude;
        new_building.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(0.05f, 1 - dist / distMax, 1f, true);
        new_building.transform.Translate(pos);
        new_building.transform.localScale += new Vector3(0f, height, 0f);

        new_building.GetComponent<RoundBuilding>().create_round_building(width, length, 1, number_meridians, number_slices_skipped, index);
        new_building.transform.SetParent(city.transform);
    }
}
