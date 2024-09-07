using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Reptile
{
    // Token: 0x020001D2 RID: 466
    public class NPC : GameplayEvent
    {
        // Token: 0x04000E3A RID: 3642
        [Header("It turns itself off if it's not open yet")]
        [Header("Put any trigger inside the NPC")]
        private float openTimer;

        // Token: 0x04000E3B RID: 3643
        public string anim;

        // Token: 0x04000E3C RID: 3644
        [Header("If dialogueLevel is above any dialogue, NPC is cleared")]
        [Header("Always do all progress at the end of the sequence (or both)")]
        public NPC.Dialogue[] dialogues;

        // Token: 0x04000E3D RID: 3645
        [Header("spawnFrom will get turned off")]
        [Header("path and npc are optional")]
        public NPC.TempRacer[] tempRacers = new NPC.TempRacer[1];

        // Token: 0x04000E3E RID: 3646
        public bool checkForPlayerDuringTempRacerSpawn;

        // Token: 0x04000E3F RID: 3647
        public Characters character = Characters.NONE;

        // Token: 0x04000E40 RID: 3648
        public AudioClipID useMiscTalkVoice = AudioClipID.NONE;

        // Token: 0x04000E41 RID: 3649
        public Crew crew;

        // Token: 0x04000E52 RID: 3666
        public float playerLookAtHeightForNonHumanNPC = 0.3f;

        // Token: 0x04000E53 RID: 3667
        public bool playSoftBounceAnimDuringTalk;

        // Token: 0x04000E55 RID: 3669
        [Tooltip("This is for NPCs that have a REP requirement set somewhere in there dialogue which is not their first dialogue.")]
        [SerializeField]
        private bool showRequiredRepForDialogueInPauseMenu;

        // Token: 0x02000452 RID: 1106
        public enum ConversationStarter
        {
            // Token: 0x040021D8 RID: 8664
            DANCE_TOPROCK,
            // Token: 0x040021D9 RID: 8665
            TALK,
            // Token: 0x040021DA RID: 8666
            TRIGGER,
            // Token: 0x040021DB RID: 8667
            RIGHT_AWAY,
            // Token: 0x040021DC RID: 8668
            REQUIRED_REP,
            // Token: 0x040021DD RID: 8669
            REQUIRED_SCORE,
            // Token: 0x040021DE RID: 8670
            EXTERNAL,
            // Token: 0x040021DF RID: 8671
            TALK_GIRL,
            // Token: 0x040021E0 RID: 8672
            NONE,
            // Token: 0x040021E1 RID: 8673
            TALK_AND_REQUIRED_REP,
            // Token: 0x040021E2 RID: 8674
            DANCE_POPPING,
            // Token: 0x040021E3 RID: 8675
            DANCE_HOUSE,
            // Token: 0x040021E4 RID: 8676
            DANCE_BREAK,
            // Token: 0x040021E5 RID: 8677
            DANCE_HIPHOP,
            // Token: 0x040021E6 RID: 8678
            DANCE_LOCKING,
            // Token: 0x040021E7 RID: 8679
            REQUIRED_PHOTOS_OF_TEMPRACER,
            // Token: 0x040021E8 RID: 8680
            TAXI_DANCE,
            // Token: 0x040021E9 RID: 8681
            REQUIRED_PHOTOS_OF_NPC,
            // Token: 0x040021EA RID: 8682
            TALK_RED
        }

        // Token: 0x02000453 RID: 1107
        [Serializable]
        public class Dialogue
        {
            // Token: 0x040021EB RID: 8683
            public int dialogueLevel;

            // Token: 0x040021EC RID: 8684
            public bool enterSequenceInstantly;

            // Token: 0x040021ED RID: 8685
            public bool skippable = true;

            // Token: 0x040021EE RID: 8686
            public bool disabledExitOnInput;

            // Token: 0x040021EF RID: 8687
            public bool lowerVolumeDuringDialogue = true;

            // Token: 0x040021F0 RID: 8688
            public bool beDynamicTempRacer;

            // Token: 0x040021F1 RID: 8689
            public bool placePlayerAtSnapPosition = true;

            // Token: 0x040021F2 RID: 8690
            public bool hidePlayer;

            // Token: 0x040021F3 RID: 8691
            public bool hideNPC;

            // Token: 0x040021F4 RID: 8692
            public bool playCharacterTalkVoice;

            // Token: 0x040021F5 RID: 8693
            public NPC.ConversationStarter conversationStarter;

            // Token: 0x040021F6 RID: 8694
            public PlayableDirector sequence;

            // Token: 0x040021F7 RID: 8695
            public UnityEvent OnStartSequence;

            // Token: 0x040021F8 RID: 8696
            public UnityEvent OnEndSequence;
        }

        // Token: 0x02000454 RID: 1108
        [Serializable]
        public class TempRacer
        {
            // Token: 0x040021FB RID: 8699
            public Crew crew = Crew.FRANKS;

            // Token: 0x040021FC RID: 8700
            public Characters character = Characters.frank;

            // Token: 0x040021FD RID: 8701
            public int outfit;

            // Token: 0x040021FE RID: 8702
            public MoveStyle moveStyle = MoveStyle.BMX;

            // Token: 0x040021FF RID: 8703
            public Transform spawnFrom;

            // Token: 0x04002200 RID: 8704
            public PlayerAIPath path;

            // Token: 0x04002201 RID: 8705
            public int pathIDNo = -1;

            // Token: 0x04002202 RID: 8706
            public NPC npcAtEnd;
        }

        public void ChangeMovestyle(int num) {
        }
    }
}
