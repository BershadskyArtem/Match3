﻿using System;
using UnityEngine;

namespace CoreGameplay
{
    public class AnimationNumbers : MonoBehaviour
    {
        public static AnimationNumbers Instance;

        public float CheckDelay;
        public float GravityDelay;
        public float Gap;
        public float SwapTime;
        public float FallTime;
        public float DeathScaleTime;
        private void Awake()
        {
            Instance = this;
        }
        
        
    }
}