using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject TheMe;
    public bool EndDisGame = false;
    public UnityEngine.UI.Image imagge;
    public UnityEngine.UI.Image imagge2;
    public AutoResetCounter CountDown = new AutoResetCounter(100);
    // Start is called before the first frame update
    void Start()
    {
        imagge.gameObject.active = false;
        imagge2.gameObject.active = false;
        CountDown.MaxmizeTemp();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(TheMe.transform.position, this.transform.position) < 1f && !RevertSceneManager.instance.PauseRecording)
        {
            EndDisGame = true;
        }

        if (EndDisGame)
        {
            
            imagge.gameObject.active = true;
            if (CountDown.IsZeroReached(1, false))
            {
                imagge.gameObject.active = false;
                imagge2.gameObject.active = true;
                Cursor.visible = true;
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            EndDisGame = true;
        }
        else
        {
            h.l(collision.gameObject.name);
        }
    }
    public void OnCollisionStay (Collision collision)
    {
        OnCollisionEnter(collision);
    }
}
