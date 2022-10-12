using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLand : MonoBehaviour
{
    public bool onLand;
    HashSet<Land> lands=new HashSet<Land>();
    void OnTriggerEnter(Collider c)
    {
        var l = c.GetComponent<Land>();
        if (l == null) return;
        if (lands.Contains(l)) return;
        lands.Add(l);
        onLand = lands.Count != 0;
    }
    void OnTriggerExit(Collider c)
    {
        var l = c.GetComponent<Land>();
        if (l == null) return;
        lands.Remove(l);
        onLand = lands.Count != 0;
    }
    
}
