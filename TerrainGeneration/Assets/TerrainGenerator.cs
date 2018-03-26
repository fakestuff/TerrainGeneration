using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    Vector3[] newVertices;
    int[] newTriangles;
    Vector3[] newNormals;
    private float[,] terrainData;
    private Mesh terrain;

    public int width = 128;

    public int height = 128;
    [Range(0.01f, 30.0f)]
    public float amplitute = 1.0f;
    // Use this for initialization
    void Start () {
		newVertices = new Vector3[width*height];
        newNormals = new Vector3[width * height];
        newTriangles = new int[(width-1)*(height-1)*6];
        terrain = GetComponent<MeshFilter>().mesh;
        terrainData = new float[height,width];

        GenerateRawTerrainData();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    UpdateTerrain();
        UpdateMesh();
	    
	}

    void GenerateRawTerrainData()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                terrainData[row, col] = UnityEngine.Random.Range(-1.0f, 1.0f);
            }
        }
        
    }
    void UpdateTerrain()
    {
        //for (int row = 0; row < height; row++)
        //{
        //    for (int col = 0; col < width; col++)
        //    {
        //        terrainData[row, col] = Mathf.Sin(col)+ Mathf.Cos(row);
        //    }
        //}
       
    }

    void UpdateMesh()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width  ; col++)
            {
                newVertices[row * width + col] = new Vector3(col,terrainData[row,col],row);
            }
        }
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (row == (height - 1) || col == (width - 1))
                {
                    newNormals[row * width + col] = new Vector3(0.5f,0.5f,0.5f);
                    
                    continue;
                }
                Vector3 dy = Vector3.Normalize(newVertices[(row+1)*width+col] - newVertices[row * width + col]);
                Vector3 dx = Vector3.Normalize(newVertices[row * width + col + 1] - newVertices[row * width + col]);

                newNormals[row * width + col] = Vector3.Cross(dy,dx);
            }
        }

        

        for (int row = 0; row < height-1; row++)
        {
            for (int col = 0; col < width-1; col++)
            {
                //xy
                //00 row col
                //01 row col+1
                //10 col+1 row
                //11 col+1 row+1
                int index = (row * (width - 1)+col) * 6;
                // 00 01 10 
                newTriangles[index++] = row*(width)+col;
                newTriangles[index++] = (row + 1) * (width) + col;
                newTriangles[index++] = (row) * (width )+ col + 1;
                
                // 01 11 10
                newTriangles[index++] = row * (width ) + col + 1;
                newTriangles[index++] = (row + 1) * (width) + col;
                newTriangles[index] = (row + 1) * (width ) + col+1;
                

            }
        }
        
        terrain.vertices = newVertices;
     
        terrain.normals = newNormals;
        terrain.triangles = newTriangles;
    }
}
