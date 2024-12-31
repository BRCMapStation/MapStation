using CommonAPI.Phone;
using MapStation.Common.Runtime;
using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Plugin.Phone {
    public class AppMapOptions : CustomApp {
        public override bool Available => MapOptions.Instance != null && MapOptions.Instance.Options.Count() > 0;
        private LoadedMapOptions _loadedMapOptions = LoadedMapOptions.GetCurrentMapOptions();
        private MapOptions _mapOptions = MapOptions.Instance;
        private Dictionary<string, SimplePhoneButton> _buttonByOptionName = new();

        private Camera _currentCamera = null;

        public override void OnAppInit() {
            base.OnAppInit();
            CreateIconlessTitleBar("Map Options", 70f);
            ScrollView = PhoneScrollView.Create(this);
            if (_mapOptions == null) return;
            var resetButton = PhoneUIUtility.CreateSimpleButton("Reset");
            resetButton.OnConfirm += () => {
                ResetOptions();
            };
            ScrollView.AddButton(resetButton);
            foreach(var option in _mapOptions.Options) {
                var button = PhoneUIUtility.CreateSimpleButton(GetOptionString(option.Name));
                button.OnConfirm += () => {
                    ChangeOption(option.Name);
                };
                _buttonByOptionName[option.Name] = button;
                ScrollView.AddButton(button);
            }
            MapOptions.OnMapOptionsChanged -= RefreshButtonNames;
            MapOptions.OnMapOptionsChanged += RefreshButtonNames;
        }

        public override void OnAppUpdate() {
            base.OnAppUpdate();
            UpdateCamera();
            UpdateDesc();
        }

        public override void OnAppDisable() {
            base.OnAppDisable();
            DisableCamera();
            MapOptionDescriptionUI.Instance.gameObject.SetActive(false);
        }

        public override void OnAppTerminate() {
            base.OnAppTerminate();
            DisableCamera();
            MapOptionDescriptionUI.Instance.gameObject.SetActive(false);
        }

        private void UpdateDesc() {
            var descUi = MapOptionDescriptionUI.Instance;
            var currentButton = ScrollView.Buttons[ScrollView.SelectedIndex];
            foreach (var buttonByOption in _buttonByOptionName) {
                if (buttonByOption.Value == currentButton) {
                    var mapOptions = MapOptions.Instance;
                    foreach (var mapOption in mapOptions.Options) {
                        if (mapOption.Name == buttonByOption.Key) {
                            descUi.gameObject.SetActive(true);
                            descUi.SetText(mapOption.Description);
                            return;
                        }
                    }
                    descUi.gameObject.SetActive(false);
                    return;
                }
            }
            descUi.gameObject.SetActive(false);
        }

        private void UpdateCamera() {
            var currentButton = ScrollView.Buttons[ScrollView.SelectedIndex];
            if (currentButton == null) {
                DisableCamera();
                return;
            }
            foreach(var buttonByOption in _buttonByOptionName) {
                if (buttonByOption.Value == currentButton) {
                    var mapOptions = MapOptions.Instance;
                    foreach(var mapOption in mapOptions.Options) {
                        if (mapOption.Name == buttonByOption.Key) {
                            if (mapOption.PreviewCamera == null) {
                                DisableCamera();
                                return;
                            } else {
                                EnableCamera(mapOption.PreviewCamera);
                                return;
                            }
                        }
                    }
                    DisableCamera();
                    return;
                }
            }
            DisableCamera();
        }

        private void DisableCamera() {
            if (_currentCamera != null)
                _currentCamera.gameObject.SetActive(false);
            _currentCamera = null;
        }

        private void EnableCamera(Camera camera) {
            camera.gameObject.SetActive(true);
            _currentCamera = camera;
        }

        private void OnDestroy() {
            MapOptions.OnMapOptionsChanged -= RefreshButtonNames;
        }

        private void ResetOptions() {
            _loadedMapOptions.MakeDefault();
            MapOptions.OnMapOptionsChanged?.Invoke();
        }

        private void ChangeOption(string optionName) {
            var currentValue = _loadedMapOptions.GetOption(optionName);
            foreach(var options in _mapOptions.Options) {
                if (options.Name == optionName) {
                    var optionIndex = Array.IndexOf(options.PossibleValues, currentValue);
                    if (optionIndex < 0)
                        optionIndex = Array.IndexOf(options.PossibleValues, options.DefaultValue);
                    if (optionIndex < 0)
                        optionIndex = 0;
                    optionIndex++;
                    if (optionIndex >= options.PossibleValues.Length)
                        optionIndex = 0;
                    _loadedMapOptions.Options[optionName] = options.PossibleValues[optionIndex];
                    break;
                }
            }
            Core.Instance.SaveManager.SaveCurrentSaveSlot();
            MapOptions.OnMapOptionsChanged?.Invoke();
        }

        private void RefreshButtonNames() {
            foreach(var button in _buttonByOptionName) {
                button.Value.Label.text = GetOptionString(button.Key);
            }
        }

        private string GetOptionString(string optionName) {
            var optionValue = _loadedMapOptions.GetOption(optionName);
            return $"{optionName} = {optionValue}";
        }
    }
}
