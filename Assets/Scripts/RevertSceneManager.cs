using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertSceneManager : MonoSingleton<RevertSceneManager>
{
    public bool PositiveTimeFlowing = true;

    protected List<Revertable> subscribeList = new List<Revertable>();

    public float timeStamp = 0;

    public List<Revertable> SubscribeList { get => subscribeList; }

    public bool PauseRecording = false;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            ChangeTimeDirection();
        }
    }

    public void ChangeTimeDirection()
    {
        PositiveTimeFlowing = !PositiveTimeFlowing;
        timeStamp = Time.time;
        foreach (Revertable rev in SubscribeList)
        {
            rev.Revert();
        }
    }

    public void PlayerRespawned()
    {
        timeStamp = Time.time;
        foreach (Revertable rev in SubscribeList)
        {
            rev.Respawned();
        }
    }
    protected override void DisInit()
    {
        // IDK probably nothing to do
    }

    protected override void Init()
    {
        // IDK probably nothing to do also
    }
}
