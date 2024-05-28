using Client.Data.Core;
using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class EarningViewSystem : IEcsRunSystem
    {
        private SharedData _data;
        
        private PrefabFactory _prefabFactory;
        
        private EcsFilter<CreateEarnViewRequest>.Exclude<WorldTextProvider> _requestFilter;
        private EcsFilter<WorldTextProvider, CreateEarnViewRequest, InUseMarker>.Exclude<Timer<TimerForShowingEarningView>> _initInfoFilter;
        private EcsFilter<WorldTextProvider, CreateEarnViewRequest, TimerDoneEvent<TimerForShowingEarningView>>.Exclude<DespawnAtTimerRequest>
            _despawnInfoFilter;

        public void Run()
        {
            foreach (var idx in _requestFilter)
            {
                ref var entity = ref _requestFilter.GetEntity(idx);
                ref var createEarnViewRequest = ref entity.Get<CreateEarnViewRequest>();

                Vector3 pos = createEarnViewRequest.SpawnPoint.transform.position + createEarnViewRequest.Offset;
                _prefabFactory.SpawnWithEntity(ref entity, _data.StaticData.PrefabData.EarnInfoPrefab, pos, Quaternion.identity);
                entity.Get<InUseMarker>();
            }

            foreach (var idx in _initInfoFilter)
            {
                ref var entity = ref _initInfoFilter.GetEntity(idx);
                ref var createEarnViewRequest = ref entity.Get<CreateEarnViewRequest>();
                ref WorldTextProvider entityEarnInfoView = ref entity.Get<WorldTextProvider>();

                entityEarnInfoView.Value.text = $"+{Utility.FormatMoney(createEarnViewRequest.Value)}$";
                entity.Get<Timer<TimerForShowingEarningView>>().Value = 1.0f;
            }

            foreach (var idx in _despawnInfoFilter)
            {
                _despawnInfoFilter.GetEntity(idx).Del<InUseMarker>();
                _despawnInfoFilter.GetEntity(idx).Get<DespawnAtTimerRequest>();
            }
        }
    }
}