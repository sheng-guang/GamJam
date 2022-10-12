using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertableParticleSystem : Revertable
{


    [SerializeField]
    public ReversedMovementContainer record = new ReversedMovementContainer();

    [SerializeField]
    public ReversedMovementContainer newRecord = new ReversedMovementContainer();


    public ParticleSystem myParticleSystem;
    [SerializeField]
    public ParticleSystem.EmitParams eprams;
    public float fieldOfParticles;
    public int particlePerFrame = 1;
    public float velocity;

    public bool PositiveActivated = false;


    public override void RevertUpdate()
    {
        return;
        base.RevertUpdate();
        float TimePassed = Time.time - RevertSceneManager.instance.timeStamp;
        if (record.GetStateAtTime(TimePassed).v == 1)
        {
            for (int i = 0; i < particlePerFrame; ++i)
            {
                Vector3 RandomPoint = EulerToDirection(UnityEngine.Random.Range(-360 * fieldOfParticles, 360 * fieldOfParticles), UnityEngine.Random.Range(-360 * fieldOfParticles, 360 * fieldOfParticles));
                RandomPoint = Vector3.Normalize(Vector3.Lerp(this.transform.forward, RandomPoint, 0.5f));
                //Multiplier = AlreadyCountDown / (ChargeHoldGasp.Value * 100);
                //SqrtMultiplier = Mathf.Sqrt(Multiplier);
                //ParticleSystem.EmitParams eprams = new ParticleSystem.EmitParams();
                eprams.velocity = -RandomPoint * 2 * velocity;
                eprams.position =(RandomPoint * myParticleSystem.startLifetime * 2);
                //eprams.startSize = 0.2f * Multiplier * Multiplier;
                //eprams.startLifetime = PS.startLifetime * SqrtMultiplier;
                //eprams.startColor = new Color(PS.startColor.r, PS.startColor.g, PS.startColor.b, 1);
                myParticleSystem.Emit(eprams, 1);
            }
        }
        myParticleSystem.Stop();

    }

    public override void PositiveUpdate()
    {
        return;
        base.PositiveUpdate();
        float TimePassed = Time.time - RevertSceneManager.instance.timeStamp;
        if (PositiveActivated)
        {
            for (int i = 0; i < particlePerFrame; ++i)
            {
                Vector3 RandomPoint = EulerToDirection(UnityEngine.Random.Range(-360 * fieldOfParticles, 360 * fieldOfParticles), UnityEngine.Random.Range(-360 * fieldOfParticles, 360 * fieldOfParticles));
                RandomPoint = Vector3.Normalize(Vector3.Lerp(this.transform.forward, RandomPoint, 0.5f));
                //Multiplier = AlreadyCountDown / (ChargeHoldGasp.Value * 100);
                //SqrtMultiplier = Mathf.Sqrt(Multiplier);
                //ParticleSystem.EmitParams eprams = new ParticleSystem.EmitParams();
                eprams.velocity = RandomPoint * 2 * velocity;
                //eprams.startSize = 0.2f * Multiplier * Multiplier;
                //eprams.startLifetime = PS.startLifetime * SqrtMultiplier;
                //eprams.startColor = new Color(PS.startColor.r, PS.startColor.g, PS.startColor.b, 1);
                myParticleSystem.Emit(eprams, 1);
            }
        }
        myParticleSystem.Stop();
        newRecord.PushStateAtTime(new ReversedMovementContainer.TimeStampedState(Vector3.zero, Vector3.zero, "", 0, TimePassed, PositiveActivated? 1:-1 ));

    }

    public override void Respawned()
    {
        return;
    }

    public override void Revert()
    {
        record = newRecord;
        this.NotInRevertSpace = !RevertSceneManager.instance.PositiveTimeFlowing;
        revertPlaying = true;
    }
    public Vector3 EulerToDirection(float Elevation, float Heading)
    {
        float elevation = Elevation * Mathf.Deg2Rad;
        float heading = Heading * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));
    }
}
