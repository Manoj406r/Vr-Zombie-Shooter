using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    public enum handmodeltype { left, right};
    public handmodeltype handmodel;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}
