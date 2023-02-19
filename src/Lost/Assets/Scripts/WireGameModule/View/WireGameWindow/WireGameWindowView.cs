using System.Collections.Generic;
using MvvmModule;
using UnityEngine;

namespace WireGameModule.View.WireGameWindow
{
    public sealed class WireGameWindowView : View<WireGameWindowHierarchy, IWireGameWindowViewModel>
    {
        private readonly List<ConnectPointView> _pointsViewA = new();
        private readonly List<ConnectPointView> _pointsViewB = new();
        private readonly List<WireView> _wireViews = new();

        private const string POINT_PREFAB_NAME = "WireConnectPoint";
        private const string WIRE_PREFAB_NAME = "WireConnectLine";

        public WireGameWindowView(GameObject gameObject, IViewFactory viewFactory) : base(gameObject, viewFactory)
        {
        }

        protected override void UpdateViewModel(IWireGameWindowViewModel viewModel)
        {
            Hierarchy.BackImage.sprite = viewModel.BackSprite;

            Bind(viewModel.CurrentSum, OnCurrentSumChange);
            CreatePointViews(_pointsViewA, viewModel.PointsA);
            CreatePointViews(_pointsViewB, viewModel.PointsB);
            CreateWireViews(viewModel.WireViewModels);
        }

        private void CreateWireViews(IReadOnlyList<IWireViewModel> wireViewModels)
        {
            foreach (IWireViewModel wireViewModel in wireViewModels)
            {
                WireView wireView = CreateView<WireView, WireGameWireHierarchy>(WIRE_PREFAB_NAME, Hierarchy.WiresRoot);
                wireView.Initialize(wireViewModel);
                _wireViews.Add(wireView);
            }
        }

        private void CreatePointViews(List<ConnectPointView> connectPointViews,
            IReadOnlyList<IConnectPointViewModel> connectPointViewModels)
        {
            foreach (IConnectPointViewModel connectPointViewModel in connectPointViewModels)
            {
                ConnectPointView connectPointView = CreateConnectPointView();
                connectPointView.Initialize(connectPointViewModel);
                connectPointViews.Add(connectPointView);
            }
        }

        private ConnectPointView CreateConnectPointView()
        {
            return CreateView<ConnectPointView, WireGameConnectPointHierarchy>(POINT_PREFAB_NAME, Hierarchy.PointsRoot);
        }

        private void OnCurrentSumChange(int currentSum)
        {
            float normalizedProgress = (float) currentSum / ViewModel.TargetSum;
            Hierarchy.SimpleProgressBar.SetProgress(normalizedProgress);
        }

        protected override void ReleaseViewModel()
        {
            base.ReleaseViewModel();
            foreach (ConnectPointView connectPointView in _pointsViewA)
                connectPointView.Dispose();

            _pointsViewA.Clear();
            
            foreach (ConnectPointView connectPointView in _pointsViewB)
                connectPointView.Dispose();

            _pointsViewB.Clear();

            foreach (WireView wireView in _wireViews) 
                wireView.Dispose();
            
            _wireViews.Clear();
        }
    }
}