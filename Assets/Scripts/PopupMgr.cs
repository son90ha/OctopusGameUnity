using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PopupMgr
{
    public static T CreatePopup<T>(string path, Transform parent) where T : MonoBehaviour
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (!prefab)
        {
            Debug.LogError("[PopupMgr] CANNOT find prefab in resources path: " + path);
        }
        GameObject newGameObj = Object.Instantiate(prefab, parent);
        return newGameObj.GetComponent<T>();
    }
}
