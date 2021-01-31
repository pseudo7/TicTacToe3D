using System.Collections.Generic;
using TicTacToe3D.Shapes;
using UnityEngine;

namespace TicTacToe3D.Pillars
{
    public sealed class PillarSegment : MonoBehaviour
    {
        internal PillarBase CurrentPillar { get; private set; }
        internal int SegmentIndex { get; private set; }

        internal ShapeBase CurrentShape { get; set; }

        private void Awake()
        {
            SegmentIndex = transform.GetSiblingIndex();
            CurrentPillar = GetComponentInParent<PillarBase>();
            (CurrentPillar.PillarSegments ??= new List<PillarSegment> {null, null, null})
                [SegmentIndex] = this;
        }

        internal void SetSegment()
        {
            MoveToThisSegment();
        }

        private void MoveToThisSegment()
        {
        }
    }
}