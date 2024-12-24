using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class climbinteractable : XRBaseInteractable
{
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (interactor is XRDirectInteractor)
        {
            Climber.climbinghand = interactor.GetComponent<XRController>();
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);

        if (Climber.climbinghand != null && Climber.climbinghand.name == interactor.name)
        {
            Climber.climbinghand = null;
        }
    }
}
