using HMUI;
using System;
using System.Collections.Generic;
using UIEngine.Extensions;
using UnityEngine;
using static UIEngine.Configuration.PluginConfig;

namespace UIEngine.Components
{
    /// <summary>
    /// Responsible for changing material properties that aren't animateable once the button state changes.
    /// </summary>
    public class ButtonMaterialPropertyChanger : MonoBehaviour
    {
        private NoTransitionsButton _button;
        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        private HashSet<(string, string, string, MaterialPropertyType)> _materialInformation = new HashSet<(string, string, string, MaterialPropertyType)>();

        public void Awake()
        {
            _button = GetComponent<NoTransitionsButton>();
            _button.selectionStateDidChangeEvent += _button_selectionStateDidChangeEvent;
        }

        public void Start()
        {
            _button_selectionStateDidChangeEvent(_button.selectionState);
        }

        public void OnDestroy()
        {
            if (_button != null)
            {
                _button.selectionStateDidChangeEvent -= _button_selectionStateDidChangeEvent;
            }
            _settingsType = null;
            _settingsForStateProvider = null;
            _materials.Clear();
            _materialInformation.Clear();
        }

        private Type _settingsType;
        private ISettingsForStateProvider<ISettingsState, NoTransitionsButton.SelectionState> _settingsForStateProvider;
        public void InitWithState<T>(ISettingsForStateProvider<ISettingsState, NoTransitionsButton.SelectionState> settingsForStateProvider) where T : ISettingsState
        {
            _settingsType = typeof(T);
            _settingsForStateProvider = settingsForStateProvider;
        }

        public void AddMaterialPath(string relativePath, string propertyName, string settingsValueName, MaterialPropertyType materialPropertyType = MaterialPropertyType.Color)
        {
            var all = (relativePath, propertyName, settingsValueName, materialPropertyType);
            if (!_materialInformation.Contains(all))
            {
                _materialInformation.Add(all);
            }
        }

        private void _button_selectionStateDidChangeEvent(NoTransitionsButton.SelectionState state)
        {
            if (_settingsType == null || _settingsForStateProvider == null) return;
            
            var settingsState = _settingsForStateProvider.GetSettingsStateForButtonState(state);

            Material mat;

            foreach((string, string, string, MaterialPropertyType) data in _materialInformation)
            {
                string relativePath = data.Item1;
                string propertyName = data.Item2;
                string settingsValueName = data.Item3;
                MaterialPropertyType materialPropertyType = data.Item4;

                if (!_materials.TryGetValue(relativePath, out mat))
                {
                    // Clone Material
                    var iv = gameObject.GetComponentOnChild<ImageView>(relativePath);
                    mat = new Material(iv.material);
                    iv.material = mat;

                    _materials.Add(relativePath, mat);
                }
                // Jank lol
                switch(materialPropertyType)
                {
                    case MaterialPropertyType.VectorFour:
                        mat.SetVector(propertyName, (Vector4) _settingsType.GetProperty(settingsValueName).GetValue(settingsState));
                        break;
                    case MaterialPropertyType.Float:
                        mat.SetFloat(propertyName, (float) _settingsType.GetProperty(settingsValueName).GetValue(settingsState));
                        break;
                    default:
                    case MaterialPropertyType.Color:
                        mat.SetColor(propertyName, (Color) _settingsType.GetProperty(settingsValueName).GetValue(settingsState));
                        break;
                }
                
            }
        }

        public enum MaterialPropertyType
        {
            Color,
            Float,
            VectorFour
        }
    }
}
