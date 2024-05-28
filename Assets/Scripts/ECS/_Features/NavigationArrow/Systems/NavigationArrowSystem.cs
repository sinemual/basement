using Client.Data.Core;
using DG.Tweening;
using Leopotam.Ecs;

namespace Client
{
    public class NavigationArrowSystem : IEcsRunSystem
    {
        private SharedData _data;
        
        private EcsFilter<NavigationArrowProvider> _filter;
        private EcsFilter<EnableNavigationArrowRequest> _enableRequestFilter;
        private EcsFilter<DisableNavigationArrowRequest> _disableRequestFilter;

        public void Run()
        {
            foreach (var enableRequest in _enableRequestFilter)
            {
                ref var requestEntity = ref _enableRequestFilter.GetEntity(enableRequest);
                ref var request = ref requestEntity.Get<EnableNavigationArrowRequest>();

                foreach (var idx in _filter)
                {
                    ref var arrowEntity = ref _filter.GetEntity(idx);
                    ref var arrow = ref arrowEntity.Get<NavigationArrowProvider>();
                    arrow.ToPoint = request.Point.transform;
                    arrow.ArrowGameObject.SetActive(true);
                }

                requestEntity.Del<EnableNavigationArrowRequest>();
            }

            foreach (var idx in _filter)
            {
                ref var arrowEntity = ref _filter.GetEntity(idx);
                ref var arrow = ref arrowEntity.Get<NavigationArrowProvider>();
                if (arrow.ArrowGameObject.activeInHierarchy)
                    arrow.ArrowGameObject.transform.DOLookAt(arrow.ToPoint.position, 0.0f, AxisConstraint.Y);
            }

            foreach (var disableRequest in _disableRequestFilter)
            {
                ref var requestEntity = ref _disableRequestFilter.GetEntity(disableRequest);

                foreach (var idx in _filter)
                {
                    ref var arrowEntity = ref _filter.GetEntity(idx);
                    ref var arrow = ref arrowEntity.Get<NavigationArrowProvider>();
                    arrow.ArrowGameObject.SetActive(false);
                }

                requestEntity.Del<DisableNavigationArrowRequest>();
            }
        }
    }
}