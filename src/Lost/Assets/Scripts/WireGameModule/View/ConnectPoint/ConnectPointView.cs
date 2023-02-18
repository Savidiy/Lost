using System;
using MvvmModule;
using SettingsModule;
using UnityEngine;

namespace WireGameModule.View
{
    public sealed class ConnectPointView : View<WireConnectPointHierarchy, IConnectPointViewModel>
    {
        private readonly GameSettings _gameSettings;

        public ConnectPointView(GameObject gameObject, IViewFactory viewFactory, GameSettings gameSettings) : base(gameObject, viewFactory)
        {
            _gameSettings = gameSettings;
        }

        protected override void UpdateViewModel(IConnectPointViewModel viewModel)
        {
            Hierarchy.transform.position = viewModel.Position;
            Hierarchy.Text.text = viewModel.Text;
            Hierarchy.name = $"Point{viewModel.Text}";
            Bind(viewModel.PointCondition, OnPointConditionChange);
            Hierarchy.Button.onClick.AddListener(OnButtonClicked);
        }

        protected override void ReleaseViewModel()
        {
            base.ReleaseViewModel();
            Hierarchy.Button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            ViewModel.ClickFromView();
        }

        private void OnPointConditionChange(EPointCondition pointCondition)
        {
            Color color = pointCondition switch
            {
                EPointCondition.Unused => _gameSettings.ConnectPointUnusedColor,
                EPointCondition.Used => _gameSettings.ConnectPointUsedColor,
                EPointCondition.Selected => _gameSettings.ConnectPointSelectedColor,
                EPointCondition.Available => _gameSettings.ConnectPointAvailableColor,
                _ => throw new ArgumentOutOfRangeException(nameof(pointCondition), pointCondition, null)
            };
            
            Hierarchy.Image.color = color;
        }
    }
}