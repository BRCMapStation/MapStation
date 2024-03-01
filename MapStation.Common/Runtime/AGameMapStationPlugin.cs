using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Common {
    public abstract class AGameMapStationPlugin {
        public virtual void OnAddMapToDatabase(ZipArchive archive, string path, string mapName) {
        }
    }
}
