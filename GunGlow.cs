using UnityEngine;
using UnityEngine.Android;

public class GunGlow : MonoBehaviour
{
    public Renderer[] meshrenderers;
    public Color glowcolor;
    public float glowIntensity;
    public float glowspeed;


    private Material[] materials;
    private bool isglowing = true;


    void Start()
    {
        materials = new Material[meshrenderers.Length];
        for(int i = 0; i < meshrenderers.Length; i++)
        {
            materials[i] = meshrenderers[i].material;
        }
        foreach(Material mat in materials)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }
    void Update()
    {
        if(isglowing)
        {
            float emission = Mathf.PingPong(Time.time * glowspeed,glowIntensity);
            foreach(Material mat in materials)
            {
                mat.SetColor("_EmissionColor" ,glowcolor*emission);
            }
        }
        else
        {
            foreach (Material mat in materials)
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
        
    }
    public void enableglow()
    {
        isglowing=true;
    }
    public void disableglow()
    {
        isglowing = false;
    }
}
