using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SlopCrew.API;
using SlopCrew.Server.XmasEvent;
using CommonAPI;
using Reptile;
using System.Collections;

namespace Winterland.Common {
    public class RespawnToyLinesSequenceAction : SequenceAction {
        [Header("Name of action to jump to if the gift collection is successful.")]
        public string SuccessTarget = "";
        [Header("Name of action to jump to if SlopCrew rejects collection.")]
        public string RejectedTarget = "";
        [Header("Name of action to jump to if can't reach SlopCrew")]
        public string NoConnectionTarget = "";
        private bool immediate;
        private const float TimeOutSeconds = 5f;
        public override void Run(bool immediate) {
            base.Run(immediate);
            this.immediate = immediate;
            var sequenceHandler = CustomSequenceHandler.instance;
            var api = APIManager.API;
            if (api.Connected) {
                sequenceHandler.skipTextActiveState = SequenceHandler.SkipState.NOT_SKIPPABLE;
                var giftCollectPacket = new XmasClientCollectGiftPacket();
                NetManager.Instance.SendPacket(giftCollectPacket);
                NetManager.Instance.OnPacket += ListenForPacket;
                StartCoroutine(RunTimeout());
            }
            else {
                RunNoConnection(immediate);
            }
        }

        private IEnumerator RunTimeout() {
            yield return new WaitForSeconds(TimeOutSeconds);
            if (Sequence.CurrentAction == this) {
                RunNoConnection(immediate);
            }
        }

        private void ListenForPacket(XmasPacket packet) {
            if (packet.PlayerID != uint.MaxValue)
                return;
            if (packet is XmasServerAcceptGiftPacket) {
                RunSuccess(immediate);
            }
            else if (packet is XmasServerRejectGiftPacket) {
                RunFailure(immediate);
            }
        }

        private void RunSuccess(bool immediate) {
            NetManager.Instance.OnPacket -= ListenForPacket;
            StopAllCoroutines();
            if (Sequence.Sequence.Skippable)
                CustomSequenceHandler.instance.skipTextActiveState = SequenceHandler.SkipState.IDLE;
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
            NetManager.Instance.OnPacket -= ListenForPacket;
            StopAllCoroutines();
            if (Sequence.Sequence.Skippable)
                CustomSequenceHandler.instance.skipTextActiveState = SequenceHandler.SkipState.IDLE;
            var action = Sequence.Sequence.GetActionByName(RejectedTarget);
            if (action != null)
                action.Run(immediate);
            else
                Finish(immediate);
        }

        private void RunNoConnection(bool immediate) {
            NetManager.Instance.OnPacket -= ListenForPacket;
            StopAllCoroutines();
            if (Sequence.Sequence.Skippable)
                CustomSequenceHandler.instance.skipTextActiveState = SequenceHandler.SkipState.IDLE;
            var action = Sequence.Sequence.GetActionByName(NoConnectionTarget);
            if (action != null)
                action.Run(immediate);
            else
                Finish(immediate);
        }
    }
}
