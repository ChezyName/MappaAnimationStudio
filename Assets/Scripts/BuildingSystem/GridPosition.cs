using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct Buildable
{
    public bool isPlaced;
    public GameObject Item;
}

public class GridPosition : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Grid grid;

    [SerializeField]
    private LayerMask GridMask;
    
    [SerializeField]
    public GameObject Spawnable;

    [SerializeField] private GameObject GridVisualization;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);

    public bool buildMode = true;
    
    //Grid Item Spawnables
    private Buildable[,] SpawnList;

    [SerializeField]
    private Vector2 BottomLeftCornerXY = new Vector2(0, 0);
    
    [SerializeField]
    private Vector2 TopRightCornerXY = new Vector2(10, 10);
    
    // Start is called before the first frame update
    void Start()
    {
        if(cam == null) cam = Camera.current;
        grid = GetComponent<Grid>();
        if (grid == null)
        {
            grid = GetComponentInChildren<Grid>();
        }
        
        lastPosition = grid.WorldToCell(Vector3.zero);
        
        GridVisualization.SetActive(buildMode);

        int x = (int)(-BottomLeftCornerXY.x + TopRightCornerXY.x);
        int y = (int)(-BottomLeftCornerXY.x + TopRightCornerXY.x);
        SpawnList = new Buildable[x,y];
    }

    private GameObject Vis = null;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildMode = !buildMode;
            GridVisualization.SetActive(buildMode);
            Destroy(Vis);
        }
        
        if (buildMode)
        {
            if (Vis == null)
            {
                Vector3 pos = GetPositionOnGrid();
                pos.y = offset.y;
                pos.x += offset.x;
                pos.z += offset.z;

                Vis = Instantiate(Spawnable, pos,
                    Quaternion.identity);
            }
            else
            {
                Vector3 pos = GetPositionOnGrid();
                pos.y = offset.y;
                pos.x += offset.x;
                pos.z += offset.z;
                
                Vis.transform.SetPositionAndRotation(pos, Quaternion.identity);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = GetPositionOnGrid();
                
                //Debug.Log(pos);
                
                int x = (int) pos.x + (int) -BottomLeftCornerXY.x;
                int y = (int) pos.z + (int) -BottomLeftCornerXY.y;
                
                //Debug.Log(x + ", " + y);
                
                if (!SpawnList[x,y].isPlaced)
                {
                    pos.y = offset.y;
                    pos.x += offset.x;
                    pos.z += offset.z;
            
                    GameObject spawned = Instantiate(Spawnable, pos, 
                        Quaternion.identity);

                    SpawnList[x,y].isPlaced = true;
                    SpawnList[x,y].Item = spawned;
                }
                else Debug.Log("Cannot Spawn At Location: " + pos);
            }
        }
        else
        {
            if(Vis != null) Destroy(Vis);
        }
    }

    private Vector3 lastPosition;
    public Vector3 GetPositionOnGrid()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = cam.nearClipPlane;

        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 99999, GridMask))
        {
           lastPosition = grid.WorldToCell(hit.point);;
            
            Debug.DrawLine(cam.transform.position, hit.point, Color.red);
        }

        return lastPosition;
    }
    
    public void DrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
    {
        // create matrix
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, scale);
 
        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));
 
        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));
 
        Debug.DrawLine(point1, point2, c);
        Debug.DrawLine(point2, point3, c);
        Debug.DrawLine(point3, point4, c);
        Debug.DrawLine(point4, point1, c);
 
        Debug.DrawLine(point5, point6, c);
        Debug.DrawLine(point6, point7, c);
        Debug.DrawLine(point7, point8, c);
        Debug.DrawLine(point8, point5, c);
 
        Debug.DrawLine(point1, point5, c);
        Debug.DrawLine(point2, point6, c);
        Debug.DrawLine(point3, point7, c);
        Debug.DrawLine(point4, point8, c);
 
        // optional axis display
        //Debug.DrawRay(m.GetPosition(), m.GetForward(), Color.magenta);
        //Debug.DrawRay(m.GetPosition(), m.GetUp(), Color.yellow);
        //Debug.DrawRay(m.GetPosition(), m.GetRight(), Color.red);
    }
}
