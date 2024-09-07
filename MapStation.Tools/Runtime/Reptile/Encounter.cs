using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Reptile {
    // Token: 0x02000160 RID: 352
    public class Encounter : GameplayEvent {
        // Token: 0x04000749 RID: 1865
        public bool allowPhone = true;

        // Token: 0x0400074A RID: 1866
        public bool stopWantedOnStart = true;

        // Token: 0x0400074B RID: 1867
        public GameObject[] makeUnavailableDuringEncounter;

        // Token: 0x0400074C RID: 1868
        public bool restartImmediatelyOnFail;

        // Token: 0x0400074D RID: 1869
        public bool instantIntro;

        // Token: 0x0400074E RID: 1870
        public PlayableDirector introSequence;

        // Token: 0x0400074F RID: 1871
        public PlayableDirector outroSuccesSequence;

        // Token: 0x04000750 RID: 1872
        [Header("Has to be different than the succes outro! Will break otherwise")]
        public PlayableDirector outroFailSequence;

        // Token: 0x04000751 RID: 1873
        public bool turnOffPlayerDuringSequences;

        // Token: 0x04000752 RID: 1874
        public bool lowerVolumeDuringSequence;

        // Token: 0x04000753 RID: 1875
        public Transform startSpawner;

        // Token: 0x04000754 RID: 1876
        public Transform completeSpawner;

        // Token: 0x04000755 RID: 1877
        public Transform failSpawner;

        // Token: 0x04000756 RID: 1878
        public UnityEvent OnCompleted;

        // Token: 0x04000757 RID: 1879
        public UnityEvent OnFailed;

        // Token: 0x04000758 RID: 1880
        [Header("the moment the intro starts")]
        public UnityEvent OnIntro;

        // Token: 0x04000759 RID: 1881
        [Header("when the intro is over, the moment main event is started")]
        public UnityEvent OnStart;

        // Token: 0x0400075A RID: 1882
        [Header("the moment the outro starts, both win and lose")]
        public UnityEvent OnOutro;

        // Token: 0x0400075B RID: 1883
        public Material uniqueSkybox;

        // Token: 0x0400075E RID: 1886
        public bool mustDodgeStreetlife;

        // Token: 0x0400075F RID: 1887
        public bool doNotRestartStageMusicAtEnd;

        // Token: 0x04000760 RID: 1888
        public Teleport deathZoneTeleporter;

        // Token: 0x04000761 RID: 1889
        [SerializeField]
        public Encounter.EncounterCheckpoint[] checkpoints;

        // Token: 0x04000763 RID: 1891
        public GameObject stuffToTurnOnDuringEncounter;

        // Token: 0x04000764 RID: 1892
        public bool assumeActivatingEncounterFromReadDataMeansWeFailed;

        // Token: 0x0200040C RID: 1036
        public enum EncounterState {
            // Token: 0x04002049 RID: 8265
            CLOSED,
            // Token: 0x0400204A RID: 8266
            OPEN_STARTUP,
            // Token: 0x0400204B RID: 8267
            OPEN,
            // Token: 0x0400204C RID: 8268
            INTRO,
            // Token: 0x0400204D RID: 8269
            MAIN_EVENT,
            // Token: 0x0400204E RID: 8270
            MAIN_EVENT_SUCCES_DECAY,
            // Token: 0x0400204F RID: 8271
            MAIN_EVENT_FAILED_DECAY,
            // Token: 0x04002050 RID: 8272
            OUTRO_SUCCES,
            // Token: 0x04002051 RID: 8273
            OUTRO_FAIL
        }

        // Token: 0x0200040D RID: 1037
        [Serializable]
        public class EncounterCheckpoint {
            // Token: 0x04002052 RID: 8274
            public Collider trigger;

            // Token: 0x04002053 RID: 8275
            public Transform spawnLocation;
        }

        // Token: 0x0200040E RID: 1038
        [Serializable]
        public class Racer {
            // Token: 0x04002054 RID: 8276
            public Characters character;

            // Token: 0x04002055 RID: 8277
            public Crew crew;

            // Token: 0x04002056 RID: 8278
            public int outfit;

            // Token: 0x04002057 RID: 8279
            public MoveStyle moveStyle;

            // Token: 0x04002058 RID: 8280
            public Transform spawner;

            // Token: 0x04002059 RID: 8281
            public PlayerAIPath path;

            // Token: 0x0400205A RID: 8282
            public int pathID = -1;
        }
    }
}
