using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class KeyLockIntegration : MonoBehaviour
{
    public Transform keypivot;
    public Animator lockanim;
    public Animator chestanim;
    public float requiredRotation = 90f;
    public XRGrabInteractable keyinteractable;

    private bool iskeyAttached = false;
    private bool iskeyUnLocked = false;

    private Quaternion initailRotation;
    private Transform grabbingHand;
    private float currentRotation = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        if(keyinteractable != null)
        {
            keyinteractable.selectEntered.AddListener(OnGrabbed);
            keyinteractable.selectExited.AddListener(Onreleased);
        }
    }
    private void OnDestroy()
    {
        if (keyinteractable != null)
        {
            keyinteractable.selectEntered.RemoveListener(OnGrabbed);
            keyinteractable.selectExited.RemoveListener(Onreleased);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(!iskeyAttached  && grabbingHand != null && !iskeyUnLocked)
        {
            Quaternion rotationdelta = Quaternion.Inverse(initailRotation) * grabbingHand.rotation;
            float angleDelta = rotationdelta.eulerAngles.y;
            keypivot.Rotate(0,angleDelta,0);

            currentRotation += angleDelta;

            initailRotation = grabbingHand.rotation;

            if(Mathf.Abs(currentRotation) >= requiredRotation)
            {
                iskeyUnLocked = true;
                lockanim.SetTrigger("open");
                Debug.Log("key Unlocked");
                chestanim.SetTrigger("Chest");

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Key") && !iskeyAttached)
        {
            Attachkey();
        }
    }
    private void Attachkey()
    {
        iskeyAttached = true;
        keyinteractable.transform.position = keypivot.position;
        keyinteractable.transform.rotation = keypivot.rotation;
        keyinteractable.enabled = true;
    }
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        grabbingHand = args.interactorObject.transform;
        initailRotation = grabbingHand.rotation;
    }
    private void Onreleased(SelectExitEventArgs args)
    {
        grabbingHand = null;
    }
}
