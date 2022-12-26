namespace Data
{
    public class PlayerKillRepository : IDataRepository<PlayerKillData>
    {
        private PlayerKillData _playerKillData = new PlayerKillData();
        
        public PlayerKillData Data => _playerKillData;
    }
}
