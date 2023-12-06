using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPI;

namespace Winterland.Common {
    public class DialogSequenceAction : SequenceAction {
        public override void Run() {
            base.Run();
            var dialogs = GetComponents<DialogBlock>();
            var customDialogs = new List<CustomDialogue>();
            for(var i=0;i<dialogs.Length;i++) {
                var dialog = dialogs[i];

                var customDialog = new CustomDialogue(dialog.SpeakerName, dialog.Text);
                customDialogs.Add(customDialog);

                if (dialog.Mode == DialogBlock.SpeakerMode.None)
                    customDialog.CharacterName = "";
                if (dialog.Mode == DialogBlock.SpeakerMode.NPC)
                    customDialog.CharacterName = NPC.Name;

                if (i > 0) {
                    var prevDialogue = customDialogs[i - 1];
                    if (prevDialogue != null)
                        prevDialogue.NextDialogue = customDialog;
                }

                if (i >= dialogs.Length - 1) {
                    customDialog.OnDialogueEnd = () => {
                        Finish();
                    };
                }
            }

            if (customDialogs.Count > 0)
                Sequence.StartDialogue(customDialogs[0]);
        }
    }
}
