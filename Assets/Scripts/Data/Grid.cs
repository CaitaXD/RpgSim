using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int height;
    private int width;
    private int[,] gridArray;
    private float cellSpacing;
    private Sprite grahpic;
    public GameObject[,] GridArray;
    public Grid(int width, int height, float cellSpacing, Sprite graphic,Transform parent)
    {
        this.width = width;
        this.height = height;
        this.cellSpacing = cellSpacing;
        gridArray = new int[width, height];
        GridArray = new GameObject[height, width];
        for (int x = 0; x< gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GameObject gameObject = new GameObject("Tile", typeof(SpriteRenderer));
                GridArray[x, y] = gameObject;
                var col = gameObject.AddComponent<BoxCollider>();
                SpriteRenderer spriteMask = gameObject.GetComponent<SpriteRenderer>();
                spriteMask.sprite = graphic;
                Transform transform = gameObject.transform;
                gameObject.AddComponent<Tile>();
                transform.localPosition = new Vector3(x,y,0)*cellSpacing;
                if(parent != null)
                transform.SetParent(parent);
                col.isTrigger = true;
                col.size = new Vector3(1,1,0);
            }
        }
           var pos = parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
           pos.y += Mathf.Sin(Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad)/(Mathf.Cos(Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad)) * Camera.main.transform.position.z - height/2 + 0.4f;
           pos.x += Mathf.Sin(-Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad)/(Mathf.Cos(-Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad)) * Camera.main.transform.position.z - width/2;
           pos.z = Mathf.Sin(Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad)/(Mathf.Cos(Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad)) * Camera.main.transform.position.z;
           parent.transform.position = pos;      
    }
}