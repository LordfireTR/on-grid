using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    int width;
    int height;
    float cellSize;
    Vector3 originPosition;
    TGridObject[,] gridArray;
    TextMesh[,] debugGridArray;

    bool showDebug = true;
        
    public Grid (int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        if (showDebug)
        {
            debugGridArray = new TextMesh[width, height];
    
            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    debugGridArray[x, z] = CreateWorldText(gridArray[x, z]?.ToString(), GetCellPosition(x, z) + new Vector3 (cellSize, 0, cellSize) / 2, 15, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetCellPosition(x, z), GetCellPosition(x, z+1), Color.white, 100f);
                    Debug.DrawLine(GetCellPosition(x, z), GetCellPosition(x+1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetCellPosition(0, height), GetCellPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetCellPosition(width, 0), GetCellPosition(width, height), Color.white, 100f);
        }
    }

    public static TextMesh CreateWorldText (string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.localPosition = localPosition;
        transform.eulerAngles = new Vector3(60, 45, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    public Vector3 GetCellPosition(int x, int z)
    {
        return new Vector3 (x, 0, z) * cellSize + originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    // public void AddValue (int x, int z, int value)
    // {
    //     if (x >= 0 && z >= 0 && x < width && z < height)
    //     {
    //         gridArray[x, z] += value;
    //         debugGridArray[x, z].text = gridArray[x, z].ToString();
    //     }
    // }

    // public void AddValue (Vector3 worldPosition, int value)
    // {
    //     int x, z;
    //     GetXZ(worldPosition, out x, out z);
    //     AddValue(x, z, value);
    // }

    public void SetGridObject (int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
            if (showDebug)
            {
                debugGridArray[x, z].text = gridArray[x, z]?.ToString();
            }
        }
    }

    public void SetGridObject (Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject (int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    public int GetWidth()
    {
        return gridArray.GetLength(0);
    }

    public int GetHeight()
    {
        return gridArray.GetLength(1);
    }

    public float GetCellSize()
    {
        return cellSize;
    }
}
