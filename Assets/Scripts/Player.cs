using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action SutGol;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SutGol += SutAnim;
    }

    private void OnDisable()
    {
        SutGol -= SutAnim;
    }

    private void SutAnim()
    {
        anim.SetTrigger("Kick");
    }
}
