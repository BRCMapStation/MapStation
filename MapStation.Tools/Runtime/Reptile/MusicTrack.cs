using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001C2 RID: 450
    [CreateAssetMenu(menuName = "Reptile/Phone/Music/New Track")]
    public class MusicTrack : AUnlockable {
        // Token: 0x04000DCF RID: 3535
        public AudioClip AudioClip;

        // Token: 0x04000DD0 RID: 3536
        public string Title = string.Empty;

        // Token: 0x04000DD1 RID: 3537
        public string Artist = string.Empty;

        // Token: 0x04000DD2 RID: 3538
        public bool isRepeatable;
    }
}
