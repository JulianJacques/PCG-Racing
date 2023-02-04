using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishSquareBehavior : MonoBehaviour
{
   private float timer;

   private void Update()
   {
      timer += Time.deltaTime;
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      FindObjectOfType<GameMaster>().WriteTime(timer);
   }
}
