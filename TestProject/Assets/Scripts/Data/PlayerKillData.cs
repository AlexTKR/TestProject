using UniMob;

namespace Data
{
    public class PlayerKillData : ILifetimeScope
    {
        [Atom] public int KillCount { get; set; }
        
        public Lifetime Lifetime { get; }
    }
}