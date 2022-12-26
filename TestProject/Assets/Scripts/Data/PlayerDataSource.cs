using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerData")]
    public class PlayerDataSource : SoDataSourceBase<PlayerData>
    {
        [SerializeField] private float Radius;
        [SerializeField] private float Speed;
        [SerializeField] private int DamageAmount;


        public override void Compose(PlayerData instance)
        {
            instance.Radius = Radius;
            instance.Speed = Speed;
            instance.DamageAmount = DamageAmount;
        }
    }
}