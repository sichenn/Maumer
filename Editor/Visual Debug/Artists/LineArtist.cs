using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Maumer.Internal
{
    public class LineArtist : SceneArtist
    {
        [SerializeField]
        private Vector3 from;
        [SerializeField]
        private Vector3 to;

        public LineArtist(Vector3 from, Vector3 to): base()
        {
            artistType = typeof(LineArtist).ToString();
            this.from = from;
            this.to = to;
        }

        public override void Draw(bool isActive)
        {
#if UNITY_EDITOR
            base.Draw(isActive);
            Handles.DrawLine(this.from, this.to);
#endif
        }
    }

}
