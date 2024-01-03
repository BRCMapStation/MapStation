using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Reptile {
    // Token: 0x020000A6 RID: 166
    [Serializable]
    public struct SfxClip {
        // Token: 0x06000770 RID: 1904 RVA: 0x00019F28 File Offset: 0x00018128
        public static implicit operator SfxClip(SfxCollectionID sfxCollectionID) {
            SfxClip sfxClip;
            sfxClip.collectionId = sfxCollectionID;
            sfxClip.audioClipId = AudioClipID.NONE;
            return sfxClip;
        }

        // Token: 0x0400049E RID: 1182
        [SerializeField]
        public SfxCollectionID collectionId;

        // Token: 0x0400049F RID: 1183
        [SerializeField]
        public AudioClipID audioClipId;
    }
}
