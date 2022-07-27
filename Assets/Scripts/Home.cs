using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject happyFrog;

    void Awake() 
    {
        enabled = false;    
    }

    private void OnEnable()
    {
        happyFrog.SetActive(true);
    }

    private void OnDisable()
    {
        happyFrog.SetActive(false);
    }
   
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Home is triggered");

        if(other.tag == "Player") {
            enabled = true;
            FindObjectOfType<Manager>().HomeOccupied();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
