using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine.SceneManagement;

namespace Client
{
    public class RestartGameSystem : IEcsRunSystem
    {
        private SharedData _data;
        
        private EcsFilter<RestartGameRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                var entity = _filter.GetEntity(idx);
                _data.ResetData();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                entity.Del<RestartGameRequest>();
            }
        }
    }
}