using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    /// <summary>
    /// Serverside progress.
    /// </summary>
    public interface IGlobalProgress {
        float TreeConstructionPercentage {get;}
        delegate void OnTreeConstructionPercentageChangedHandler();
        event OnTreeConstructionPercentageChangedHandler OnTreeConstructionPercentageChanged;
    }

    public class WritableGlobalProgress : IGlobalProgress {
        public float TreeConstructionPercentage {get; private set;}

        public event IGlobalProgress.OnTreeConstructionPercentageChangedHandler OnTreeConstructionPercentageChanged;

        public void SetTreeConstructionPercentage(float value) {
            TreeConstructionPercentage = value;
            OnTreeConstructionPercentageChanged?.Invoke();
        }
    }
}
