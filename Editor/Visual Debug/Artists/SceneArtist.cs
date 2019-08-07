using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Maumer.Internal
{
	public class SceneArtist
	{

		public string artistType;
		[SerializeField] protected Color activeDrawColour = Color.white;
		[SerializeField] protected Color inactiveDrawColour = Color.gray;
        public bool showWhenInBackground = true;

		public SceneArtist()
		{
			activeDrawColour = Color.white;
			inactiveDrawColour = Color.gray;
		}

		public void SetColour(Color activeDrawColour, Color backgroundDrawColour)
		{
			this.activeDrawColour = activeDrawColour;
			this.inactiveDrawColour = backgroundDrawColour;
		}


		public virtual void Draw(bool isActive)
		{
#if UNITY_EDITOR
			Handles.color = (isActive) ? activeDrawColour : inactiveDrawColour;
#endif
		}
	}
}