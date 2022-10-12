using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePreFabe : MonoBehaviour
{
    //public GameObject PrefabeOld;
    public GameObject PreFabeNew;
    public string StartsWithName = "";

    [ContextMenu("change")]
    public void Change()
    {
        foreach (Transform child in transform)
        {
           var to = child;
            if (to.name.StartsWith(StartsWithName) == false) continue;
         var ne=    Instantiate(PreFabeNew, to.position, to.rotation);
            ne.name = PreFabeNew.name;
            ne.transform.parent = transform;
            DestroyImmediate(to.gameObject);
        }
    }
}
