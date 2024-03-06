using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapStation.Common
{
    public class MiniMapProperties : MonoBehaviour
    {
        [Header("Material that will change colors in phone and pause menu.")]
        public Material MapMaterial;
        [Header("Automatically sort renderers in MiniMap so that higher objects always display on top.")]
        public bool SortRenderers = true;
    }
}
