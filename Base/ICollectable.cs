using System.Runtime.InteropServices;
using UnityEngine;

interface ICollectable
{
    void Effect([Optional]Vector3 pos);
    void Contact(GameObject target);
    
}
