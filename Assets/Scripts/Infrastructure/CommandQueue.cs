using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure
{

    internal class ActionItem
    {
        public Action Action;
        public float Delay;
        public bool IsExecuting = false;
        
        public void Execute()
        {
            IsExecuting = true;
            Action?.Invoke();
            IsExecuting = false;

        }
        
        //public async void Execute()
        //{
        //    if (Delay == 0)
        //    {
        //        IsExecuting = true;
        //        this.Action.Invoke();
        //        IsExecuting = false;
        //        return;
        //    }
        //    await Task.Delay(TimeSpan.FromMilliseconds(Delay)).ContinueWith( t =>
        //    {
        //        IsExecuting = true;
        //        Action.Invoke();
        //        IsExecuting = false;
        //    });
        //}
    }
    
    public class CommandQueue
    {
        private List<ActionItem> _queue;
        private bool _isCancelRequested;

        public CommandQueue()
        {
            _queue = new List<ActionItem>();
        }
        
        public CommandQueue AddToQueue(Action command, float delay = 0)
        {
            var item = new ActionItem
            {
                Action = command,
                Delay = delay
            };
            _queue.Add(item);
            return this;
        }

        public IEnumerator Run(float delay)
        {
            int i = 0;
            if (delay != 0) yield return new WaitForSeconds(delay);
            while (i < _queue.Count)
            {
                if(_isCancelRequested) yield break;
                float currentDelay = _queue[i].Delay;
                if(currentDelay != 0)
                    yield return new WaitForSeconds(currentDelay);
                _queue[i].Execute();
                i++;
            }
        }

        public void Cancel()
        {
            _isCancelRequested = true;
        }
    }
}