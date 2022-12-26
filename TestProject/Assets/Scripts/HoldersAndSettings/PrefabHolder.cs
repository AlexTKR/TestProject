using UnityEngine;

namespace Holders
{
    [CreateAssetMenu(menuName = "PrefabHolder")]
    public class PrefabHolder : ScriptableObject
    {
        public GameObject[] Prefabs;
    }
}
