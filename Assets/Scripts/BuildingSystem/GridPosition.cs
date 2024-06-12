using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Grid grid;

    [SerializeField]
    private LayerMask GridMask;
    
    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.current;
        grid = GetComponent<Grid>();
        lastPosition = grid.WorldToCell(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        GetPositionOnGrid();
        DrawBox(lastPosition, Quaternion.identity,
            new Vector3(1, 1, 1), Color.yellow);
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
            if(Input.GetMouseButton(0)) lastPosition = grid.WorldToCell(hit.point);;
            
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
