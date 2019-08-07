
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Maumer.Internal
{
    public class LinesArtist : SceneArtist
    {
        public Vector3[] lineSegments;

        public LinesArtist(IEnumerable<Vector3> lineSegments)
        {
            artistType = typeof(LinesArtist).ToString();
            this.lineSegments = lineSegments.ToArray();

        }

        public override void Draw(bool isActive)
        {
#if UNITY_EDITOR
			base.Draw(isActive);
            Handles.DrawLines(lineSegments);
#endif
        }
    }
}
