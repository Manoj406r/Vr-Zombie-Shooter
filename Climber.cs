using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Climber : MonoBehaviour
{
    private CharacterController characterController;
    public static XRController climbinghand; // Use XRController from XR Interaction Toolkit
    public DynamicMoveProvider dynamicMoveProvider;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (dynamicMoveProvider == null)
        {
            Debug.LogError("DynamicMoveProvider is not assigned. Please assign it in the Inspector.");
        }
    }

    private void FixedUpdate()
    {
        if (climbinghand != null)
        {
            dynamicMoveProvider.enabled = false;
            Climb();
        }
        else
        {
            dynamicMoveProvider.enabled = true;
        }
    }

    void Climb()
    {
        if (climbinghand != null && climbinghand.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
        {
            characterController.Move(-velocity * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogWarning("Failed to get velocity from the XRController.");
        }
    }
}
