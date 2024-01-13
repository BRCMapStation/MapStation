using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.API {
    public interface IMapStationAPI {
        public ReadOnlyCollection<ICustomStage> CustomStages { get; }
        /// <summary>
        /// Retrieves a custom stage by its ID. Returns null if not a valid ID for a custom stage.
        /// </summary>
        public ICustomStage GetCustomStageByID(int stageID);
        /// <summary>
        /// Retrieves the Stage ID for a custom map's internal name.
        /// </summary>
        public int GetStageID(string stageInternalName);
    }
}
