using System;
using Morpeh;

namespace ECS.Components
{
    public struct DamageDelayComponent : IComponent
    {
        public TimeSpan DelayTime;
    }
}