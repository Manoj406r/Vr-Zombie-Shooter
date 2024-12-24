using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject AttachPanel;
    public GameObject NextPanel;
    public float AudioTime;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= AudioTime)
        {
            if(NextPanel)
            {
                NextPanel.SetActive(true);
            }
           
            if(AttachPanel)
            {
                AttachPanel.SetActive(false);
            }
            
        }
        
    }

}
