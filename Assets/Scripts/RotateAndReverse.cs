using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndReverse : MonoBehaviour
{
    public GameObject MeTheBigRoll;
    public GameObject ThePairingBigRoll;

    public Vector3 MeOrigRot;
    public Vector3 PairOrigRot;

    private bool ThePlayerEntered = false;
    private float EnterTime;
    private GameObject PlayerGO;
    private bool pada = false;

    public List<RevertableParticleSystem> RPS = new List<RevertableParticleSystem>();

    public Vector3 camLookAt;
    // Start is called before the first frame update
    void Start()
    {
        MeOrigRot = MeTheBigRoll.transform.eulerAngles;
        PairOrigRot = ThePairingBigRoll.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (ThePlayerEntered)
        {
            var passedTime = Time.time - EnterTime;
            if(passedTime < 2)
            {
                foreach (RevertableParticleSystem rps in RPS)
                {
                    rps.PositiveActivated = true;
                }
                MeTheBigRoll.transform.eulerAngles = new Vector3(0, passedTime * -45, 0) + MeOrigRot;
                ThePairingBigRoll.transform.eulerAngles = new Vector3(0, passedTime * 45, 0) + PairOrigRot;
                PlayerGO.transform.position = MeTheBigRoll.transform.position - Vector3.up * 1.3f;
                pada = true;
                //Camera.main.transform.LookAt(camLookAt);
            }
            else if(passedTime < 4)
            {
                foreach (RevertableParticleSystem rps in RPS)
                {
                    rps.PositiveActivated = false;
                }
                if (pada)
                {
                    RevertSceneManager.instance.ChangeTimeDirection();
                    RevertSceneManager.instance.PauseRecording = true;
                }
                pada = false;
                MeTheBigRoll.transform.eulerAngles = new Vector3(0, (4 - passedTime) * -45, 0) + MeOrigRot;
                ThePairingBigRoll.transform.eulerAngles = new Vector3(0, (4 - passedTime) * 45, 0) + PairOrigRot;
                PlayerGO.transform.position = ThePairingBigRoll.transform.position - Vector3.up * 1.3f;
                //Camera.main.transform.LookAt(camLookAt);
            }
            else
            {
                ThePlayerEntered = false;
                RevertSceneManager.instance.PauseRecording = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ThePlayerEntered = true;
            EnterTime = Time.time;
            PlayerGO = other.gameObject;
            
        }
    }
}
