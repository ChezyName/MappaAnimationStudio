using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Min X & Y Position Camera Can Move To
    [SerializeField]
    private Vector2 MinCameraClamp = new Vector2(-250, -250);
    
    //Max X & Y Position Camera Can Move To
    [SerializeField] private Vector2 MaxCameraClamp = new Vector2(250, 250);

    //Camera Speed (Movement Speed)
    [SerializeField]
    private float CameraSpeed = 12;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;

        pos += new Vector3(
            x * Time.deltaTime * CameraSpeed,
            0,
            y * Time.deltaTime * CameraSpeed
        );
        
        pos.x = Math.Clamp(pos.x, MinCameraClamp.x, MaxCameraClamp.x);
        pos.z = Math.Clamp(pos.z, MinCameraClamp.y, MaxCameraClamp.y);
        pos.y = 0;
        
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 startingLoc = transform.position;
        Vector3 topLeft = new Vector3(MinCameraClamp.x, 0, MaxCameraClamp.y) + startingLoc;
        Vector3 topRight = new Vector3(MaxCameraClamp.x, 0, MaxCameraClamp.y) + startingLoc;
        
        Vector3 bottomLeft = new Vector3(MinCameraClamp.x, 0, MinCameraClamp.y) + startingLoc;
        Vector3 bottomRight = new Vector3(MaxCameraClamp.x, 0, MinCameraClamp.y) + startingLoc;
        
        Gizmos.DrawLine(topLeft,topRight);
        Gizmos.DrawLine(topRight,bottomRight);
        Gizmos.DrawLine(bottomRight,bottomLeft);
        Gizmos.DrawLine(bottomLeft,topLeft);
    }
}
