using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    
    public bool isOpen;
    public Animator animator;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openObject(){

        if(!isOpen){
            isOpen = true;
            Debug.Log("Object are get interact");
            
            isOpen = false;
        }
    }
}
