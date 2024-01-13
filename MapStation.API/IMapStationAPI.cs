using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.API {
    public interface IMapStationAPI {
        public ReadOnlyCollection<ICustomStage> CustomStages { get; }
    }
}
