using System.Collections.Generic;
using UnityEngine;

public class TriangularGrid : MonoBehaviour
{
    private float edgeLength = 2.962f; // Lenght of the edge of the Triangular Token
    private float sqrt3 = Mathf.Sqrt(3);

    public GameObject token;
    public Vector3Int initialPosition;

    public List<(int x, int y, int z)> occupiedPositions = new List<(int x, int y, int z)>();

    public Vector2 TriCenter(int a, int b, int c)
    {
        // Calculate the center using the provided formula
        float x = (0.5f * a - 0.5f * c) * edgeLength;
        float y = (-sqrt3 / 6 * a + sqrt3 / 3 * b - sqrt3 / 6 * c) * edgeLength;

        return new Vector2(x, y);
    }

    public Vector2 TriCenter(Vector3Int tri)
    {
        return TriCenter(tri.x, tri.y, tri.z);
    }

    // Function to check if a triangle is pointing upwards
    public bool PointsUp(int a, int b, int c)
    {
        return a + b + c == 2;
    }

    public bool PointsUp(Vector3Int tri)
    {
        return PointsUp(tri.x, tri.y, tri.z);
    }

    // Function to find the triangle containing a given Cartesian coordinate point
    // Returns null if the triangle is already occupied
    public Vector3Int? PickTri(float x, float y)
    {
        // Using dot product, measures which row and diagonals a given point occupies.
        // Or equivalently, multiply by the inverse matrix to tri_center
        // Note we have to break symmetry, using floor(...)+1 instead of ceil, in order
        // to deal with corner vertices like (0, 0) correctly.

        int a = Mathf.CeilToInt((1 * x - sqrt3 / 3 * y) / edgeLength);
        int b = Mathf.FloorToInt((sqrt3 * 2 / 3 * y) / edgeLength) + 1;
        int c = Mathf.CeilToInt((-1 * x - sqrt3 / 3 * y) / edgeLength);

        if(occupiedPositions!=null && occupiedPositions.Contains((a, b, c)))        
            return null;        
        else
        {
            occupiedPositions.Add((a, b, c));
            return new Vector3Int(a, b, c);
        }
    }

    // Function to get triangles that share an edge with the given triangle
    public Vector3Int[] TriNeighbours(int a, int b, int c)
    {
        if (PointsUp(a, b, c))
        {
            return new Vector3Int[]
            {
                new Vector3Int(a - 1, b, c),
                new Vector3Int(a, b - 1, c),
                new Vector3Int(a, b, c - 1)
            };
        }
        else
        {
            return new Vector3Int[]
            {
                new Vector3Int(a + 1, b, c),
                new Vector3Int(a, b + 1, c),
                new Vector3Int(a, b, c + 1)
            };
        }
    }


    //TODO : Test purposes only, just delete!
    private void Start()
    {
        Vector2 center1 = TriCenter(initialPosition.x, initialPosition.y, initialPosition.z);
        Vector2 center2 = TriCenter(1, 0, 1);
        Vector2 center3 = TriCenter(1, 1, 0);
        Vector2 center4 = TriCenter(0, 2, 0);

        occupiedPositions.Add((initialPosition.x, initialPosition.y, initialPosition.z));
        occupiedPositions.Add((1,0, 1));
        occupiedPositions.Add((1, 1, 0));
        occupiedPositions.Add((0, 2, 0));

        Vector3 initialPos = new Vector3(center1.x, transform.position.y, center1.y);
        Vector3 initialPos2 = new Vector3(center2.x, transform.position.y, center2.y);
        Vector3 initialPos3 = new Vector3(center3.x, transform.position.y, center3.y);
        Vector3 initialPos4 = new Vector3(center4.x, transform.position.y, center4.y);

        Instantiate(token, initialPos, transform.rotation);
        Instantiate(token, initialPos2, transform.rotation);
        Instantiate(token, initialPos3, transform.rotation);
        Instantiate(token, initialPos4, transform.rotation);
    }
}