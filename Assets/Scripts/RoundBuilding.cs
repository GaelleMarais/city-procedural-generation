using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBuilding : MonoBehaviour
{
    private Vector3[] vertices;
    private int number_vertices;
    private int[] triangles;
    private int number_triangles;
    private Vector2[] uv;
    private int number_uv;

    public Material material;
    public float length;
    public float width;
    public float height;
    public int number_meridians;
    public int number_slices_skipped;
    int index;


    public void create_round_building(float length, float width, float height, int number_meridians, int number_slices_skipped, int index)
    {	
        // create arrays
		vertices = new Vector3[(number_meridians + 1 - number_slices_skipped * 2) * 4 + 50];
		triangles = new int[(number_meridians - number_slices_skipped * 2) * 12 + 72];
		uv = new Vector2[(number_meridians + 1 - number_slices_skipped * 2) * 4 + 50];

        // fill arrays
		fill_arrays(length, width, height, number_meridians, number_slices_skipped, index);


        // create mesh
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;

		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		gameObject.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
    }


    public void fill_arrays(float length, float width, float height, int number_meridians, int number_slices_skipped, int index)
    {
        number_vertices = (number_meridians + 1 - number_slices_skipped * 2) * 4 + 2;
        vertices = new Vector3[number_vertices];

        number_triangles = (number_meridians - number_slices_skipped * 2) * 12;
        triangles = new int[number_triangles];

        uv = new Vector2[number_vertices];

        float x;
        float y;
        float radius_x = length / 2;
        float radius_y = width / 2;
        int count = 0;

        for (int i = 0; i < number_meridians + 1; i++)
        {
            x = radius_x * Mathf.Cos(2 * Mathf.PI * i / number_meridians);
            y = radius_y * Mathf.Sin(2 * Mathf.PI * i / number_meridians);

            uv[count] = new Vector2(1f / number_meridians * i, 0);
            uv[count + ((number_meridians + 1 - number_slices_skipped * 2)) * 2] = new Vector2(uv[count].x, 1);
            uv[count + 1] = new Vector2(1f / number_meridians * i, 0);
            uv[count + 1 + ((number_meridians + 1 - number_slices_skipped * 2)) * 2] = new Vector2(uv[count].x, 1);

            // increment i when skipping slices
            if (i == index || i == number_meridians - index - number_slices_skipped){
                i += number_slices_skipped;
            } 

            vertices[count] = new Vector3(x + length / 2, 0, y + width / 2);
            vertices[count + ((number_meridians + 1 - number_slices_skipped * 2)) * 2] = new Vector3(x + length / 2, height, y + width / 2);
            vertices[count + 1] = vertices[count];
            vertices[count + 1 + ((number_meridians + 1 - number_slices_skipped * 2)) * 2] = new Vector3(x + length / 2, height, y + width / 2);

            count += 2;
        }

        vertices[number_vertices - 2] = new Vector3(length / 2, 0, width / 2);
        vertices[number_vertices - 1] = new Vector3(length / 2, height, width / 2);
        uv[number_vertices - 2] = new Vector2(0, 0);
        uv[number_vertices - 1] = new Vector2(1, 1);

        int number_triangles_body = number_triangles - ((number_meridians - number_slices_skipped * 2) * 6);

        for (int i = 0, k = 0; i < number_triangles_body; i += 6, k += 2)
        {
            triangles[i] = triangles[i + 3] = k;
            triangles[i + 1] = k + ((number_meridians + 1 - number_slices_skipped * 2)) * 2;
            triangles[i + 2] = triangles[i + 4] = k + ((number_meridians - number_slices_skipped * 2) + 2) * 2;
            triangles[i + 5] = k + 2;
        }

        for (int i = 0, k = 0; k < (number_meridians - number_slices_skipped * 2) * 2; i += 6, k += 2)
        {
            triangles[i + number_triangles_body] = k + 1;
            triangles[i + number_triangles_body + 1] = k + 3;
            triangles[i + number_triangles_body + 2] = number_vertices - 2;
            triangles[i + number_triangles_body + 3] = k + (number_meridians + 1 - number_slices_skipped * 2) * 2 + 3;
            triangles[i + number_triangles_body + 4] = k + (number_meridians + 1 - number_slices_skipped * 2) * 2 + 1;
            triangles[i + number_triangles_body + 5] = number_vertices - 1;
        }
    }
}
