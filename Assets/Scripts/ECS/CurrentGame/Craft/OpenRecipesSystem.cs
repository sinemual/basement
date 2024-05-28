using Client.Data.Core;
using Client.ECS.CurrentGame.Experience;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class OpenRecipesSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private CameraService _cameraService;

        private EcsFilter<BuildEvent> _filter;
        private EcsFilter<BuildingProvider> _buidlingFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var build = ref entity.Get<BuildEvent>();

                Vector3 position = new Vector3();
                foreach (var building in _buidlingFilter)
                {
                    if (_buidlingFilter.Get1(building).Type == build.Data.Type)
                        position = _buidlingFilter.Get1(building).BuidlingPlace.transform.position;
                }
                
                foreach (var recipe in build.Data.GettedCraftRecipes)
                {
                    GameObject spawnItem = Object.Instantiate(recipe.View.DropItemPrefab,
                        position, Quaternion.identity);

                    _world.NewEntity().Get<GoToPlayerRequest>() = new GoToPlayerRequest()
                    {
                        ItemData = recipe,
                        ItemGo = spawnItem
                    };
                }
            }
        }
    }
}