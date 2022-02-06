using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    [SerializeField] private List<Transform> nesneler = new List<Transform>();

    public static Action<Transform> Check;

    private void OnEnable()
    {
        Check += CheckThis;
    }

    private void OnDisable()
    {
        Check -= CheckThis;
    }

    private void Start()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.layer = 3;
            nesneler.Add(t);
        }
    }

    private void CheckThis(Transform tt)
    {
        nesneler.Remove(tt);

        if (nesneler.Count == 0)
        {
            GameBase.SuccesefulFinishGame.Invoke();
        }
    }
}
