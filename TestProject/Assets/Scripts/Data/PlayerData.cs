using UniMob;

namespace Data
{
    public class PlayerData : ILifetimeScope
    {
        [Atom] public float Radius { get; set; }
        [Atom] public float Speed { get; set; }
        [Atom] public float DamageAmount { get; set; }
        
        public Lifetime Lifetime { get; }
    }
}