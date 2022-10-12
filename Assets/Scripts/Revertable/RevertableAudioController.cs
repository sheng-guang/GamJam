using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RevertableAudioController : Revertable
{
    public AudioSource PositiveAudio;
    public AudioSource ReversedAudio;


    public override void PositiveUpdate()
    {
        base.PositiveUpdate();
        if (!PositiveAudio.isPlaying)
        {
            PositiveAudio.Play();
        }
        ReversedAudio.Stop();
    }
    public override void RevertUpdate()
    {
        base.RevertUpdate();
        if (!ReversedAudio.isPlaying)
        {
            ReversedAudio.Play();
        }
        PositiveAudio.Stop();
    }

    public override void Revert()
    {
        //base.Revert();
        //var temptime = PositiveAudio.time;
        //PositiveAudio.time = ReversedAudio.clip.length - ReversedAudio.time;
        //ReversedAudio.time = ReversedAudio.clip.length - temptime;
        //h.l(ReversedAudio.clip.length - ReversedAudio.time);
        //h.l(ReversedAudio.clip.length - temptime);
    }


    public override void Respawned()
    {
        PositiveAudio.time = 0;
        ReversedAudio.time = 0;
    }
}

