using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    private bool isOpened = false;

    [SerializeField] private GameObject Target;
    
    // Start is called before the first frame update
    void Start()
    {
        Target.SetActive(isOpened);
    }

    public void Hide()
    {
        isOpened = false;
        Target.SetActive(isOpened);
    }

    private void OnMouseOver()
    {
        //Enables if Right Click Object
        if (Input.GetMouseButtonDown(1))
        {
            isOpened = !isOpened;
            
            //Set HUD Enabled or Not
            Target.SetActive(isOpened);
            
            //Hide All Others
            foreach (OpenMenu menu in FindObjectsOfType<OpenMenu>())
            {
                if (menu != this)
                {
                    menu.Hide();
                }
            }
        }
    }
}
