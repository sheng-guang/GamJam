using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoSomeThing : MonoBehaviour
{
    public abstract void Do();
}
public class E_Button : MonoBehaviour
{
    public KeyCode ActivationoKey;
    //被触发的物体需要写个脚本继承DoSomeThing
    public DoSomeThing todo;
    //是否显示提示ui
    public bool Show = false;
    //提示文字
    public string ButtenText;
    void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Move>()) Show = true;
        //UI.SetActive(Show);
    }
    void OnTriggerExit(Collider c)
    {
        if (c.GetComponent<Move>()) Show = false;
        //UI.SetActive(Show);
    }
    void Update()
    {
        if (Show && Input.GetKeyDown(ActivationoKey)) todo.Do();
    }
    GUIStyle fontStyle = new GUIStyle();
    private void Awake()
    {
        fontStyle.fontSize = 50;
        fontStyle.normal.textColor = new Color(0.5f, 1, 1);
       
    }

    void OnGUI()
    {
        //print("1");
        if (Show) GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.8f, 150, 200), ButtenText,fontStyle);
    }

}
