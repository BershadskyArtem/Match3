using System;
using UnityEngine;

namespace CoreGameplay
{
    public class AnimationNumbers : MonoBehaviour
    {
        private static AnimationNumbers Instance;
        public static float SwipeCheckDelay;
        public static float GravityApplyDelay;
        public static float NodeGap;
        public static float SwapSpeed;
        public static float FallSpeed;
        public static float DeathAnimationSpeed;
        public static AnimationCurve FallMovCurve; 
        public static AnimationCurve DestroyScaleCurve; 
        public static AnimationCurve SwapMovCurve;
        public static float NodeFlyAwayTime;


        public float CheckDelay;
        public float GravityDelay;
        public float Gap;
        public float SwapTime;
        public float FallTime;
        public float DeathScaleTime;
        public float FlyAwayTime;
        public AnimationCurve FallCurve;
        public AnimationCurve DestroyCurve;
        public AnimationCurve SwapCurve;
        private void Awake()
        {
            Instance = this;
            
            SwipeCheckDelay = CheckDelay;
            GravityApplyDelay = GravityDelay;
            NodeGap = Gap;
            SwapSpeed = SwapTime;
            FallSpeed = FallTime;
            DeathAnimationSpeed = DeathScaleTime;
            FallMovCurve = FallCurve;
            DestroyScaleCurve = DestroyCurve;
            SwapMovCurve = SwapCurve;
            NodeFlyAwayTime = FlyAwayTime;
        }
        
        
    }
}