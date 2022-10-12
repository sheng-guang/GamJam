using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenMinSuicide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnGUI()
    {
        //print("1");
        if (Time.time  - RevertSceneManager.instance.timeStamp > 540)
        {
            if (Time.time - RevertSceneManager.instance.timeStamp > 600)
            {
                Application.Quit();
            }
                GUI.Label(new Rect(Screen.width * 0.8f, Screen.height * 0.8f, 150, 200), "Game will auto close in 1 min\n in order to protecc ur computer");
        }
    }

}
