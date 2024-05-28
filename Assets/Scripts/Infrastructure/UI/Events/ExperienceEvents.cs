using System;

namespace Client.Infrastructure
{
    public class ExperienceEvents
    {
        public event Action GetExperience;
        
        public void OnGetExperience() => GetExperience?.Invoke();
    }
}