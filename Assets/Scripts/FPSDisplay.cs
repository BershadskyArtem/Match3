using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
   public TextMeshProUGUI fpsTxt;
   
   public float pollingTime = 1f;
   
   public float time;
   
   public int frameCount;

   private void Awake()
   {
       //Application.targetFrameRate = 60;
   }

   private void Start()
   {
       Application.targetFrameRate = 120;
   }

   void Update()
   
   {
   
       time += Time.deltaTime;
   
       frameCount++;
   
       if(time > pollingTime)
   
       {
   
           int frameRate = Mathf.RoundToInt(frameCount / time);
   
           fpsTxt.text = frameRate.ToString();
   
           time -= pollingTime;
   
           frameCount = 0;
   
       }
   
   }

}
