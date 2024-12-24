using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shotgungrab : MonoBehaviour
{
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    bool isholding = false;
    public GameObject bulletprefab;
    public Transform bulletSpawnPoint;
    public float bulletspeed;

    private bool isreloading = false;
    private bool canShoot = true;
    public float shootCooldown = 0f;

    public AudioSource reloadsound;
    public AudioSource shootSound;
    public ParticleSystem muzzleFlash;

    private int shotcount = 0;
    private int maxshotgunreload = 30;
    //public Animator anim;
    public GunGlow GunGlow;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);
        grabInteractable.activated.AddListener(x => shoot());
        grabInteractable.deactivated.AddListener(x => shoot());

        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            SetHandDataValues(handData, rightHandPose);
            SendHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
            isholding = true;

            GunGlow.disableglow();
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            SendHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
            isholding = false;
            GunGlow.enableglow();
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

    public void shoot()
    {
        if (isholding && canShoot && !isreloading)
        {
            if (shotcount >= maxshotgunreload)
            {
                StartCoroutine(reloading());
                return;
            }
            canShoot = false;


            GameObject bulletref = Instantiate(bulletprefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bulletref.transform.Rotate(0, 90, -90);
            bulletref.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletspeed;
            Destroy(bulletref, 5f);

            // Play the shoot sound
            if (shootSound != null)
            {
                shootSound.Play();
            }


            if (muzzleFlash != null)
            {

                muzzleFlash.Play();
            }
            shotcount++;

            StartCoroutine(ShootCooldown());
        }
    }
    private IEnumerator reloading()
    {
        if (reloadsound != null)
        {
            reloadsound.Play();
        }
        //anim.SetBool("Loading", true);
        Debug.Log("reload sound play");
        isreloading = true;
        yield return new WaitForSeconds(reloadsound.clip.length);
        //anim.SetBool("Loading", false);
        shotcount = 0;
        isreloading = false;

    }


    private IEnumerator ShootCooldown()
    {

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
