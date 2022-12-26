using UnityEngine;

namespace HoldersAndSettings
{
    [CreateAssetMenu(menuName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public int EnemyCount;
        public int MaxDamageCount;
    }
}
