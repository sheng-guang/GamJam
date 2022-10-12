using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertableBlockades : Revertable
    {
    public ReversedMovementContainer record = new ReversedMovementContainer();
    public ReversedMovementContainer NewRecord = new ReversedMovementContainer();
    public Animator myAnimator;

    public string preRecordFileName;

    private Vector3 OrigPos;
    private Vector3 OrigRot;

   

    private void Start()
    {
        OrigPos = this.transform.position;
        OrigRot = this.transform.eulerAngles;
        if (preRecordFileName != "")
        {
            record = h.readMovementFromFile(preRecordFileName);
            record.Init();
        }
        else
        {
            record = new ReversedMovementContainer();
        }
        RevertSceneManager.instance.SubscribeList.Add(this);
    }
    public override void Revert()
    {
        record = NewRecord;
        record.Init();
        NewRecord = new ReversedMovementContainer();
    }

    public override void RevertUpdate()
    {
        base.RevertUpdate();
        float TimePassed = Time.time - RevertSceneManager.instance.timeStamp;
        ReversedMovementContainer.TimeStampedState TSS = record.GetStateAtTime(TimePassed);
        if(record.record.Count > 3)
        {
            this.transform.position = TSS.PlayerCenterPos.toVector3();
            this.transform.eulerAngles = TSS.PlayerEulerAngle.toVector3();
            this.gameObject.transform.position = TSS.PlayerCenterPos.toVector3();
            this.gameObject.transform.eulerAngles = TSS.PlayerEulerAngle.toVector3();
            if (myAnimator)
            {
                myAnimator.Play(TSS.PlayerAnimationState, 1, TSS.PlayerStateNormalizedTime);
                myAnimator.speed = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.K) && preRecordFileName != "")
        {
            h.writeMovementToFile(preRecordFileName, record);
            h.l("WRITTEN IN" + preRecordFileName);
        }
    }

    public override void PositiveUpdate()
    {
        base.PositiveUpdate();
        if (myAnimator)
        { 
            myAnimator.speed = 1;
        }
        float TimePassed = Time.time - RevertSceneManager.instance.timeStamp;
        //h.l(TimePassed);
        RecordPositivePlayer(TimePassed);
    }

    private void RecordPositivePlayer(float TimePassed)
    {
        if(this.gameObject.transform.position == Vector3.zero)
        {
            return;
        }
        string stateName = "";
        float time = 0;
        if (myAnimator)
        {
            stateName = h.GetStateName(Constants.AllPossibleStateNames, myAnimator.GetCurrentAnimatorStateInfo(1));
            time = myAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        }
            // TODO: store animation state and stuff
            ReversedMovementContainer.TimeStampedState TSSNonRevert =
            new ReversedMovementContainer.TimeStampedState(
                this.transform.position,
                this.transform.eulerAngles,
               stateName,
                time,
                TimePassed, 0);
        NewRecord.PushStateAtTime(TSSNonRevert);
        //h.l("HEY I AM RECORDING!" + TSSNonRevert.ToString() + gameObject.name);
    }

    public override void Respawned()
    {
        if (revertPlaying)
        {
            record.Init();
        }
        else
        {
            record = new ReversedMovementContainer();
        }
    }
}

