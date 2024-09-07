using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Reptile {
    public class GameplayEvent : AProgressable, IInitializableSceneObject {
        // Token: 0x04000CB8 RID: 3256
        [Header("Use AllowOpen() OR set to open itself here OR use a TEMP_RACER")]
        public bool canOpen;

        // Token: 0x04000CB9 RID: 3257
        [Header("(even if canOpen is true, it won't be active until openOn is valid)")]
        [Header("This is what you need to do to open it")]
        public GameplayEvent.OpenOn openOn;

        // Token: 0x04000CBA RID: 3258
        public int requirement;

        // Token: 0x04000CBB RID: 3259
        public Story.ObjectiveID beTargetForObjective = Story.ObjectiveID.NONE;

        // Token: 0x04000CBC RID: 3260
        public int objectiveTargetOnlyFromDialogueLevel;

        // Token: 0x04000CBD RID: 3261
        public Story.ObjectiveID specifiedStoryObjectiveToAdvanceTo = Story.ObjectiveID.NONE;

        // Token: 0x04000CBF RID: 3263
        public PlayableDirector openingSequence;

        // Token: 0x04000CC0 RID: 3264
        public float openStartupDuration;

        // Token: 0x04000CC1 RID: 3265
        public Encounter unavailableForEveryEncounterExceptThis;

        // Token: 0x04000CC2 RID: 3266
        public bool standDownEnemiesOnStartSequence = true;

        // Token: 0x04000CC6 RID: 3270
        public MusicTrack customMusicTrack;

        // Token: 0x02000438 RID: 1080
        public enum OpenOn {
            // Token: 0x04002165 RID: 8549
            JUST_OPEN,
            // Token: 0x04002166 RID: 8550
            REQUIRED_REP,
            // Token: 0x04002167 RID: 8551
            REQUIRED_SCORE,
            // Token: 0x04002168 RID: 8552
            TEMP_RACER,
            // Token: 0x04002169 RID: 8553
            OBJECTIVE,
            // Token: 0x0400216A RID: 8554
            REQUIRED_COUNTER,
            // Token: 0x0400216B RID: 8555
            CHARACTER_IS_UNLOCKED
        }
    }
}
