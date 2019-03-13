using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maumer.Internal
{
    [System.Serializable]
    public class DebugData
    {
        public const int defaultFontSize = 18;
        public Object logger;
        public string loggerName { get { return logger.ToString(); } }
        public VisualDebug.Tag tag;
        [SerializeField]
        public List<Frame> frames;
        public bool dontShowNextElementWhenFrameIsInBackground;
    
        public DebugData()
        {
            frames = new List<Frame>();
            // currentActiveColour = Color.white;
            // currentBackgroundColour = currentActiveColour;
            // currentFontSize = defaultFontSize;
        }
    }
}