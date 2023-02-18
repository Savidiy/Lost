using UnityEngine;
using UnityEngine.UI;

namespace UiModule
{
    internal sealed class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        public void SetProgress(float normalizedProgress)
        {
            _fillImage.fillAmount = normalizedProgress;
        }
    }
}