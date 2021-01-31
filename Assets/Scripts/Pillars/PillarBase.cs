using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe3D.Pillars
{
    public abstract class PillarBase : MonoBehaviour
    {
        internal List<PillarSegment> PillarSegments { get; set; }
        protected internal int PillarIndex { get; set; }
        protected internal int PillarRowIndex { get; set; }
        protected internal int PillarColumnIndex { get; set; }
        protected abstract void SetupPillar();

        protected virtual void Awake() =>
            SetupPillar();
    }
}