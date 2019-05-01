using UnityEngine;

namespace Singletons
{
    public class StageSession : UnsavedSingleton<StageSession>
    {
        public void PlayerDied()
        {
            Debug.Log("Failed");
        }
    }
}