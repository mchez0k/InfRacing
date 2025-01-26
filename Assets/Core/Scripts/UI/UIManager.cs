using Core.Systems;
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
        [SerializeField] private PanelBase[] panels;
        [SerializeField] private int firstPanelIndex;
        private readonly Dictionary<Type, PanelBase> panelsDictionary = new();
        private readonly LinkedList<Type> previousPanels = new();
        private Type currentPanel;
        public Type CurrentPanel => currentPanel;
        public Type PreviousPanel => previousPanels.Count == 0 ? null : previousPanels.Last.Value;
        public bool IsInitialized { get; private set; }

        private InfoPanel infoPanel;

        #region Public methods

        public void Initialize()
        {
            InitializePanels();
            WarmupUI();
            DisableAll();
            OpenLobby();
            IsInitialized = true;
        }

        public void Open(Type type, bool savePrevious = true)
        {
            if (!panelsDictionary.ContainsKey(type) || currentPanel == type)
                return;
            DisableAll();
            if (savePrevious)
                previousPanels.AddLast(currentPanel);
            currentPanel = type;
            panelsDictionary[currentPanel].Open();
        }

        public void Back()
        {
            if (previousPanels.Last == null)
                return;
            DisableAll();
            currentPanel = previousPanels.Last.Value;
            previousPanels.RemoveLast();
            panelsDictionary[currentPanel].Open();
        }


        public void ClearPrevious(bool all = false)
        {
            if (all)
                previousPanels.Clear();
            else
                previousPanels.RemoveLast();
        }

        public T GetPanel<T>() where T : PanelBase
        {
            if (!panelsDictionary.ContainsKey(typeof(T)))
                return null;
            return panelsDictionary[typeof(T)] as T;
        }

        public void SetPrevPanel(Type panel)
        {
            previousPanels.Remove(previousPanels.Last);
            previousPanels.AddLast(panel);
        }

        #endregion

        #region Private methods

        private void InitializePanels()
        {
            foreach (var panel in panels)
            {
                panelsDictionary[panel.GetType()] = panel;
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
            foreach (var panel in panels)
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