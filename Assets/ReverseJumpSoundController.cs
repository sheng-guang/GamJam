using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseJumpSoundController : MonoBehaviour
{
    public AudioSource aud;
    float prevY = -1000;
    public bool Played = false;
    public TouchLand TL;
    // Start is called before the first frame update
    void Start()
    {
        aud.Stop();
        aud.loop = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((prevY - this.transform.position.y) < 0.2f && !aud.isPlaying && !Played && !TL.onLand)
        {
            aud.Play();
            Played = true;
        }

        if (TL.onLand)
        {
            Played = false;
        }
        prevY = this.transform.position.y;
    }
}
