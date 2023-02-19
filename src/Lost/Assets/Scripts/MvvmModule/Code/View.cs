using System;
using System.Collections.Generic;
using Savidiy.Utils;
using UniRx;
using UnityEngine;

namespace MvvmModule
{
    public abstract class View<THierarchy, TViewModel> : View<THierarchy>
        where THierarchy : MonoBehaviour
        where TViewModel : class, IViewModel
    {
        private readonly List<IDisposable> _bindDisposables = new();

        public TViewModel ViewModel { get; private set; }

        protected View(GameObject gameObject, IViewFactory viewFactory) : base(gameObject, viewFactory)
        {
        }

        public void Initialize(TViewModel viewModel)
        {
            ClearViewModel();
            ViewModel = viewModel;
            UpdateViewModel(viewModel);
        }

        protected abstract void UpdateViewModel(TViewModel viewModel);

        protected override void ReleaseViewModel()
        {
            base.ReleaseViewModel();
            for (int i = 0; i < _bindDisposables.Count; i++)
                _bindDisposables[i].Dispose();

            _bindDisposables.Clear();
        }

        public override void ClearViewModel()
        {
            base.ClearViewModel();

            if (ViewModel == null)
                return;

            ReleaseViewModel();
            if (ViewModel is IDisposable disposable)
                disposable.Dispose();
            ViewModel = null;
        }

        public override void Dispose()
        {
            ClearViewModel();
            base.Dispose();
        }

        protected void Bind<T>(IReadOnlyReactiveProperty<T> field, Action<T> onChange)
        {
            _bindDisposables.Add(field.Subscribe(onChange));
        }

        protected void Bind<T>(IReadOnlyReactiveProperty<T> field, Action<Pair<T>> onChange)
        {
            BindSilently(field, onChange);
            onChange(new Pair<T>(field.Value, field.Value));
        }

        protected void BindSilently<T>(IReadOnlyReactiveProperty<T> field, Action<T> onChange)
        {
            _bindDisposables.Add(field.SkipLatestValueOnSubscribe().Subscribe(onChange));
        }

        protected void BindSilently<T>(IReadOnlyReactiveProperty<T> field, Action<Pair<T>> onChange)
        {
            _bindDisposables.Add(field.Pairwise().Subscribe(onChange));
        }

        protected void Bind<T>(IReadOnlyReactiveCollection<T> field, Action<int> onChange)
        {
            _bindDisposables.Add(field.ObserveCountChanged().Subscribe(onChange));
            onChange(field.Count);
        }
    }

    public abstract class View<THierarchy> : View
        where THierarchy : MonoBehaviour
    {
        public bool IsActive => Hierarchy.gameObject.activeSelf;
        public THierarchy Hierarchy { get; }

        protected View(GameObject gameObject, IViewFactory viewFactory) : base(viewFactory)
        {
            Hierarchy = gameObject.GetComponent<THierarchy>();
            if (Hierarchy == null)
                Debug.LogError($"Can't find component '{typeof(THierarchy)}' on '{gameObject.name}'");
        }

        public void SetActive(bool active)
        {
            Hierarchy.gameObject.SetActive(active);
        }
    }

    public abstract class View : DisposableCollector
    {
        private readonly IViewFactory _viewFactory;
        private readonly List<View> _childViews = new();

        protected View(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        protected virtual void ReleaseViewModel()
        {
        }

        public virtual void ClearViewModel()
        {
            for (var i = 0; i < _childViews.Count; i++)
            {
                _childViews[i].ClearViewModel();
            }
        }

        protected TView CreateView<TView, THierarchy>(THierarchy hierarchy)
            where TView : View<THierarchy> where THierarchy : MonoBehaviour
        {
            TView view = _viewFactory.CreateView<TView, THierarchy>(hierarchy);

            _childViews.Add(view);
            AddDisposable(view);

            return view;
        }
    }
}