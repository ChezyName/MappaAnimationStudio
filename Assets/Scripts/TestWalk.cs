using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestWalk : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    
    [SerializeField]
    private LayerMask GridMask;

    [SerializeField] private NavMeshAgent Agent;
    
    // Start is called before the first frame update
    void Start()
    {
        if(cam == null) cam = Camera.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            if (cam == null) cam = FindFirstObjectByType<Camera>();
            
            mousePosition.z = cam.nearClipPlane;

            Ray ray = cam.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 99999, GridMask))
            {
                Agent.destination = hit.point;
            }
        }
    }
}
