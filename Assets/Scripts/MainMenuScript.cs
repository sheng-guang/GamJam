using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnterPlayScene()
    {
        SceneManager.LoadScene("play");
    }

    public void EnterMainMenu()
    {
        SceneManager.LoadScene("Starter Scene");
    }
}
