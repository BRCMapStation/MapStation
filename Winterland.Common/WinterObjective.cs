#pragma warning disable CS0649
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    [CreateAssetMenu]
    public class WinterObjective : ScriptableObject {
        [Header("Whether this is the first objective you get when starting a clean save.")]
        public bool StartingObjective = false;
        public bool HasSpecificStage => !string.IsNullOrEmpty(onlyInStage);
        public string Stage => onlyInStage;
        [Header("Text displayed in pause menu. If this is empty the main story objective will be kept instead.")]
        public string Description;
        [Header("Objective will only display in post game.")]
        public bool OnlyInPostGame = true;
        [Header("If this is not empty, the objective will only display on this stage (e.g. hideout, osaka, square)")]
        [SerializeField]
        private string onlyInStage;

        /// <summary>
        /// Checks whether this objective is valid in the current conditions.
        /// </summary>
        public bool Test() {
            var beatTheGame = Core.Instance.SaveManager.CurrentSaveSlot.CurrentStoryObjective == Story.ObjectiveID.HangOut;
            if (!beatTheGame && OnlyInPostGame)
                return false;
            var currentStage = Core.Instance.BaseModule.CurrentStage.ToString();
            if (HasSpecificStage && Stage != currentStage)
                return false;
            return true;
        }
    }
}
