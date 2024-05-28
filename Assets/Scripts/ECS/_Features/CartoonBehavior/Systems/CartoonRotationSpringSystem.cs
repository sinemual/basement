using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class CartoonRotationSpringSystem : IEcsRunSystem
    {
        private EcsFilter<CartoonSpringProvider> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var cartoonSpringProvider = ref entity.Get<CartoonSpringProvider>();

                if (!cartoonSpringProvider.IsRotate)
                    continue;

                var relativePosition =
                    cartoonSpringProvider.ObjectTransform.InverseTransformPoint(cartoonSpringProvider.SpringTransform
                        .position);

                cartoonSpringProvider.ObjectBody.localEulerAngles =
                    new Vector3(relativePosition.z, 0, -relativePosition.x) * cartoonSpringProvider.RotationCoef;
            }
        }
    }
}