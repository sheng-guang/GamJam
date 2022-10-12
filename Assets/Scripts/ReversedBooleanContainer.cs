using System.Collections.Generic;
using UnityEngine;

public class ReversedBooleanContainer
{

    private bool currentState = false;
    private List<float> record = new List<float>(4095);
    private int progressIndex = 0;
    private float recordingLength = 0;

    /// <summary>
    /// get the state of a button for a sepecific timeStamp
    /// </summary>
    /// <param name="timeStamp">a given timeStamp</param>
    /// <returns></returns>
    public bool getCurrentState(float timeStamp)
    {
        timeStamp = recordingLength - timeStamp;
        while (progressIndex > 0 && record[progressIndex] > timeStamp)
        {
            progressIndex -= 1;
            currentState = !currentState;
        }
        return currentState;
    }

    /// <summary>
    /// pushes the timeStamp of a state change of a button into the recording
    /// </summary>
    /// <param name="timeStamp">the timeStamp of the state change</param>
    public void pushTimeStamp(float timeStamp)
    {
        record.Add(timeStamp);
    }

    /// <summary>
    /// Finalizes a recording or reset it
    /// </summary>
    /// <param name="finalTimeStamp">The final timeStamp marking the end of a recording, use -1 for a soft reset</param>
    /// <param name="finalState">The final state of the button, discarded if recieving -1 for finalTimeStamp</param>
    public void Init(float finalTimeStamp, bool finalState)
    {
        progressIndex = record.Count - 1;
        if (finalTimeStamp != -1)
        {
            recordingLength = finalTimeStamp;
            currentState = finalState;
        }
    }

    /// <summary>
    /// reverse the current state and return the new state
    /// </summary>
    /// <returns></returns>
    private bool reverseState()
    {
        currentState = !currentState;
        return currentState;
    }

    public override string ToString()
    {
        string output = string.Format("currentState = {0}, progressIndex = {1}, recordingLength = {2}\n", currentState.ToString(),
            progressIndex.ToString(), recordingLength.ToString());
        for (int i = 0; i < record.Count; i++)
        {
            output += string.Format("ID = {0}, timeStamp = {1}\n", i.ToString(), record[i].ToString());
        }
        return output;
    }
}
