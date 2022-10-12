using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Todo1 : DoSomeThing
{
    public override void Do()
    {
        transform.eulerAngles += Vector3.one * 10;
    }
}
