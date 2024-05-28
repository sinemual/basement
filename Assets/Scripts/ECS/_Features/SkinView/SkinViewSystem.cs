using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class SkinViewSystem : IEcsRunSystem
    {
        private EcsFilter<SkinViewProvider>.Exclude<SkinSelectedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var skinViewProvider = ref entity.Get<SkinViewProvider>();

                foreach (var skin in skinViewProvider.SkinViews)
                    skin.SetActive(false);

                if (skinViewProvider.IsRandomSkin)
                {
                    int randomNum = -1;
                    randomNum = Random.Range(0, skinViewProvider.SkinViews.Count);
                    skinViewProvider.SkinViewNum = randomNum;
                }

                skinViewProvider.SkinViews[skinViewProvider.SkinViewNum].SetActive(true);

                entity.Get<SkinSelectedMarker>();
            }
        }
    }
}