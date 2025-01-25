using Core.Systems;
using Core.UI;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    /// <summary>
    /// UIManager controls all panels using the reflections to open them
    /// </summary>
    public class UIManager : MonoBehaviour, IInitializable
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [SerializeField] private PanelBase[] _panels;
        [SerializeField] private int _firstPanelIndex;
        private readonly Dictionary<Type, PanelBase> _panelsDictionary = new();
        private readonly LinkedList<Type> _previousPanels = new();
        private Type _currentPanel;
        public Type CurrentPanel => _currentPanel;
        public Type PreviousPanel => _previousPanels.Count == 0 ? null : _previousPanels.Last.Value;
        public bool IsInitialized { get; private set; }

        private InfoPanel infoPanel;

        #region Public methods

        public void Initialize()
        {
            InitializePanels();
            WarmupUI();
            DisableAll();
            OpenLobby();
        }

        public void Open(Type type, bool savePrevious = true)
        {
            if (!_panelsDictionary.ContainsKey(type) || _currentPanel == type)
                return;
            DisableAll();
            if (savePrevious)
                _previousPanels.AddLast(_currentPanel);
            _currentPanel = type;
            _panelsDictionary[_currentPanel].Open();
        }

        public void Back()
        {
            if (_previousPanels.Last == null)
                return;
            DisableAll();
            _currentPanel = _previousPanels.Last.Value;
            _previousPanels.RemoveLast();
            _panelsDictionary[_currentPanel].Open();
        }


        public void ClearPrevious(bool all = false)
        {
            if (all)
                _previousPanels.Clear();
            else
                _previousPanels.RemoveLast();
        }

        public T GetPanel<T>() where T : PanelBase
        {
            if (!_panelsDictionary.ContainsKey(typeof(T)))
                return null;
            return _panelsDictionary[typeof(T)] as T;
        }

        public void SetPrevPanel(Type panel)
        {
            _previousPanels.Remove(_previousPanels.Last);
            _previousPanels.AddLast(panel);
        }

        #endregion

        #region Private methods

        private void InitializePanels()
        {
            foreach (var panel in _panels)
            {
                _panelsDictionary[panel.GetType()] = panel;
                panel.Initialize();
                panel.gameObject.SetActive(true);
            }
        }
        private void WarmupUI()
        {
            Canvas.ForceUpdateCanvases();
        }

        private void DisableAll()
        {
            foreach (var panel in _panels)
            {
                panel.Close();
            }
        }

        private void OpenLobby()
        {
            Open(typeof(MenuPanel));
        }

        #endregion
    }
}