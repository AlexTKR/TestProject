using Morpeh;

namespace ECS.Components
{
    public enum EnemyType
    {
        Red,
        Yellow,
        Green
    }

    public struct EnemyComponent : IComponent
    {
        public EnemyType EnemyType;
    }
}
