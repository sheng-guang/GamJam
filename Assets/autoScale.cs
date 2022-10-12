using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoScale : MonoBehaviour
{
    public RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rect = rt.rect;
        rect.size = new Vector2 (Screen.width, Screen.height);
        rect.position = Vector2.zero;

    }
}
