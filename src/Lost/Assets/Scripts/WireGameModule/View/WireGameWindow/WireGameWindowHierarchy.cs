using UiModule;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace WireGameModule.View.WireGameWindow
{
    public sealed class WireGameWindowHierarchy : MonoBehaviour
    {
        public Button BackButton;
        public SimpleProgressBar SimpleProgressBar;
        public Image BackImage;
        public Transform PointsRoot;
        public Transform WiresRoot;
    }
}