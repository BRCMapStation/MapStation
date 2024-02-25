using System.Linq;
using CommonAPI.Phone;
using Reptile;
using TMPro;
using Vector3 = UnityEngine.Vector3;

namespace MapStation.Plugin.Phone {
    public class AppMapStation : CustomApp {
        public override void OnAppInit() {
            base.OnAppInit();
            CreateIconlessTitleBar("MapStation", 70f);
            ScrollView = PhoneScrollView.Create(this);
        }

        public override void OnAppEnable() {
            base.OnAppEnable();
            ScrollView.RemoveAllButtons();
            // Local builds first, then sorted alphabetically
            var stages = MapDatabase.Instance.maps.Values.OrderByDescending(map => map.source).ThenBy(map => map.Properties.displayName);
            addButton(Stage.hideout, "Hideout");
            foreach(var stage in stages) {
                addButton(stage.stageId, stage.Properties.displayName, stage.source == MapSource.TestMaps);
            }
            void addButton(Stage stageId, string displayName, bool localBuild = false) {
                var button = PhoneUIUtility.CreateSimpleButton(displayName);
                if(localBuild) {
                    // Ugly, temporary: in lieu of an icon, add the letter L on the corner of the button
                    // "L" for "Local Build"
                    var localLabel = Instantiate(button.Label.transform.gameObject, button.AnimationParent);
                    localLabel.GetComponent<TextMeshProUGUI>().text = "L";
                    var rect = localLabel.RectTransform();
                    rect.localPosition += Vector3.up * 65 + Vector3.left * 65;
                }
                button.OnConfirm += () => {
                    Core.Instance.BaseModule.StageManager.ExitCurrentStage(stageId);
                };
                ScrollView.AddButton(button);
            }
        }
    }
}
