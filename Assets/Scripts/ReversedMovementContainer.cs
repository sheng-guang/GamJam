using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReversedMovementContainer
{
    public List<TimeStampedState> record = new List<TimeStampedState>(65535);
    private int progressIndex = 0;
    private float recordingLength = 0;

    [System.Serializable]
    public struct TimeStampedState
    {
        public CVector3 PlayerCenterPos;
        public CVector3 PlayerEulerAngle;
        public string PlayerAnimationState;
        public float PlayerStateNormalizedTime;
        public float PlayerTimeStamp;
        public float v;

        public TimeStampedState(Vector3 playerpos, Vector3 playerangle, string animatestate, float stateNormalizedTime, float timeStamp, float V)
        {
            PlayerCenterPos = new CVector3(playerpos);
            PlayerEulerAngle = new CVector3(playerangle);
            PlayerAnimationState = animatestate;
            PlayerStateNormalizedTime = stateNormalizedTime;
            PlayerTimeStamp = timeStamp;
            v = V;

        }

        public override string ToString()
        {
            return string.Format("Pos = {0}, Angle = {1}, AnimationState = {2}, StateNormalizedTime = {3}, TimeStamp = {4}", PlayerCenterPos.toVector3().ToString(),
                PlayerEulerAngle.toVector3().ToString(), PlayerAnimationState, PlayerStateNormalizedTime.ToString(), PlayerTimeStamp.ToString());
        }

    }

    /// <summary>
    /// Return the averaged TimeStampedState from given timestamp
    /// Initialize first before calling for the first time!!!!!
    /// </summary>
    /// <param name="timeStamp"> A given timestamp</param>
    /// <returns></returns>
    public TimeStampedState GetStateAtTime(float timeStamp)
    {
        if (recordingLength == 0)
        {
            return new TimeStampedState(Vector3.up * -1000, Vector3.zero, "", 0, 0, 0) ;
        }
        timeStamp = recordingLength - timeStamp;
        while (progressIndex >= 0 && record[progressIndex].PlayerTimeStamp > timeStamp)
        {
            progressIndex -= 1;
        }
        if (progressIndex == -1)
        {
            return record[0];
        }
        TimeStampedState previousState = record[progressIndex];
        TimeStampedState nextTimeState = record[progressIndex + 1];
        float interpolant = Mathf.InverseLerp(previousState.PlayerTimeStamp, nextTimeState.PlayerTimeStamp, timeStamp);
        float v = Mathf.InverseLerp(previousState.v, nextTimeState.v, timeStamp);
        Vector3 LerpPlayerCenterPos = Vector3.Lerp(previousState.PlayerCenterPos.toVector3(), nextTimeState.PlayerCenterPos.toVector3(), interpolant);
        Vector3 LerpPlayerEulerAngle = Vector3.Lerp(previousState.PlayerEulerAngle.toVector3(), nextTimeState.PlayerEulerAngle.toVector3(), interpolant);
        string LerpPlayerAnimationState;
        float LerpPlayerStateNormalizedTime = Mathf.Lerp(previousState.PlayerStateNormalizedTime, nextTimeState.PlayerStateNormalizedTime, interpolant);
        if (interpolant < 0.5)
        {
            LerpPlayerAnimationState = previousState.PlayerAnimationState;
        }
        else
        {
            LerpPlayerAnimationState = nextTimeState.PlayerAnimationState;
        }
        return new TimeStampedState(LerpPlayerCenterPos, LerpPlayerEulerAngle, LerpPlayerAnimationState, LerpPlayerStateNormalizedTime, timeStamp, v);
    }

    /// <summary>
    /// Pushes a new player state into the list
    /// </summary>
    /// <param name="newState"></param>
    public void PushStateAtTime(TimeStampedState newState)
    {
        record.Add(newState);
    }

    /// <summary>
    /// Initialize or reset the container for movement requests after the recording finishes
    /// </summary>
    public void Init()
    {
        if (record.Count < 2)
        {
            return;
        }
        progressIndex = Math.Max(record.Count - 2, 0);
        recordingLength = record[progressIndex + 1].PlayerTimeStamp;
    }

    public override string ToString()
    {
        return record.Count.ToString();
        string output = string.Format("progressIndex = {0}, recordingLength = {1}\n", progressIndex.ToString(), recordingLength.ToString());
        for (int i = 0; i < record.Count; i++)
        {
            output += string.Format("ID = {0}, Content = [{1}]\n", i.ToString(), record[i].ToString());
        }
        return output;
    }

}

[System.Serializable]
public struct CVector3
{
    float x;
    float y;
    float z;

    public CVector3(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }
}

