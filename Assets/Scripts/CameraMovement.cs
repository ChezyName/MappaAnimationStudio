using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    private Vector3 startingPos;
    
    private CinemachineVirtualCamera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float Zoom = Input.GetAxisRaw("Mouse ScrollWheel");
        float r = Input.GetAxis("QE");

        Vector3 pos = new Vector3(
            x * Time.deltaTime * CameraSpeed,
            0,
            y * Time.deltaTime * CameraSpeed
        );
        
        pos.x = Math.Clamp(pos.x, MinCameraClamp.x, MaxCameraClamp.x);
        pos.z = Math.Clamp(pos.z, MinCameraClamp.y, MaxCameraClamp.y);
        pos.y = Math.Clamp(pos.y, -5, 15);
        
        transform.Translate(pos);
        transform.Rotate(Vector3.up,-r * (CameraSpeed*4) * Time.deltaTime);

        cam.m_Lens.FieldOfView = Math.Clamp(
            cam.m_Lens.FieldOfView + -Zoom * CameraSpeed * Time.deltaTime * 150,
            20,
            60
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 startingLoc = startingPos;
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
