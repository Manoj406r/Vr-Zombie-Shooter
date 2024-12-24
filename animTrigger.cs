using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animTrigger1 : MonoBehaviour
{
    public Animator anim;
    public string Trigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetTrigger(Trigger);
    }
}
