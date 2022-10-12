using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseABlockade : DoSomeThing
{
    public RevertableBlockades Blockade;

    public Vector3[] PosFromTo = new Vector3[2];
    public Vector3[] RotFromTo = new Vector3[2];
    public float TotalSecondNeeded = 1;
    private bool released = false;

    public bool UseCurrentTransformAsFrom = false;

    private float StartTime;
    public List<RevertableParticleSystem> RPS = new List<RevertableParticleSystem>();
    public override void Do()
    {
        released = true;
        StartTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (UseCurrentTransformAsFrom)
        {
            PosFromTo[0] = this.transform.position;
            RotFromTo[0] = this.transform.eulerAngles;
        }
        else
        {
            this.transform.position = PosFromTo[0];
            this.transform.eulerAngles = RotFromTo[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(released && !Blockade.RevertPlaying)
        {
            foreach(RevertableParticleSystem rps in RPS)
            {
                rps.PositiveActivated = true;
            }
            this.transform.position = Vector3.Lerp(PosFromTo[0], PosFromTo[1], (Time.time - StartTime) / TotalSecondNeeded);
            this.transform.eulerAngles = Vector3.Lerp(RotFromTo[0], RotFromTo[1], (Time.time - StartTime) / TotalSecondNeeded);
        }
        if (Blockade.RevertPlaying)
        {
            foreach (RevertableParticleSystem rps in RPS)
            {
                rps.PositiveActivated = false;
            }
            released = false;
        }

    }
}
