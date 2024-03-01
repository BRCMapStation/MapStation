using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

namespace MapStation.Tools
{
    public abstract class AMapStationPlugin
    {
        public virtual string[] GetDependencies() {
            return new string[0];
        }

        public virtual void ProcessThunderstoreZip(ZipArchive archive, string mapName) {

        }

        public virtual void ProcessMapZip(ZipArchive archive, string mapName, System.IO.Compression.CompressionLevel compressionLevel) {

        }
    }
}
