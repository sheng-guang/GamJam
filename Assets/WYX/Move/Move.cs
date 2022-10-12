using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour,IRealPoss,ICameraFollowe_update
{
    public Vector3 RealPoss => transform.position;

    public Vector3 Virtual_poss => visual_point.position;
    public Transform visual_point;
    public Rigidbody rig;
    public Animator anim;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        Cam.Tar_realposs = this;
        anim.SetFloat("v", 0);
    }
    void Start()
    {
        this.FollowCam();
    }
    public float FloatV = 1;
    public float v=5;
    public float shiftV = 8;
    public float JumpV = 5;
    void Update()
    {
        //Cursor.visible = false;
        ws = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
         da = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        shift = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKey(KeyCode.Space);

    }
    public void FollowCamUpdate_()
    {
        transform.forward = Cam.foward_x0z();
    }
    public int ws, da;

    public bool shift;
    public bool jump;


    public TouchLand OnLand;
    void FixedUpdate()
    {
        Vector3 WSDA = Vector3.zero;
        Vector3 LandV = Vector3.zero;
        //Vector3 FloatV = Vector3.zero;
        if (ws != 0 || da != 0)
        {
            WSDA = Cam.foward_x0z().normalized * ws + Cam.Right().normalized * da;
           
            WSDA = WSDA.normalized * (shift ? shiftV : v);
            //FloatV=WSDA.normalized*(shift?)
        }



        //朝向
        if ((ws != 0 || da != 0)&&OnLand.onLand) 
        {
            //Vector3 foward = WSDA;
            //foward.y = 0;

        }

        //速度

        rig.velocity = new Vector3(WSDA.x, rig.velocity.y, WSDA.z);
        //if (OnLand.onLand)
        //{
        //}
        //else
        //{
        //    //rig.velocity=new Vector3(WSDA)
        //}

        if (jump && OnLand.onLand)
        {
            rig.velocity += (Vector3.up) * JumpV;
        }
        //anim.SetFloat("ws", ws*v);
        //anim.SetFloat("da", da*v);

        anim.SetFloat("v", rig.velocity.magnitude);
        anim.SetBool("OnLand", OnLand.onLand);


        anim.SetFloat("v up", rig.velocity.y);
    }


}
