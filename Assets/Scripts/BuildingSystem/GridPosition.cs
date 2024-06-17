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

public struct Wall
{
    public bool isPlaced;
    public GameObject Item;
    public int Rotation;
}

public class GridPosition : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Grid grid;

    [SerializeField]
    private LayerMask GridMask;
    
    [SerializeField]
    public Placeable Spawnable;

    [SerializeField]
    private GameObject GridVisualization;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);

    public bool buildMode = true;
    
    //Grid Item Spawnables
    private Buildable[,] SpawnList;
    private Wall[,] WallList;

    [SerializeField]
    private Vector2 BottomLeftCornerXY = new Vector2(0, 0);
    
    [SerializeField]
    private Vector2 TopRightCornerXY = new Vector2(10, 10);

    //0 = 0
    //1 = 90
    //2 = 180
    //3 = 360
    public int RotationModifier = 0;
    public Quaternion Rotation;
    
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
        WallList = new Wall[x, y];

        Rotation = Quaternion.identity;
    }

    private bool checkIfWallPlaced(int x, int y,int RotationMod)
    {
        Wall wall = WallList[x, y];
        if (wall.isPlaced)
        {
            if (wall.Rotation == RotationMod) return true;
            return false;
        }
        return false;
    }

    private GameObject Vis = null;
    // Update is called once per frame
    void Update()
    {
        if(buildMode) GetPositionOnGrid();
        
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildMode = !buildMode;
            GridVisualization.SetActive(buildMode);
            Destroy(Vis);
        }
        
        if (buildMode)
        {
            if (Input.GetButtonDown("Rotate"))
            {
                RotationModifier++;
                if (RotationModifier > 3) RotationModifier = 0;

                int rot = RotationModifier == 0 ? 0 :
                    RotationModifier == 1 ? 90 :
                    RotationModifier == 2 ? 180 :
                    RotationModifier == 3 ? 270 : 0;
                Rotation = Quaternion.AngleAxis(rot, Vector3.up);
            }
            
            if (Vis == null)
            {
                Vector3 pos = lastPosition;
                pos.y = offset.y;
                pos.x += offset.x;
                pos.z += offset.z;

                Vis = Instantiate(Spawnable.Spawnable, pos,
                    Rotation);
            }
            else
            {
                Vector3 pos = lastPosition;
                pos.y = offset.y;
                pos.x += offset.x;
                pos.z += offset.z;
                
                Vis.transform.SetPositionAndRotation(pos, Rotation);
            }
            
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = GetPositionOnGrid();
                Debug.Log(pos);
                if (pos == new Vector3(-9999, -9999, -9999)) return;
                
                //Debug.Log(pos);
                
                int x = (int) pos.x + (int) -BottomLeftCornerXY.x;
                int y = (int) pos.z + (int) -BottomLeftCornerXY.y;
                
                //Debug.Log(x + ", " + y);

                Debug.Log("Placing Something!");
                if (Spawnable.isWall)
                {
                    Debug.Log("Wall!");
                    if (!checkIfWallPlaced(x,y,RotationModifier))
                    {
                        pos.y = offset.y;
                        pos.x += offset.x;
                        pos.z += offset.z;
            
                        GameObject spawned = Instantiate(Spawnable.Spawnable, pos, 
                            Rotation);

                        WallList[x,y].isPlaced = true;
                        WallList[x,y].Item = spawned;
                        WallList[x, y].Rotation = RotationModifier;
                    }
                }
                else
                {
                    Debug.Log("not Wall!");
                    if (!SpawnList[x,y].isPlaced)
                    {
                        pos.y = offset.y;
                        pos.x += offset.x;
                        pos.z += offset.z;
            
                        GameObject spawned = Instantiate(Spawnable.Spawnable, pos, 
                            Rotation);

                        SpawnList[x,y].isPlaced = true;
                        SpawnList[x,y].Item = spawned;
                    }
                }
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
           return lastPosition;
        }

        return new Vector3(-9999,-9999,-9999);
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
