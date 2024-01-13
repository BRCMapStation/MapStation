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
            CreateIconlessTitleBar("MapStation");
            ScrollView = PhoneScrollView.Create(this);
        }

        public override void OnAppEnable() {
            base.OnAppEnable();
            ScrollView.RemoveAllButtons(true);
            var stages = MapDatabase.Instance.maps.Values;
            foreach(var stage in stages) {
                var button = PhoneUIUtility.CreateSimpleButton(stage.Properties.displayName);
                button.OnConfirm += () => {
                    Core.Instance.BaseModule.StageManager.ExitCurrentStage(stage.stageId);
                };
                ScrollView.AddButton(button);
            }
        }
    }
}
