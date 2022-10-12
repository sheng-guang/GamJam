using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastDoorController : DoSomeThing
{
    public Animator ann;
    public bool ok = false;
    public RevertableBlockades Blockade;
    public override void Do()
    {
        ok = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ok)
        {
            ann.enabled = true;
        }
        if (Blockade.RevertPlaying)
        {
            ann.enabled = false;
        }
    }
}
