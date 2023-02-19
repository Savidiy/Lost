using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MvvmModule;
using SettingsModule;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WireGameModule.Model;
using WireGameModule.View;
using Random = UnityEngine.Random;

namespace WireGameModule.Setup
{
    [ExecuteInEditMode]
    internal sealed class WireGameEditor : MonoBehaviour
    {
        private const int START_ORDER = 10;
        private const int VALUES_ORDER = 20;
        private const string RANDOM_VALUES_GROUP = nameof(RANDOM_VALUES_GROUP);
        private const int RANDOM_VALUES_ORDER = 30;
        private const int CONNECTION_ORDER = 40;
        private const int RANDOM_CONNECTION_ORDER = 50;
        private const int STATISTICS_ORDER = 55;
        private const string CHANGE_POINTS_GROUP = nameof(CHANGE_POINTS_GROUP);
        private const int CHANGE_POINTS_ORDER = 51;
        private const int SAVE_DATA_ORDER = 52;
        private const int COMPONENTS_ORDER = 100;
        private bool _hasLevel;
        private WireGameLevel _previousWireGameLevel;
        private readonly List<ConnectPointView> _pointsViewA = new();
        private readonly List<ConnectPointView> _pointsViewB = new();
        private readonly WireGameStatistics _wireGameStatistics = new();

        public WireGameLevel WireGameLevel;

        [ShowIf(nameof(_hasLevel))]
        public Sprite BackSprite;

        [ShowIf(nameof(_hasLevel)), PropertyOrder(CONNECTION_ORDER)]
        public List<PointPair> StartConnections = new();

        [ShowIf(nameof(_hasLevel)), PropertyOrder(CONNECTION_ORDER)]
        public int TargetSum;

        private int _previousTargetSum;

        [ShowIf(nameof(_hasLevel)), ShowInInspector, PropertyOrder(VALUES_ORDER)]
        [TableMatrix(HorizontalTitle = "A points", VerticalTitle = "B points")]
        public int[,] ConnectsValue = new int[0, 0];

        [FoldoutGroup("Components"), PropertyOrder(COMPONENTS_ORDER)]
        public Image BackImage;

        [FoldoutGroup("Components"), PropertyOrder(COMPONENTS_ORDER)]
        public WireConnectPointHierarchy PointPrefab;

        [FoldoutGroup("Components"), PropertyOrder(COMPONENTS_ORDER)]
        public GameSettings GameSettings;

        [Button]
        [ShowIf(nameof(_hasLevel)), PropertyOrder(SAVE_DATA_ORDER)]
        private void SaveData() => SaveData(WireGameLevel);

        [Button]
        [ShowIf(nameof(_hasLevel)), PropertyOrder(START_ORDER)]
        private void RefreshData() => SetupEditor(WireGameLevel);

        [LabelText("Min"), LabelWidth(40), HorizontalGroup(RANDOM_VALUES_GROUP), ShowIf(nameof(_hasLevel)),
         PropertyOrder(RANDOM_VALUES_ORDER)]
        public int MinRandomValue = 1;

        [LabelText("Max"), LabelWidth(40), HorizontalGroup(RANDOM_VALUES_GROUP), ShowIf(nameof(_hasLevel)),
         PropertyOrder(RANDOM_VALUES_ORDER)]
        public int MaxRandomValue = 4;

        [Button, HorizontalGroup(RANDOM_VALUES_GROUP), ShowIf(nameof(_hasLevel)), PropertyOrder(RANDOM_VALUES_ORDER)]
        public void RandomValues()
        {
            int lengthA = ConnectsValue.GetLength(0);
            int lengthB = ConnectsValue.GetLength(1);

            for (int x = 0; x < lengthA; x++)
            for (int y = 0; y < lengthB; y++)
                ConnectsValue[x, y] = Random.Range(MinRandomValue, MaxRandomValue + 1);

            SaveData(WireGameLevel);
            SetupEditor(WireGameLevel);
        }

        [Button, ShowIf(nameof(_hasLevel)), PropertyOrder(RANDOM_CONNECTION_ORDER)]
        private void RandomConnections()
        {
            var indexesA = new List<int>();
            int pointsACount = WireGameLevel.PointsA.Count;
            for (int i = 0; i < pointsACount; i++)
                indexesA.Add(i);

            var indexesB = new List<int>();
            int pointsBCount = WireGameLevel.PointsB.Count;
            for (int i = 0; i < pointsBCount; i++)
                indexesB.Add(i);

            int connectionCount = Math.Min(StartConnections.Count, Math.Min(pointsACount, pointsBCount));
            StartConnections.Clear();
            for (int i = 0; i < connectionCount; i++)
            {
                int a = Random.Range(0, indexesA.Count);
                int b = Random.Range(0, indexesB.Count);
                StartConnections.Add(new PointPair(indexesA[a], indexesB[b]));
                indexesA.RemoveAt(a);
                indexesB.RemoveAt(b);
            }

            SaveData(WireGameLevel);
            SetupEditor(WireGameLevel);
        }

        [ReadOnly, PropertyOrder(STATISTICS_ORDER)]
        public List<string> Statistics = new();

        [Button, HorizontalGroup(CHANGE_POINTS_GROUP), ShowIf(nameof(_hasLevel)), PropertyOrder(CHANGE_POINTS_ORDER)]
        private void AddA() => AddPointToCollection("A", _pointsViewA);

        [Button, HorizontalGroup(CHANGE_POINTS_GROUP), ShowIf(nameof(_hasLevel)), PropertyOrder(CHANGE_POINTS_ORDER)]
        private void RemoveA() => RemoveLastPointFromCollection(_pointsViewA);

        [Button, HorizontalGroup(CHANGE_POINTS_GROUP), ShowIf(nameof(_hasLevel)), PropertyOrder(CHANGE_POINTS_ORDER)]
        private void AddB() => AddPointToCollection("B", _pointsViewB);

        [Button, HorizontalGroup(CHANGE_POINTS_GROUP), ShowIf(nameof(_hasLevel)), PropertyOrder(CHANGE_POINTS_ORDER)]
        private void RemoveB() => RemoveLastPointFromCollection(_pointsViewB);

        private void AddPointToCollection(string prefix, List<ConnectPointView> connectPointViews)
        {
            ConnectPointView connectPointView = CreateOrActivatePoint(prefix, connectPointViews);

            connectPointView.Hierarchy.transform.position = transform.position;

            SaveData(WireGameLevel);
            SetupEditor(WireGameLevel);
        }

        private ConnectPointView CreateOrActivatePoint(string prefix, List<ConnectPointView> connectPointViews)
        {
            foreach (ConnectPointView pointView in connectPointViews)
            {
                if (pointView.Hierarchy.gameObject.activeSelf == false)
                {
                    pointView.SetActive(true);
                    return pointView;
                }
            }

            ConnectPointView connectPointView = CreateConnectPointView();
            IConnectPointViewModel viewModel = new SetupConnectPointViewModel(Vector3.zero, $"{prefix}{connectPointViews.Count}");
            connectPointView.Initialize(viewModel);
            connectPointViews.Add(connectPointView);
            return connectPointView;
        }

        private void RemoveLastPointFromCollection(List<ConnectPointView> connectPointViews)
        {
            for (int i = connectPointViews.Count - 1; i >= 0; i--)
            {
                ConnectPointView connectPointView = connectPointViews[i];
                if (connectPointView.IsActive)
                {
                    connectPointView.SetActive(false);
                    break;
                }
            }

            SaveData(WireGameLevel);
            SetupEditor(WireGameLevel);
        }

        private void SaveData(WireGameLevel wireGameLevel)
        {
            wireGameLevel.BackSprite = BackSprite;

            wireGameLevel.PointsA = _pointsViewA
                .Where(a => a.IsActive)
                .Select(a => a.Hierarchy.transform.position)
                .ToList();

            wireGameLevel.PointsB = _pointsViewB
                .Where(a => a.IsActive)
                .Select(a => a.Hierarchy.transform.position)
                .ToList();

            wireGameLevel.ConnectsValue = ConnectsValue;
            wireGameLevel.StartConnections = StartConnections;
            wireGameLevel.TargetSum = TargetSum;

            EditorUtility.SetDirty(wireGameLevel);
            AssetDatabase.SaveAssetIfDirty(wireGameLevel);
        }

        private void OnValidate()
        {
            _hasLevel = WireGameLevel != null;

            if (_hasLevel)
            {
                bool isLevelChanged = _previousWireGameLevel != WireGameLevel;
                if (isLevelChanged)
                {
                    if (_previousWireGameLevel != null)
                    {
                        SetupEditor(_previousWireGameLevel);
                        SaveData(_previousWireGameLevel);
                    }

                    _previousWireGameLevel = WireGameLevel;
                    SetupEditor(WireGameLevel);
                }
                else
                {
                    WireGameLevel.TargetSum = TargetSum;
                    SetupEditor(WireGameLevel);
                }
            }

            BackImage.sprite = BackSprite;
        }

        private int[,] CloneArray(int[,] fromArray)
        {
            int countA = fromArray.GetLength(0);
            int countB = fromArray.GetLength(1);
            var ints = new int [countA, countB];

            for (int i = 0; i < countA ; i++)
            for (int j = 0; j < countB ; j++)
                ints[i, j] = fromArray[i, j];

            return ints;
        }

        private void SetupEditor(WireGameLevel wireGameLevel)
        {
            BackSprite = wireGameLevel.BackSprite;
            StartConnections = wireGameLevel.StartConnections.ToList();
            ConnectsValue = CloneArray(wireGameLevel.ConnectsValue);
            TargetSum = wireGameLevel.TargetSum;

            RemoveOldPointHierarchy();

            UpdatePointsPosition(wireGameLevel.PointsA, _pointsViewA, "A");
            UpdatePointsPosition(wireGameLevel.PointsB, _pointsViewB, "B");

            RemoveUselessStartConnections();
            int maxSum = _wireGameStatistics.UpdateStatistics(Statistics, StartConnections, ConnectsValue);
        }

        private void RemoveUselessStartConnections()
        {
            int maxCount = Math.Min(ConnectsValue.GetLength(0), ConnectsValue.GetLength(1));

            int uselessCount = StartConnections.Count - maxCount;
            if (uselessCount > 0)
                StartConnections.RemoveRange(maxCount - 1, uselessCount);
        }

        private void RemoveOldPointHierarchy()
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (!child.TryGetComponent(out WireConnectPointHierarchy pointHierarchy))
                    continue;

                var found = false;
                foreach (ConnectPointView connectPointView in _pointsViewA)
                    if (connectPointView.Hierarchy == pointHierarchy)
                    {
                        found = true;
                        break;
                    }

                foreach (ConnectPointView connectPointView in _pointsViewB)
                    if (connectPointView.Hierarchy == pointHierarchy || found)
                    {
                        found = true;
                        break;
                    }

                if (!found)
                {
                    var viewFactoryMock = Mock.Of<IViewFactory>();
                    var pointView = new ConnectPointView(pointHierarchy.gameObject, viewFactoryMock, GameSettings);
                    var targetList = _pointsViewA.Count < _pointsViewB.Count ? _pointsViewA : _pointsViewB;
                    string label = targetList == _pointsViewA ? $"A{_pointsViewA.Count}" : $"B{_pointsViewB.Count}";
                    IConnectPointViewModel viewModel = new SetupConnectPointViewModel(Vector3.zero, label);
                    pointView.Initialize(viewModel);
                    targetList.Add(pointView);
                }
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
            var viewFactoryMock = Mock.Of<IViewFactory>();
            var pointView = new ConnectPointView(wireConnectPointHierarchy.gameObject, viewFactoryMock, GameSettings);
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
            Debug.Log($"Destroy point '{connectPointView.ViewModel.Text}'");
            connectPointView.ClearViewModel();
            DestroyImmediate(connectPointView.Hierarchy.gameObject);
        }
    }
}