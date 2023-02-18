using System;
using UnityEngine;
using Zenject;

namespace Lost.Utils
{
    public class TickInvoker : ITickable, ILateTickable
    {
        public event Action Updated;
        public event Action LateUpdated;
        public float DeltaTime { get; private set; }

        public void Tick()
        {
            DeltaTime = Time.deltaTime;
            Updated?.Invoke();
        }

        public void LateTick()
        {
            LateUpdated?.Invoke();
        }
    }
}