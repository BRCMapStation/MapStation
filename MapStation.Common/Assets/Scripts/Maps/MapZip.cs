using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace MapStation.Common {
    /// <summary>
    /// Read and write map zip files
    /// </summary>
    public class MapZip : IDisposable {

        public const string propertiesFilename = "properties.json";
        public const string sceneBundleFilename = "scene";
        public const string assetsBundleFilename = "assets";

        private string path;
        private ZipArchive zip;

        public MapZip(string path) {
            this.path = path;
        }

        /// <summary>
        /// Write map zip to disc
        /// </summary>
        public void WriteZip(string propertiesContents, string sceneBundlePath, string assetsBundlePath, bool compressed = true) {
            var compressionLevel = compressed ? CompressionLevel.Optimal : CompressionLevel.NoCompression;
            zip = ZipFile.Open(path, ZipArchiveMode.Create);
            zip.CreateEntryFromFile(sceneBundlePath, sceneBundleFilename, compressionLevel);
            zip.CreateEntryFromFile(assetsBundlePath, assetsBundleFilename, compressionLevel);
            var propertiesEntry = zip.CreateEntry(propertiesFilename, compressionLevel);
            using(var propertiesFile = propertiesEntry.Open())
            using(var propertiesWriter = new StreamWriter(propertiesFile)) {
                propertiesWriter.Write(propertiesContents);
            }
            zip.Dispose();
        }

        private void openForReading() {
            zip ??= ZipFile.Open(path, ZipArchiveMode.Read);
        }

        public string GetPropertiesText() {
            openForReading();
            using(var stream = zip.GetEntry(propertiesFilename).Open())
            using(var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        public Stream GetSceneBundleStream() {
            openForReading();
            return zip.GetEntry(sceneBundleFilename).Open();
        }

        public Stream GetAssetsBundleStream() {
            openForReading();
            return zip.GetEntry(assetsBundleFilename).Open();
        }

        public void Dispose() {
            zip?.Dispose();
        }
    }
}