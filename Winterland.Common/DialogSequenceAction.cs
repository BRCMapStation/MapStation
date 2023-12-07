using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPI;
using UnityEngine;

namespace Winterland.Common {
    public class DialogSequenceAction : SequenceAction {
        public enum DialogType {
            Normal,
            YesNah
        }
        [Header("Type of dialog. Can make a branching question.")]
        public DialogType Type = DialogType.Normal;
        [HideInInspector]
        public string YesTarget = "";
        [HideInInspector]
        public string NahTarget = "";
        public override void Run() {
            base.Run();
            var dialogs = GetComponents<DialogBlock>().Where((block) => block.Owner == this).ToArray();
            var customDialogs = new List<CustomDialogue>();
            for(var i=0;i<dialogs.Length;i++) {
                var dialog = dialogs[i];

                var customDialog = new CustomDialogue(dialog.SpeakerName, dialog.Text);
                customDialogs.Add(customDialog);

                if (dialog.Speaker == DialogBlock.SpeakerMode.None)
                    customDialog.CharacterName = "";
                if (dialog.Speaker == DialogBlock.SpeakerMode.NPC)
                    customDialog.CharacterName = NPC.Name;

                if (i > 0) {
                    var prevDialogue = customDialogs[i - 1];
                    if (prevDialogue != null)
                        prevDialogue.NextDialogue = customDialog;
                }

                if (i >= dialogs.Length - 1) {
                    if (Type == DialogType.YesNah) {
                        customDialog.OnDialogueBegin = () => {
                            Sequence.RequestYesNoPrompt();
                        };
                    }
                    customDialog.OnDialogueEnd = () => {
                        if (Type == DialogType.YesNah) {
                            var yesTarget = GetActionByName(YesTarget);
                            var noTarget = GetActionByName(NahTarget);
                            if (customDialog.AnsweredYes) {
                                if (yesTarget == null)
                                    Finish();
                                else
                                    yesTarget.Run();
                            }
                            else {
                                if (noTarget == null)
                                    Finish();
                                else
                                    noTarget.Run();
                            }
                        }
                        else
                            Finish();
                    };
                }
            }

            if (customDialogs.Count > 0)
                Sequence.StartDialogue(customDialogs[0]);
        }
    }
}
