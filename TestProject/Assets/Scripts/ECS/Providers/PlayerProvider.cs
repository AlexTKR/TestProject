using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerProvider : MonoProvider<PlayerComponent> {
    }
}