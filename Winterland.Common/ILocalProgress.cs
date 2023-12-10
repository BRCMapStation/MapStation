using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    /// <summary>
    /// Clientside progress.
    /// </summary>
    public interface ILocalProgress {
        public WinterObjective Objective { get; set; }
        public void Save();
        public void Load();
        public void SetNPCDirty(CustomNPC npc);
        public SerializedNPC GetNPCProgress(CustomNPC npc);
    }
}
