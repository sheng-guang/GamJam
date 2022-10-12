using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSoundController : MonoBehaviour
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
        if ((Input.GetKey(KeyCode.Space)) && TL.onLand && !aud.isPlaying)
        {
            aud.Play();
        }
    }
}
