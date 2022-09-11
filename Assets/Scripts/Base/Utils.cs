using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils
{
    public static GameObject GetChildWithName(this GameObject obj, string name) => obj.transform.Find(name)?.gameObject;

    public static GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;

        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }

        return child;
    }

    public static int CalculateSiblingIndex(int x, int y)
    {
        return x * Constants.BOARD_WIDTH + y;
    }

    public static (int row, int col) GetRowAndColIndex(int siblingIndex)
    {
        return (siblingIndex / Constants.BOARD_WIDTH, siblingIndex % Constants.BOARD_WIDTH);
    }
}
