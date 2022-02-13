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
            nesneler.Add(t);
            t.gameObject.layer = 3;

            if (t.TryGetComponent<Collider>(out var rb))
            {
                rb.isTrigger = true;
            }
            else
            {
                rb = t.gameObject.AddComponent<BoxCollider>();
                rb.isTrigger = true;
            }

            if (!t.TryGetComponent<CanBrekable>(out var can))
            {
                can = t.gameObject.AddComponent<CanBrekable>();
                var outline = t.gameObject.AddComponent<Outline>();
                outline.OutlineColor = Color.red;
                outline.OutlineWidth = 3;
            }





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
