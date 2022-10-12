using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSoundController : MonoBehaviour
{
    public AudioSource aud;
    public TouchLand TL;
    // Start is called before the first frame update
    void Start()
    {
        aud.Stop();
        aud.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && TL.onLand && !aud.isPlaying)
        {
            aud.Play();
        }
    }
}
