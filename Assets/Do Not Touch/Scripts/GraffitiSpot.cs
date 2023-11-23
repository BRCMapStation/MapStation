using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using ch.sycoforge.Decal;

namespace Reptile
{
	public class GraffitiSpot : AProgressable, IInitializableSceneObject
	{
		public enum AttachType
		{
			DEFAULT = 0,
			COPS = 1,
			DREAMGOAL = 2
		}

		public Story.ObjectiveID beTargetForObjective = Story.ObjectiveID.NONE;

		public bool isOpen = true;

		[AlignWithRay(8396801)]
		public EasyDecal topGraffiti;

		public EasyDecal bottomGraffiti;

		public Material emptyGraffitiMaterial;

		public Crew topCrew;

		public Crew bottomCrew;

		public AttachType attachedTo;

		public bool reportAsCrime = true;

		public GameObject marker;

		public Transform extraSlotsContainer;

		public PlayerType notAllowedToPaint = PlayerType.NONE;

		public GraffitiSize size;

		public UnityEvent OnFinish;

		public bool isMarkerActive;

		public bool deactivateDuringEncounters;

		public GameObject dynamicRepPickup;

		public Renderer[] decalTopPlaneAlternativeRenderer;

		public Renderer[] decalBottomPlaneAlternativeRenderer;
	}
}
