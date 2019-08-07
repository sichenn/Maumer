using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Maumer.Internal
{
    public class WireCubeArtist : SceneArtist
    {
        public Vector3 center;
        public Vector3 size;

        public WireCubeArtist(Vector3 center, Vector3 size)
        {
			artistType = typeof(WireCubeArtist).ToString();
			this.center = center;
			this.size = size;
        }

        public override void Draw(bool isActive)
        {
#if UNITY_EDITOR
            base.Draw(isActive);
			Handles.DrawWireCube(center, size);
#endif
        }
    }
}
