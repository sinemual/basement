using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client.Client
{
    public class CalculateResourceSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private UserInterfaceEventBus _uiEventBus;
        
        private EcsFilter<AddResourceRequest> _addFilter;
        private EcsFilter<SpendResourceRequest> _spendFilter;
    
        public void Run()
        {
            foreach (var idx in _addFilter)
            {
                ref var entity = ref _addFilter.GetEntity(idx);
                ref var request = ref entity.Get<AddResourceRequest>();
                    
                _data.PlayerData.Resources[request.Type] += request.Amount;
                _data.RuntimeData.MinedLevelResources[request.Type] += request.Amount;
                if (_data.PlayerData.Resources[request.Type] > 999)
                    _data.PlayerData.Resources[request.Type] = 999;
                _uiEventBus.Resources.OnChangeResourceAmount();
                
                entity.Del<AddResourceRequest>();
            }
    
            foreach (var idx in _spendFilter)
            {
                ref var entity = ref _spendFilter.GetEntity(idx);
                ref var request = ref entity.Get<SpendResourceRequest>();
                    
                _data.PlayerData.Resources[request.Type] -= request.Amount;
                if (_data.PlayerData.Resources[request.Type] < 0)
                    _data.PlayerData.Resources[request.Type] = 0;
                _uiEventBus.Resources.OnChangeResourceAmount();
                
                entity.Del<SpendResourceRequest>();
            }
        }
    }
}