using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseWalkSoundController : MonoBehaviour
{
    public AudioSource aud;
    public Vector3 prevPos = Vector3.zero;
    public TouchLand TL;
    private AutoResetCounter ARC = new AutoResetCounter(10);
    // Start is called before the first frame update
    void Start()
    {
        aud.Stop();
        aud.loop = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var diff = new Vector2(this.transform.position.x - prevPos.x, this.transform.position.z - prevPos.z);
        if (diff.sqrMagnitude > 0 && TL.onLand && !aud.isPlaying)
        {
            aud.Play();
        }

        if (ARC.IsZeroReached(-1))
        {
            prevPos = this.transform.position;

        }
    }
}
