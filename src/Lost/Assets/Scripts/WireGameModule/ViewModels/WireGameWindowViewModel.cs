using System;
using System.Collections.Generic;
using MvvmModule;
using UniRx;
using UnityEngine;
using WireGameModule.Infrastructure;
using WireGameModule.Model;
using WireGameModule.View;
using WireGameModule.View.WireGameWindow;

namespace WireGameModule.ViewModels
{
    public sealed class WireGameWindowViewModel : ViewModel<int>, IWireGameWindowViewModel
    {
        private readonly WireGameLevelData _wireGameLevelData;
        private readonly List<ConnectPointViewModel> _pointsA;
        private readonly List<ConnectPointViewModel> _pointsB;
        private EPointGroup _selectedGroup = EPointGroup.None;
        private int _selectedIndex;

        public Sprite BackSprite { get; }
        public IReadOnlyList<IConnectPointViewModel> PointsA => _pointsA;
        public IReadOnlyList<IConnectPointViewModel> PointsB => _pointsB;
        public IReadOnlyList<PointPair> Connections { get; }
        public int TargetSum { get; }
        public IReadOnlyReactiveProperty<int> CurrentSum { get; }

        public WireGameWindowViewModel(int levelNumber, IViewModelFactory viewModelFactory,
            WireGameLevelHolder wireGameLevelHolder) : base(levelNumber, viewModelFactory)
        {
            _wireGameLevelData = wireGameLevelHolder.GetLevel(levelNumber);

            BackSprite = _wireGameLevelData.BackSprite;
            Connections = _wireGameLevelData.Connections;
            TargetSum = _wireGameLevelData.TargetSum;
            CurrentSum = _wireGameLevelData.CurrentSum;

            List<Vector3> pointsA = _wireGameLevelData.PointsA;
            _pointsA = new List<ConnectPointViewModel>(pointsA.Count);
            CreatePointViewModels(pointsA, EPointGroup.A);

            List<Vector3> pointsB = _wireGameLevelData.PointsB;
            _pointsB = new List<ConnectPointViewModel>(pointsB.Count);
            CreatePointViewModels(pointsB, EPointGroup.B);
        }

        private void CreatePointViewModels(List<Vector3> points, EPointGroup pointGroup)
        {
            for (var index = 0; index < points.Count; index++)
            {
                Vector3 vector3 = points[index];
                var startCondition = CalcPointCondition(Connections, index, pointGroup);
                var args = new ConnectPointViewModelArgs(pointGroup, index, vector3, startCondition);

                ConnectPointViewModel connectPointViewModel =
                    CreateViewModel<ConnectPointViewModel, ConnectPointViewModelArgs>(args);

                connectPointViewModel.Clicked += ConnectPointViewModelOnClicked;
                List<ConnectPointViewModel> viewModels = GetPointsViewModels(pointGroup);

                viewModels.Add(connectPointViewModel);
            }
        }

        private List<ConnectPointViewModel> GetPointsViewModels(EPointGroup pointGroup)
        {
            var viewModels = pointGroup switch
            {
                EPointGroup.A => _pointsA,
                EPointGroup.B => _pointsB,
                _ => throw new ArgumentOutOfRangeException(nameof(pointGroup), pointGroup, null)
            };

            return viewModels;
        }

        private void ConnectPointViewModelOnClicked(EPointGroup group, int index)
        {
            bool hasNotSelected = _selectedGroup == EPointGroup.None;
            bool isSelectedAnotherGroup = _selectedGroup != group;
            bool isClickedSamePoint = _selectedGroup == group && _selectedIndex == index;

            if (hasNotSelected || isSelectedAnotherGroup)
            {
                _selectedGroup = group;
                _selectedIndex = index;
            }
            else if (isClickedSamePoint)
            {
                _selectedGroup = EPointGroup.None;
            }
            else
            {
                _wireGameLevelData.SwapConnections(group, _selectedIndex, index);
                _selectedGroup = EPointGroup.None;
            }

            UpdatePointsCondition();
        }

        private void UpdatePointsCondition()
        {
            for (var index = 0; index < _pointsA.Count; index++)
            {
                ConnectPointViewModel connectPointViewModel = _pointsA[index];
                EPointCondition pointCondition = CalcPointCondition(Connections, index, EPointGroup.A);
                connectPointViewModel.SetPointCondition(pointCondition);
            }

            for (var index = 0; index < _pointsB.Count; index++)
            {
                ConnectPointViewModel connectPointViewModel = _pointsB[index];
                EPointCondition pointCondition = CalcPointCondition(Connections, index, EPointGroup.B);
                connectPointViewModel.SetPointCondition(pointCondition);
            }
        }

        private EPointCondition CalcPointCondition(IReadOnlyList<PointPair> connections, int index, EPointGroup pointGroup)
        {
            if (_selectedGroup == pointGroup)
            {
                return index == _selectedIndex
                    ? EPointCondition.Selected
                    : EPointCondition.Available;
            }

            if (pointGroup == EPointGroup.A)
            {
                foreach (PointPair pointPair in connections)
                    if (pointPair.IndexA == index)
                        return EPointCondition.Used;

                return EPointCondition.Unused;
            }

            if (pointGroup == EPointGroup.B)
            {
                foreach (PointPair pointPair in connections)
                    if (pointPair.IndexB == index)
                        return EPointCondition.Used;

                return EPointCondition.Unused;
            }

            throw new Exception($"Unavailable point group '{pointGroup}'");
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (ConnectPointViewModel connectPointViewModel in _pointsA)
                connectPointViewModel.Clicked -= ConnectPointViewModelOnClicked;

            foreach (ConnectPointViewModel connectPointViewModel in _pointsB)
                connectPointViewModel.Clicked -= ConnectPointViewModelOnClicked;
        }
    }
}