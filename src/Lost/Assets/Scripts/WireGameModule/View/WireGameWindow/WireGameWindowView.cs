using System.Collections.Generic;
using MvvmModule;
using UnityEngine;

namespace WireGameModule.View.WireGameWindow
{
    public sealed class WireGameWindowView : View<WireGameWindowHierarchy, IWireGameWindowViewModel>
    {
        private List<ConnectPointView> _pointsViewA = new();
        private List<ConnectPointView> _pointsViewB = new();
        private const string POINT_PREFAB_NAME = "WireConnectPoint";
        private const string WIRE_PREFAB_NAME = "WireConnectLine";

        public WireGameWindowView(GameObject gameObject, IViewFactory viewFactory) : base(gameObject, viewFactory)
        {
        }

        protected override void UpdateViewModel(IWireGameWindowViewModel viewModel)
        {
            Bind(viewModel.CurrentSum, OnCurrentSumChange);
            CreatePointViews(_pointsViewA, viewModel.PointsA);
            CreatePointViews(_pointsViewB, viewModel.PointsB);
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
            
        }
    }
}