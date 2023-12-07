using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPI;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    public class DialogSequenceAction : CameraSequenceAction {
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

                customDialog.OnDialogueBegin += () => {
                    if (dialog.AudioClips != null && dialog.AudioClips.Length > 0) {
                        var audioManager = Core.Instance.AudioManager;
                        var clip = dialog.AudioClips[UnityEngine.Random.Range(0, dialog.AudioClips.Length)];
                        audioManager.PlayNonloopingSfx(audioManager.audioSources[5], clip, audioManager.mixerGroups[5], 0f);
                    }
                };

                if (i >= dialogs.Length - 1) {
                    customDialog.OnDialogueBegin += () => {
                        if (Type == DialogType.YesNah)
                            Sequence.RequestYesNoPrompt();
                    };
                    customDialog.OnDialogueEnd += () => {
                        if (Type == DialogType.YesNah) {
                            var yesTarget = Sequence.Sequence.GetActionByName(YesTarget);
                            var noTarget = Sequence.Sequence.GetActionByName(NahTarget);
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
