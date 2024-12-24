using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyattach : MonoBehaviour
{
    public Transform attachpoint;
    private bool isattached = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("key") && !isattached)
        {
            other.transform.position = attachpoint.position;
            other.transform.rotation = attachpoint.rotation;
            isattached = true;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
