using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.API {
    public interface ICustomStage {
        public string DisplayName { get; }
        public string InternalName { get; }
        public string AuthorName { get; }
        public int StageID { get; }
    }
}
