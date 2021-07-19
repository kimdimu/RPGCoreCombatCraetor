using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction curAction;
        public void StartAction(IAction action)
        {
            if (curAction == action) return;
            if (curAction != null)
            {
                curAction.Cancel();
            }
            curAction = action;
        }
    }
}