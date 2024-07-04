using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEmployeeColoring : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
        {
            // 3 = eyes
            // 2 = hair
            // 0 = shirt

            Random.InitState(System.DateTime.Now.Millisecond);
            renderer.materials[0].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
            renderer.materials[3].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
            renderer.materials[2].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        }
    }
}
