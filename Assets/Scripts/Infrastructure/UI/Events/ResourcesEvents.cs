using System;

namespace Client.Infrastructure
{
    public class ResourcesEvents
    {
        public event Action ChangeResourceAmount;
        
        public void OnChangeResourceAmount() => ChangeResourceAmount?.Invoke();
    }
}