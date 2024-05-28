using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class CartoonScaleSpringSystem : IEcsRunSystem
    {
        private EcsFilter<CartoonSpringProvider> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var cartoonSpringProvider = ref entity.Get<CartoonSpringProvider>();

                if (!cartoonSpringProvider.IsScale)
                    continue;

                var relativePosition =
                    cartoonSpringProvider.ObjectTransform.InverseTransformPoint(cartoonSpringProvider.SpringTransform
                        .position);

                var interpolate = relativePosition.y * cartoonSpringProvider.ScaleCoef;

                var scale = Lerp3(cartoonSpringProvider.ScaleDown, Vector3.one, cartoonSpringProvider.ScaleUp,
                    interpolate);

                cartoonSpringProvider.ObjectBody.localScale = scale;
            }
        }

        private Vector3 Lerp3(Vector3 scaleDown, Vector3 scaleOne, Vector3 scaleUp, float interpolate)
        {
            if (interpolate < 0)
                return Vector3.LerpUnclamped(scaleDown, scaleOne, interpolate + 1f);
            return Vector3.LerpUnclamped(scaleOne, scaleUp, interpolate);
        }
    }
}