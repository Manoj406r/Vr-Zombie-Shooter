using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TorchGrab : MonoBehaviour
{

    [Header("Torch Componenets")]
    public Light spotlight;
    public MeshRenderer renderer;
    public string emissionkeyword = "_EMISSION";
    private bool isTorchon = false;



    [Header("Hand Grab pose")]
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);
        grabInteractable.selectEntered.AddListener(x => ToggleTorchlight());
        grabInteractable.selectExited.AddListener(x =>ToggleTorchlight());

        rightHandPose.gameObject.SetActive(false);
        setTorchState(false);
    }



    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            SetHandDataValues(handData, rightHandPose);
            SendHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
            
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            SendHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
            
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
            h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
            h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h1.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SendHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
    private void ToggleTorchlight()
    {
        isTorchon = !isTorchon;
        setTorchState(isTorchon);
    }
    private void setTorchState(bool state)
    {
        if(spotlight != null)
        {
            spotlight.enabled = state;
        }
        if(renderer != null && renderer.material.HasProperty(emissionkeyword))
        {
            if(state)
            {
                renderer.material.EnableKeyword(emissionkeyword);
            }
            else
            {
                renderer.material.DisableKeyword(emissionkeyword);
            }
        }
    }
}
