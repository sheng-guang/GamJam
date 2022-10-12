using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RevertablePlayerMovementControl : Revertable
{
    [SerializeField]
    public ReversedMovementContainer record = new ReversedMovementContainer();
    public Animator myAnimator;

    [SerializeField]
    public ReversedMovementContainer newRecord = new ReversedMovementContainer();

    public Animator NonRevertPlayer;

    private Vector3 lastFramePos;
    private float lastV;


    protected override void Start()
    {
        base.Start();
        record = h.readMovementFromFile("123.txt");
        record.Init();
        lastFramePos = this.transform.position;
        lastV = 0;
        //RevertSceneManager.instance.SubscribeList.Add(this);
        //InRevertSpace = false;
    }


    public override void RevertUpdate()
    {
        base.RevertUpdate();
        if (RevertSceneManager.instance.PauseRecording) { this.transform.position = Vector3.up * -1000; }
        float TimePassed = Time.time - RevertSceneManager.instance.timeStamp;
        ReversedMovementContainer.TimeStampedState TSS = record.GetStateAtTime(TimePassed);
        this.transform.position = TSS.PlayerCenterPos.toVector3();
        this.transform.eulerAngles = TSS.PlayerEulerAngle.toVector3();
        myAnimator.Play(TSS.PlayerAnimationState, 1, TSS.PlayerStateNormalizedTime);
        myAnimator.speed = 0;
        myAnimator.SetFloat("v", Mathf.Lerp(lastV, (lastFramePos - this.transform.position).magnitude / Time.deltaTime, 0.02f));
        lastV = Mathf.Lerp(lastV, (lastFramePos - this.transform.position).magnitude / Time.deltaTime, 0.02f);
        lastFramePos = this.transform.position;
        RecordPositivePlayer(TimePassed);
        //h.l(newRecord.ToString()) ;
        if (Input.GetKeyUp(KeyCode.K))
        {
            h.writeMovementToFile("123.txt", newRecord);
            h.l("WRITTEN");
        }
    }

    public override void PositiveUpdate()
    {
        base.PositiveUpdate();
        RevertUpdate();

    }

    private void RecordPositivePlayer(float TimePassed)
    {
        if (RevertSceneManager.instance.PauseRecording) { return; }

        ReversedMovementContainer.TimeStampedState TSSNonRevert = 
            new ReversedMovementContainer.TimeStampedState(
                NonRevertPlayer.transform.position, 
                NonRevertPlayer.transform.eulerAngles, 
                h.GetStateName(Constants.AllPossibleStateNames, NonRevertPlayer.GetCurrentAnimatorStateInfo(1)),
                NonRevertPlayer.GetCurrentAnimatorStateInfo(1).normalizedTime, 
                TimePassed,
                NonRevertPlayer.GetFloat("v")
                );
        //record.PushStateAtTime(TSSNonRevert);
        newRecord.PushStateAtTime(TSSNonRevert);
    }

    public override void Revert()
    {
        record = newRecord;
        Respawned();
        this.NotInRevertSpace = !RevertSceneManager.instance.PositiveTimeFlowing;
        revertPlaying = true;
    }

    public override void Respawned()
    {
        record.Init();
        newRecord = new ReversedMovementContainer();
    }
}
