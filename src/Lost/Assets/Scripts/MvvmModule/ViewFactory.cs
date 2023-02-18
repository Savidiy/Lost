using UnityEngine;
using Zenject;

namespace MvvmModule
{
    internal sealed class ViewFactory : IViewFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IPrefabFactory _prefabFactory;

        public ViewFactory(DiContainer diContainer, IPrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
            _diContainer = diContainer;
        }

        public TView CreateView<TView, THierarchy>(THierarchy hierarchy)
            where TView : View<THierarchy>
            where THierarchy : MonoBehaviour
        {
            _diContainer.Bind<GameObject>().FromInstance(hierarchy.gameObject).AsSingle();
            _diContainer.Bind<TView>().AsSingle();

            var view = _diContainer.Resolve<TView>();

            _diContainer.Unbind<TView>();
            _diContainer.Unbind<GameObject>();
            return view;
        }

        public TView CreateView<TView, THierarchy>(string prefabName, Transform parent)
            where TView : View<THierarchy>
            where THierarchy : MonoBehaviour
        {
            var hierarchy = _prefabFactory.Instantiate<THierarchy>(prefabName, parent);

            return CreateView<TView, THierarchy>(hierarchy);
        }
    }
}