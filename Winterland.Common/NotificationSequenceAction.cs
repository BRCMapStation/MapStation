using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    public class NotificationSequenceAction : SequenceAction {
        [Header("Text to show in this UI Notification.")]
        public string NotificationText = "";

        public override void Run(bool immediate) {
            base.Run(immediate);
            Core.Instance.UIManager.ShowNotification(NotificationText);
            Finish(immediate);
        }
    }
}
