using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public class CoroutineWrapper
    {
        private MonoBehaviour owner;
        private Coroutine coroutine;

        public bool IsDone { get; private set; }

        public CoroutineWrapper(MonoBehaviour owner, Coroutine coroutine)
        {
            this.owner = owner;
            this.coroutine = coroutine;
            IsDone = false;
        }

        public void Stop()
        {
            owner.StopCoroutine(coroutine);
            IsDone = true;
        }
    }
}
