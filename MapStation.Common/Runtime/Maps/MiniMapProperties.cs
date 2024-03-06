using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapStation.Common
{
    public class MiniMapProperties : MonoBehaviour
    {
        [Header("Material that will change colors in phone and pause menu.")]
        public Material MapMaterial;
        [Header("Sort meshes from top to bottom when rendering the MiniMap.")]
        public bool SortRenderers = false;
    }
}
