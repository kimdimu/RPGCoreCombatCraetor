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

        public void CancelCurAction() //만드려했는데!! 이것조차 함수화
        {
            StartAction(null);
        }
    }
}