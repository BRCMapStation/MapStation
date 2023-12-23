using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winterland.Common.Challenge;

namespace Winterland.Common {
    /// <summary>
    /// Clientside progress.
    /// </summary>
    public interface ILocalProgress {
        public WinterObjective Objective { get; set; }
        public int Gifts { get; set; }
        public int FauxJuggleHighScore { get; set; }
        public int ToyLinesCollected { get; }
        public bool ArcadeUnlocked { get; set; }
        public void InitializeNew();
        public void Save();
        public void Load();
        public void SetNPCDirty(CustomNPC npc);
        public SerializedNPC GetNPCProgress(CustomNPC npc);
        public void SetToyLineCollected(Guid guid, bool collected);
        public bool IsToyLineCollected(Guid guid);
        public void SetChallengeBestTime(ChallengeLevel challenge, float bestTime);
        public float GetChallengeBestTime(ChallengeLevel challenge);
    }
}
