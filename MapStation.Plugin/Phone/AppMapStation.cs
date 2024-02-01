using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPI.Phone;
using Reptile;

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
            var stages = MapDatabase.Instance.maps.Values;
            addButton(Stage.hideout, "Hideout");
            foreach(var stage in stages) {
                addButton(stage.stageId, stage.Properties.displayName);
            }
            void addButton(Stage stageId, string displayName) {
                var button = PhoneUIUtility.CreateSimpleButton(displayName);
                button.OnConfirm += () => {
                    Core.Instance.BaseModule.StageManager.ExitCurrentStage(stageId);
                };
                ScrollView.AddButton(button);
            }
        }
    }
}
