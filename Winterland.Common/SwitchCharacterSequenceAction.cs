using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;

namespace Winterland.Common {
    public class SwitchCharacterSequenceAction : SequenceAction {
        public BrcCharacter CharacterToSwitchTo;
        public override void Run(bool immediate) {
            base.Run(immediate);
            var actualCharacter = (Characters)CharacterToSwitchTo;
            if (actualCharacter > Characters.NONE && actualCharacter < Characters.MAX) {
                var currentPlayer = WorldHandler.instance.GetCurrentPlayer();
                if (currentPlayer != null) {
                    var characterProgress = Core.Instance.SaveManager.CurrentSaveSlot.GetCharacterProgress(actualCharacter);
                    currentPlayer.SetCharacter(actualCharacter, characterProgress.outfit);
                    currentPlayer.InitVisual();
                    currentPlayer.SetCurrentMoveStyleEquipped(characterProgress.moveStyle, true, true);
                }
            }
            Finish(immediate);
        }
    }
}
