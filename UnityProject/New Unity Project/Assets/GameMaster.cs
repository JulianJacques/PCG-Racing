using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameMaster : MonoBehaviour
{
    public enum GameMode
    {
        Solo,
        Timed,
        VS
    }

    private bool autoMode = false;
    StreamWriter writer;
    
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Resources/timekeeper.csv";
        //Write some text to the test.txt file
        writer = new StreamWriter(path, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            autoMode = !autoMode;

            if (autoMode)
            {
                Time.timeScale = 5.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
        
        
    }

    public void WriteTime(float time)
    {
        writer.WriteLine(time);
        writer.Close();
    }
    
    public void GameOver()
    {
        
    }
    
    public void ResetGame()
    {
        
    }
    
    public void QuitGame()
    {
        
    }
}
