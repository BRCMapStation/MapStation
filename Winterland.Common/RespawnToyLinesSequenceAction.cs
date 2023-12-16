using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SlopCrew.API;
using SlopCrew.Server.XmasEvent;

namespace Winterland.Common {
    public class RespawnToyLinesSequenceAction : SequenceAction {
        [Header("Name of action to jump to if the gift collection is successful.")]
        public string SuccessTarget = "";
        [Header("Name of action to jump to if SlopCrew rejects collection.")]
        public string RejectedTarget = "";
        [Header("Name of action to jump to if can't reach SlopCrew")]
        public string NoConnectionTarget = "";
        public override void Run(bool immediate) {
            base.Run(immediate);
            var api = APIManager.API;
            if (api.Connected) {
                var giftCollectPacket = new XmasClientCollectGiftPacket();
                NetManager.Instance.SendPacket(giftCollectPacket);
                RunSuccess(immediate);
            }
            else {
                RunNoConnection(immediate);
            }
        }

        private void RunSuccess(bool immediate) {
            var progress = WinterProgress.Instance.LocalProgress;
            ToyLineManager.Instance.RespawnAllToyLines();
            progress.Gifts++;
            progress.Save();
            var action = Sequence.Sequence.GetActionByName(SuccessTarget);
            if (action != null)
                action.Run(immediate);
            else
                Finish(immediate);
        }

        private void RunFailure(bool immediate) {
            var action = Sequence.Sequence.GetActionByName(RejectedTarget);
            if (action != null)
                action.Run(immediate);
            else
                Finish(immediate);
        }

        private void RunNoConnection(bool immediate) {
            var action = Sequence.Sequence.GetActionByName(NoConnectionTarget);
            if (action != null)
                action.Run(immediate);
            else
                Finish(immediate);
        }
    }
}
