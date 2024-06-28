using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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

    [SerializeField] private LayerMask Wall = 7;
    [SerializeField] private LayerMask Placeable = 8;

    //0 = 0
    //1 = 90
    //2 = 180
    //3 = 360
    private int RotationModifier = 0;
    private Quaternion Rotation;
    
    [Header("Default Wall Spawning")]
    [SerializeField]
    private Placeable defaultWalls;
    [SerializeField]
    private Placeable fireExit;

    [SerializeField] private int FireExitRotation = 0;

    [SerializeField] private Vector2Int DoorLocation;
    [SerializeField] private bool spawnWallsOnStart = true;

    private GameObject placeableHolder;
    private GameObject defaultWallHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        placeableHolder = new GameObject("Placeables");
        placeableHolder.transform.parent = transform;
        
        defaultWallHolder = new GameObject("Default Walls");
        defaultWallHolder.transform.parent = transform;
        
        if(cam == null) cam = Camera.current;
        grid = GetComponent<Grid>();
        if (grid == null)
        {
            grid = GetComponentInChildren<Grid>();
        }
        
        lastPosition = grid.WorldToCell(Vector3.zero);
        
        GridVisualization.SetActive(buildMode);

        int Buildx = (int)(-BottomLeftCornerXY.x + TopRightCornerXY.x);
        int Buildy = (int)(-BottomLeftCornerXY.x + TopRightCornerXY.x);
        SpawnList = new Buildable[Buildx,Buildy];
        WallList = new Wall[Buildx, Buildy];

        Rotation = Quaternion.identity;
        
        
        //Spawn Walls at Start
        if (spawnWallsOnStart && defaultWalls != null)
        {
            //get all edges and spawn walls
            for (int iy = 0; iy < WallList.GetLength(1); iy++)
            {
                for (int ix = 0; ix < WallList.GetLength(0); ix++)
                {
                    int x = ix + (int) + BottomLeftCornerXY.x;
                    int y = iy + (int) + BottomLeftCornerXY.y;

                    //Skip if Spawn Door Location
                    if (DoorLocation.x == x && DoorLocation.y == y)
                    {
                        //Debug.Log("Creating Door Hole @ (" + x + ", " + y + ")");
                        RotationModifier = FireExitRotation;
                            
                        int rot = RotationModifier == 0 ? 0 :
                            RotationModifier == 1 ? 90 :
                            RotationModifier == 2 ? 180 :
                            RotationModifier == 3 ? 270 : 0;
                        Rotation = Quaternion.AngleAxis(rot, Vector3.up);

                        Vector3 pos = new Vector3(x, 0, y);
                        pos.y = offset.y;
                        pos.x += offset.x;
                        pos.z += offset.z;
                
                        GameObject spawned = Instantiate(fireExit.Spawnable, pos, 
                            Rotation, defaultWallHolder.transform);

                        WallList[ix,iy].isPlaced = true;
                        WallList[ix,iy].Item = spawned;
                        WallList[ix,iy].Rotation = RotationModifier;

                        
                    }
                    else
                    {
                        if (iy == 0 || iy == (WallList.GetLength(1) - 1))
                        {
                            //Debug.Log("Vert Walls - (" + x + ", " + y + ")");
                            
                            RotationModifier = iy == 0 ? 1 : 3;
                            
                            int rot = RotationModifier == 0 ? 0 :
                                RotationModifier == 1 ? 90 :
                                RotationModifier == 2 ? 180 :
                                RotationModifier == 3 ? 270 : 0;
                            Rotation = Quaternion.AngleAxis(rot, Vector3.up);

                            Vector3 pos = new Vector3(x, 0, y);
                            pos.y = offset.y;
                            pos.x += offset.x;
                            pos.z += offset.z;
                
                            GameObject spawned = Instantiate(defaultWalls.Spawnable, pos, 
                                Rotation, defaultWallHolder.transform);

                            WallList[ix,iy].isPlaced = true;
                            WallList[ix,iy].Item = spawned;
                            WallList[ix,iy].Rotation = RotationModifier;
                            
                            
                        }

                        if (ix == 0 || ix == (WallList.GetLength(0) - 1))
                        {
                            //Debug.Log("Horz Walls - (" + x + ", " + y + ")");
                            
                            RotationModifier = ix == 0 ? 2 : 4;
                            
                            int rot = RotationModifier == 0 ? 0 :
                                RotationModifier == 1 ? 90 :
                                RotationModifier == 2 ? 180 :
                                RotationModifier == 3 ? 270 : 0;
                            Rotation = Quaternion.AngleAxis(rot, Vector3.up);

                            Vector3 pos = new Vector3(x, 0, y);
                            pos.y = offset.y;
                            pos.x += offset.x;
                            pos.z += offset.z;
                
                            GameObject spawned = Instantiate(defaultWalls.Spawnable, pos, 
                                Rotation, defaultWallHolder.transform);

                            WallList[ix,iy].isPlaced = true;
                            WallList[ix,iy].Item = spawned;
                            WallList[ix,iy].Rotation = RotationModifier;
                        }
                    }
                }
            }
        }
        
        //reset rotation
        RotationModifier = 0;

        int finalRot = RotationModifier == 0 ? 0 :
            RotationModifier == 1 ? 90 :
            RotationModifier == 2 ? 180 :
            RotationModifier == 3 ? 270 : 0;
        Rotation = Quaternion.AngleAxis(finalRot, Vector3.up);
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
            GlobalGameState.getGameState().isBuildMode = buildMode;
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

            if (GlobalGameState.getGameState().currentPlaceable != Spawnable)
            {
                if(Vis != null) Destroy(Vis);
                
                Spawnable = GlobalGameState.getGameState().currentPlaceable;
                
                Vector3 pos = lastPosition;
                pos.y = offset.y;
                pos.x += offset.x;
                pos.z += offset.z;
                
                if (Spawnable == null || Spawnable.Spawnable == null) return;

                Vis = Instantiate(Spawnable.Spawnable, pos,
                    Rotation, placeableHolder.transform);

                Destroy(Vis.GetComponent<NavMeshObstacle>());
                Destroy(Vis.GetComponent<Collider>());
                
                Destroy(Vis.GetComponentInChildren<NavMeshObstacle>());
                Destroy(Vis.GetComponentInChildren<Collider>());
                
                Destroy(Vis.GetComponentInChildren<HingeJoint>());
                Destroy(Vis.GetComponentInChildren<Rigidbody>());
            }
            
            if (Vis == null)
            {
                if (Spawnable == null || Spawnable.Spawnable == null) return;
                
                Vector3 pos = lastPosition;
                pos.y = offset.y;
                pos.x += offset.x;
                pos.z += offset.z;

                Vis = Instantiate(Spawnable.Spawnable, pos,
                    Rotation, placeableHolder.transform);

                Destroy(Vis.GetComponent<NavMeshObstacle>());
                Destroy(Vis.GetComponent<Collider>());
                
                Destroy(Vis.GetComponentInChildren<NavMeshObstacle>());
                Destroy(Vis.GetComponentInChildren<Collider>());
                
                Destroy(Vis.GetComponentInChildren<HingeJoint>());
                Destroy(Vis.GetComponentInChildren<Rigidbody>());
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
                int cMoney = GlobalGameState.getGameState().getMoney();
                if (cMoney < Spawnable.Cost)
                {
                    //Debug.Log("You Are Broke");
                    return;
                }
                
                Vector3 pos = GetPositionOnGrid();
                //Debug.Log(pos);
                if (pos == new Vector3(-9999, -9999, -9999)) return;
                
                //Debug.Log(pos);
                
                int x = (int) pos.x + (int) -BottomLeftCornerXY.x;
                int y = (int) pos.z + (int) -BottomLeftCornerXY.y;
                
                //Debug.Log(x + ", " + y);
                
                Debug.Log("Placing @ " + x + ", " + y + " > " + RotationModifier);

                //Debug.Log("Placing Something!");
                if (Spawnable.isWall)
                {
                    //Debug.Log("Wall!");
                    if (!checkIfWallPlaced(x,y,RotationModifier))
                    {
                        pos.y = offset.y;
                        pos.x += offset.x;
                        pos.z += offset.z;
            
                        GameObject spawned = Instantiate(Spawnable.Spawnable, pos, 
                            Rotation, placeableHolder.transform);

                        WallList[x,y].isPlaced = true;
                        WallList[x,y].Item = spawned;
                        WallList[x, y].Rotation = RotationModifier;
                    }
                }
                else
                {
                    //Debug.Log("not Wall!");
                    if (!SpawnList[x,y].isPlaced)
                    {
                        pos.y = offset.y;
                        pos.x += offset.x;
                        pos.z += offset.z;
            
                        GameObject spawned = Instantiate(Spawnable.Spawnable, pos, 
                            Rotation, placeableHolder.transform);

                        SpawnList[x,y].isPlaced = true;
                        SpawnList[x,y].Item = spawned;
                    }
                }
            }
            
            if (Input.GetMouseButton(1))
            {
                //Destroy Currently Selected
                DestroyAtMouseLoc();
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
        
        if(EventSystem.current.IsPointerOverGameObject()) return new Vector3(-9999,-9999,-9999); 

        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 99999, GridMask))
        {
           lastPosition = grid.WorldToCell(hit.point);;
           //Debug.DrawLine(cam.transform.position, hit.point, Color.red);
           return lastPosition;
        }

        return new Vector3(-9999,-9999,-9999);
    }

    public void DestroyAtMouseLoc()
    {
        Debug.Log("Destroying A Wall or Placeable!");
        
        if (Vis != null)
        {
            Destroy(Vis);
        }
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = cam.nearClipPlane;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 99999, Wall))
        {
            //Destroy This Object [WALL]
            
            GameObject hitObj = hit.transform.GetComponentInParent<PlaceableHolder>().gameObject;
            if (hitObj == null) hitObj = hit.transform.GetComponentInChildren<PlaceableHolder>().gameObject;

            if (hitObj.GetComponent<PlaceableHolder>().PlaceableData.isDestructible == false) return;

            Vector3 loc = hitObj.GetComponent<PlaceableHolder>().transform.position;
            //REVERSE OF PREVIOUS
            loc.y = offset.y;
            loc.x -= offset.x;
            loc.z -= offset.z;
            
            loc.x += -BottomLeftCornerXY.x;
            loc.z += -BottomLeftCornerXY.y;
            
            Debug.Log("Destroying @ " + loc.x + ", " + loc.z);
            if(isBorderWall((int) loc.x,(int) loc.z)) return;
            
            
            Debug.Log("[w] Destroying: " + hitObj.name);
            Destroy(hitObj);
            

            WallList[(int) loc.x,(int) loc.z].isPlaced = false;
            WallList[(int) loc.x,(int) loc.z].Item = null;
            
            return;
        }
        
        if (Physics.Raycast(ray, out hit, 99999, Placeable))
        {
            GameObject hitObj = hit.transform.GetComponentInParent<PlaceableHolder>().gameObject;
            if (hitObj == null) hitObj = hit.transform.GetComponentInChildren<PlaceableHolder>().gameObject;
            
            if (hitObj.GetComponent<PlaceableHolder>().PlaceableData.isDestructible == false) return;
            
            Vector3 loc = hitObj.GetComponent<PlaceableHolder>().transform.position;
            //REVERSE OF PREVIOUS
            loc.y = offset.y;
            loc.x -= offset.x;
            loc.z -= offset.z;
            
            Debug.Log("[p] Destroying: " + hitObj.name);
            Destroy(hitObj);
            
            //REVERSE OF PREVIOUS
            loc.y = offset.y;
            loc.x -= offset.x;
            loc.z -= offset.z;
            
            loc.x += -BottomLeftCornerXY.x;
            loc.z += -BottomLeftCornerXY.y;

            SpawnList[(int) loc.x,(int) loc.z].isPlaced = false;
            SpawnList[(int) loc.x,(int) loc.z].Item = null;
            
            return;
        }
    }

    public bool isBorderWall(int x, int y)
    {
        //y == 0 OR y == max
        // x can by any number
        
        //x == 0 or x == max
        //y can be any number
        
        int ry = y == 0 ? 1 : 3;
        int rx = x == 0 ? 2 : 4;

        if (y == 0 || y == WallList.GetLength(1) - 1)
        {
            if (WallList[x, y].Rotation == ry) return true;
        }
        if (x == 0 || x == WallList.GetLength(0) - 1)
        {
            if (WallList[x, y].Rotation == rx) return true;
        }
        
        return false;
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
 
        //Debug.DrawLine(point1, point2, c);
        //Debug.DrawLine(point2, point3, c);
        //Debug.DrawLine(point3, point4, c);
        //Debug.DrawLine(point4, point1, c);
 
        //Debug.DrawLine(point5, point6, c);
        //Debug.DrawLine(point6, point7, c);
        //Debug.DrawLine(point7, point8, c);
        //Debug.DrawLine(point8, point5, c);
 
        //Debug.DrawLine(point1, point5, c);
        //Debug.DrawLine(point2, point6, c);
        //Debug.DrawLine(point3, point7, c);
        //Debug.DrawLine(point4, point8, c);
 
        // optional axis display
        //Debug.DrawRay(m.GetPosition(), m.GetForward(), Color.magenta);
        //Debug.DrawRay(m.GetPosition(), m.GetUp(), Color.yellow);
        //Debug.DrawRay(m.GetPosition(), m.GetRight(), Color.red);
    }
}
