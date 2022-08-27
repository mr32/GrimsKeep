using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static GameObject GetChildWithName(this GameObject obj, string name) => obj.transform.Find(name)?.gameObject;

}
