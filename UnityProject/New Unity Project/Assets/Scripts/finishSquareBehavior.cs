using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishSquareBehavior : MonoBehaviour
{
   public static float timer;

    private void Awake()
    {
        timer = 0f;
    }
    private void Update()
   {
        Debug.Log(timer);
      timer += Time.deltaTime;
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
        Time.timeScale = 0;
        FindObjectOfType<EndOfRaceScreen>().ToggleActive();
      //FindObjectOfType<GameMaster>().WriteTime(timer);
   }
}
