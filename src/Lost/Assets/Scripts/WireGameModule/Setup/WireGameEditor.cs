using System.Collections.Generic;
using System.Linq;
using Moq;
using MvvmModule;
using SettingsModule;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using WireGameModule.View;

namespace WireGameModule.Setup
{
    internal sealed class WireGameEditor : MonoBehaviour
    {
        private bool _hasLevel;
        private WireGameLevel _previousWireGameLevel;
        private readonly List<ConnectPointView> _pointsViewA = new();
        private readonly List<ConnectPointView> _pointsViewB = new();

        public WireGameLevel WireGameLevel;

        [ShowIf(nameof(_hasLevel))]
        public Sprite BackSprite;

        [FoldoutGroup("Components"), PropertyOrder(3)]
        public Image BackImage;

        [FoldoutGroup("Components"), PropertyOrder(3)]
        public WireConnectPointHierarchy PointPrefab;

        [FoldoutGroup("Components"), PropertyOrder(3)]
        public GameSettings GameSettings;

        [Button]
        [ShowIf(nameof(_hasLevel)), PropertyOrder(2)]
        private void SaveData()
        {
            SaveData(WireGameLevel);
        }

        [Button, HorizontalGroup("Change count"), ShowIf(nameof(_hasLevel)), PropertyOrder(1)]
        private void AddA() => AddPointToCollection("A", _pointsViewA);

        [Button, HorizontalGroup("Change count"), ShowIf(nameof(_hasLevel)), PropertyOrder(1)]
        private void RemoveA() => RemoveLastPointFromCollection(_pointsViewA);

        [Button, HorizontalGroup("Change count"), ShowIf(nameof(_hasLevel)), PropertyOrder(1)]
        private void AddB() => AddPointToCollection("B", _pointsViewB);

        [Button, HorizontalGroup("Change count"), ShowIf(nameof(_hasLevel)), PropertyOrder(1)]
        private void RemoveB() => RemoveLastPointFromCollection(_pointsViewB);

        private void AddPointToCollection(string prefix, List<ConnectPointView> connectPointViews)
        {
            foreach (ConnectPointView pointView in connectPointViews)
            {
                if (pointView.Hierarchy.gameObject.activeSelf == false)
                {
                    pointView.SetActive(true);
                    return;
                }
            }

            ConnectPointView connectPointView = CreateConnectPointView();
            IConnectPointViewModel viewModel = new SetupConnectPointViewModel(Vector3.zero, $"{prefix}{connectPointViews.Count}");
            connectPointView.Initialize(viewModel);
            connectPointViews.Add(connectPointView);
        }

        private static void RemoveLastPointFromCollection(List<ConnectPointView> connectPointViews)
        {
            int lastIndex = connectPointViews.Count - 1;
            if (lastIndex < 0)
                return;

            DestroyPoint(connectPointViews[lastIndex]);
            connectPointViews.RemoveAt(lastIndex);
        }

        private void SaveData(WireGameLevel wireGameLevel)
        {
            wireGameLevel.BackSprite = BackSprite;

            wireGameLevel.PointsA = _pointsViewA
                .Where(a => a.Hierarchy.gameObject.activeSelf)
                .Select(a => a.Hierarchy.transform.position)
                .ToList();

            wireGameLevel.PointsB = _pointsViewB
                .Where(a => a.Hierarchy.gameObject.activeSelf)
                .Select(a => a.Hierarchy.transform.position)
                .ToList();
        }

        private void OnValidate()
        {
            _hasLevel = WireGameLevel != null;

            if (_hasLevel && _previousWireGameLevel != WireGameLevel)
            {
                if (_previousWireGameLevel != null)
                    SaveData(_previousWireGameLevel);

                _previousWireGameLevel = WireGameLevel;
                SetupEditor();
            }

            BackImage.sprite = BackSprite;
        }

        private void SetupEditor()
        {
            BackSprite = WireGameLevel.BackSprite;

            RemoveOldPointHierarchy();

            UpdatePointsPosition(WireGameLevel.PointsA, _pointsViewA, "A");
            UpdatePointsPosition(WireGameLevel.PointsB, _pointsViewB, "B");
        }

        private void RemoveOldPointHierarchy()
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (!child.TryGetComponent(typeof(WireConnectPointHierarchy), out var component))
                    continue;

                var found = false;
                foreach (ConnectPointView connectPointView in _pointsViewA)
                    if (connectPointView.Hierarchy == component)
                    {
                        found = true;
                        break;
                    }

                foreach (ConnectPointView connectPointView in _pointsViewB)
                    if (connectPointView.Hierarchy == component || found)
                    {
                        found = true;
                        break;
                    }

                if (!found)
                    DestroyImmediate(component.gameObject);
            }
        }

        private void UpdatePointsPosition(List<Vector3> pointPositions, List<ConnectPointView> pointViews, string prefix)
        {
            for (int i = pointViews.Count; i < pointPositions.Count; i++)
            {
                ConnectPointView pointView = CreateConnectPointView();
                pointViews.Add(pointView);
            }

            for (var i = 0; i < pointViews.Count; i++)
            {
                pointViews[i].SetActive(false);
            }

            for (var i = 0; i < pointPositions.Count; i++)
            {
                Vector3 pointPosition = pointPositions[i];
                ConnectPointView connectPointView = pointViews[i];
                var text = $"{prefix}{i}";
                IConnectPointViewModel viewModel = new SetupConnectPointViewModel(pointPosition, text);
                connectPointView.Initialize(viewModel);
                connectPointView.SetActive(true);
            }
        }

        private ConnectPointView CreateConnectPointView()
        {
            WireConnectPointHierarchy wireConnectPointHierarchy = Instantiate(PointPrefab, transform);
            var asd = Mock.Of<IViewFactory>();
            var pointView = new ConnectPointView(wireConnectPointHierarchy.gameObject, asd, GameSettings);
            return pointView;
        }

        private void OnDestroy()
        {
            foreach (ConnectPointView connectPointView in _pointsViewA)
                DestroyPoint(connectPointView);

            _pointsViewA.Clear();

            foreach (ConnectPointView connectPointView in _pointsViewB)
                DestroyPoint(connectPointView);

            _pointsViewB.Clear();
        }

        private static void DestroyPoint(ConnectPointView connectPointView)
        {
            connectPointView.ClearViewModel();
            DestroyImmediate(connectPointView.Hierarchy);
        }
    }
}