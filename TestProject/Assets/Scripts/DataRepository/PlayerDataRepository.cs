using Data;
using DataSource;

namespace DataRepository
{
    public class PlayerDataRepository : IDataRepository<PlayerData>
    {
        private PlayerData _playerData;

        public PlayerDataRepository(IDataSource<PlayerData> dataSource)
        {
            _playerData = dataSource.Load();
        }
        
        public PlayerData Data => _playerData;
    }
}