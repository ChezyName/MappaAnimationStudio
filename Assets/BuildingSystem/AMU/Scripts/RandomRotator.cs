using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public Vector3 RotateAdd = new Vector3(1, 0, 0);
    public float SpinSpeed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(
            RotateAdd.x * SpinSpeed * Time.deltaTime,
            RotateAdd.y * SpinSpeed * Time.deltaTime,
            RotateAdd.z * SpinSpeed * Time.deltaTime);
    }
}
