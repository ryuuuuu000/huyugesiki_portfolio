using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitychan_face : MonoBehaviour
{
    [SerializeField] AnimationClip[] faceanime;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetLayerWeight(1, 1f);
    }
}
