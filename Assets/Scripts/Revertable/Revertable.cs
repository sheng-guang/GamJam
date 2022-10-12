using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Revertable : MonoBehaviour
{
    protected bool revertPlaying = false;

    public bool NotInRevertSpace = false;

    public bool RevertPlaying { get => revertPlaying;  }

    public abstract void Revert();
    //{
    //    //revertPlaying = !revertPlaying;
    //}

    public abstract void Respawned();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        RevertSceneManager.instance.SubscribeList.Add(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        revertPlaying = (RevertSceneManager.instance.PositiveTimeFlowing != this.NotInRevertSpace);
        if (revertPlaying)
        {
            RevertUpdate();
        }
        else
        {
            PositiveUpdate();
        }
    }
    public virtual void RevertUpdate() { }
    public virtual void PositiveUpdate() { }

    protected virtual void FixedUpdate()
    {
        revertPlaying = RevertSceneManager.instance.PositiveTimeFlowing != this.NotInRevertSpace;
        if (revertPlaying)
        {
            RevertFixedUpdate();
        }
        else
        {
            PositiveFixedUpdate();
        }
    }
    public virtual void RevertFixedUpdate() { }
    public virtual void PositiveFixedUpdate() { }
}
