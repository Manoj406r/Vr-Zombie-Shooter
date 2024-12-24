using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyenable : MonoBehaviour
{
    public GameObject[] magiceffects;
    public GameObject transsheep;
    public GameObject Grabsheep;
    public GameObject anim;
    public GameObject neweffect;
    public GameObject key;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("skull"))
        {
            foreach(GameObject go in magiceffects)
            {
                go.SetActive(false);
            }
            transsheep.SetActive(true);
            Grabsheep.SetActive(false);
            StartCoroutine(animenable());
            StartCoroutine(keynable());
            
        }
    }
    IEnumerator animenable()
    {
        yield return new WaitForSeconds(3f);
        anim.SetActive(true);
        neweffect.SetActive(true);

    }
    IEnumerator keynable()
    {
        yield return new WaitForSeconds(27f);
        key.SetActive(true);
        this.gameObject.SetActive(false);

    }
}
