using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    [CreateAssetMenu]
    public class WinterObjective : ScriptableObject {
        [Header("Whether this is the first objective you get when starting a clean save.")]
        public bool StartingObjective = false;
        public bool HasSpecificStage => !string.IsNullOrEmpty(onlyInStage);
        public string Stage => onlyInStage;
        [Header("Text displayed in pause menu.")]
        public string Description;
        [Header("Text in the pause menu will only be shown when you've already beaten the main story.")]
        public bool OnlyInPostGame = false;
        [Header("Text in the pause menu will only be shown when you're in this specific stage.")]
        [SerializeField]
        private string onlyInStage;
    }
}
