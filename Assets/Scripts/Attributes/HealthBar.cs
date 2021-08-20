using UnityEngine;
namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas canvas = null;

        void Update()
        {
            if (Mathf.Approximately(health.GetFraction(), 0) || Mathf.Approximately(health.GetFraction(), 1))
            {
                canvas.enabled = false;
                return;
            }
            else
                canvas.enabled = true;
                foreground.localScale = new Vector3(health.GetFraction(), 1, 1);
        }
    }
}