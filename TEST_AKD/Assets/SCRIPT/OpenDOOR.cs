using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDOOR : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float distance;
    [SerializeField] private GameObject cam;

    void Start()
    {   
        anim = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if(Physics.Raycast(ray, out hit, distance, layer))
        {   
            if(Input.GetKeyDown(KeyCode.E))
            {
                anim.SetBool("IsOpen",true);
            }
            

        }
        
    }
}
