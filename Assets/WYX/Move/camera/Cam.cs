using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;//引用
public class Cam : MonoBehaviour
{
    Transform group;
    public static Cam instance { get; private set; } = null;
    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        group = transform.GetChild(0);
        Cursor.visible = false;

        fontStyle.fontSize = 30;
        fontStyle.normal.textColor = new Color(0, 0, 0);
        group.transform.localPosition = new Vector3(0, 0, tar_dis);
    }


    // Update is called once per frame

    [Header("ro")]
    public float ro_speed = 1;
    public float max_x = 89;
    public float min_x = -89;
    Vector3 lastMouse;
    Vector3 tar_euler_angle;
    void ro()
    {
       
        //Input.mouseScrollDelta
        //var now = Input.mousePosition;
        Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))*ro_speed;
        //var delta = (now - lastMouse) * ro_speed;

        lastMouse = Input.mousePosition;
        tar_euler_angle += new Vector3(-delta.y, delta.x);
        if (tar_euler_angle.x >= max_x) tar_euler_angle.x = max_x;
        if (tar_euler_angle.x <= min_x) tar_euler_angle.x = min_x;
        transform.eulerAngles = tar_euler_angle;

        //}
    }
    [Header("dis")]
    public bool distance = false;
    public float tar_dis = -10;
    public float dis_speed = 5;
    public float back_limit = -100, foward_limit = 0;
    void dis()
    {
        if (distance == false) return;
        var dis = Input.GetAxis("Mouse ScrollWheel");
        tar_dis += dis * dis_speed;
        if (tar_dis < back_limit) tar_dis = back_limit;
        if (tar_dis > foward_limit) tar_dis = foward_limit;
        group.transform.localPosition = new Vector3(0, 0, tar_dis);
    }
    [Header("follow")]
    public bool smoth = false;

    public float maxDis = 5;
    void follow()
    {
        if (smoth)
        {
            Vector3 dis = SmothTar - transform.position;
            Vector3 dis_normal = dis.normalized;
            float dis_flo = dis.magnitude;
            if (dis_flo > maxDis) dis_flo = maxDis;
            float dis_2 = dis_flo * dis_flo;
            Vector3 tomove = dis_2 * dis_normal * Time.unscaledDeltaTime;
            if (tomove.magnitude >= dis.magnitude) tomove = dis;
            transform.position += tomove;
        }
        else
        {
            transform.position = SmothTar;
        }
        //float dis = D.magnitude;
        //dis *= forceL;
        //if (dis > maxL) dis = maxL;
        //transform.position += D * dis * Time.unscaledDeltaTime;
        //Vector3 dis = Tar - transform.position;

    }
    [DllImport("user32.dll")]
    public static extern int SetCursorPos(int x, int y);
    [Header("ma")]
    public bool followMA = false;
    public void LateUpdate()
    {
        FigureV3();
        ro();
        dis();
        follow();
        Update_extra();
        FollowUpdate();

        //else
        //{
        //    lastMouse = Input.mousePosition;
        //}
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.visible = Cursor.visible ? false : true;

            //Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Confined;
        }
        if (Cursor.visible == false)
        {
            int centerx = Screen.width / 2;
            int centery = Screen.height / 2;

            SetCursorPos(centerx, centery);
        }

    }
    void Update_extra() { if (smoth == false) follow(); }
    void FollowUpdate()
    {
        for (int i = 0; i < followUpdate.Count; i++)
        {
            if (followUpdate[i] == null || followUpdate[i].Equals(null)) continue;

            followUpdate[i].FollowCamUpdate_();
        }
    }
    public static void AddFollower(ICameraFollowe_update follower) { followUpdate.Add(follower); }
    static List<ICameraFollowe_update> followUpdate = new List<ICameraFollowe_update>();
    void FigureV3()
    {
        if (followMA == false || MACount <= 1) { SmothTar = Tar; return; }

        points[toWrite_index] = Tar;

        toWrite_index++;
        if (toWrite_index < MACount == false) toWrite_index = 0;
        SmothTar = Vector3.zero;
        for (int i = 0; i < MACount; i++)
        {
            SmothTar += points[i];
        }
        SmothTar /= MACount;

    }
    public int MACount = 3;
    int toWrite_index = 0;
    public static Vector3[] points = new Vector3[100];
    public static Vector3 SmothTar = Vector3.zero;
    GUIStyle fontStyle = new GUIStyle();
    void OnGUI()
    {
    }

    public static IRealPoss Tar_realposs { get; set; } = null;
    public static Vector3 Tar { get { return Tar_realposs != null && Tar_realposs.Equals(null) == false ? Tar_realposs.Virtual_poss : Vector3.zero; } }

    public static Vector3 camPoss { get { return instance == null ? Vector3.zero : instance.group.position; } }
    public static Vector3 foward()
    {
        if (instance == null) return Vector3.forward;
        return instance.transform.forward;
    }
    public static Vector3 foward_x0z()
    {
        if (instance == null) return Vector3.forward;
        Vector3 fo = foward();
        fo.y = 0;
        return fo;
    }
    public static Vector3 eularangle_xyz()
    {
        if (instance == null) return Vector3.forward;
        return instance.transform.eulerAngles;
    }
    public static Vector3 eularangle_0y0()
    {
        if (instance == null) return Vector3.forward;
        var re = new Vector3(0, eularangle_xyz().y);

        return re;
    }
    public static Vector3 Right()
    {

        if (instance == null) return Vector3.right;
        return instance.transform.right;
    }

    public static Vector3 eula_in_cam_space(Vector3 eula)
    {
        if (instance == null) return Vector3.zero;
        return instance.transform.localToWorldMatrix.MultiplyVector(eula);
        //return instance.transform.InverseTransformDirection(eula);
    }
    public static void look_at(Vector3 t)
    { if (instance == null) return; instance.transform.LookAt(t, Vector3.up); }
}
//可作为目标的接口
public interface IRealPoss
{
    Vector3 RealPoss { get; }
    Vector3 Virtual_poss { get; }

}
public static class CamExtra
{
    public static void FollowCam(this ICameraFollowe_update follower) { Cam.AddFollower(follower); }
}
public interface ICameraFollowe_update
{
    void FollowCamUpdate_();
}
public struct V3 : IRealPoss
{
    public Vector3 RealPoss { get; set; }

    public Vector3 Virtual_poss { get => RealPoss; set { RealPoss = value; } }
}