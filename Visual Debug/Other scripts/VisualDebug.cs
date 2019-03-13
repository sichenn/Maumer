using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using Maumer.Internal;

namespace Maumer
{
    public static partial class VisualDebug
    {
        public enum Tag
        {
            None,
            Agent
        }
        const string runningInUnityEditor = "UNITY_EDITOR";
        public static List<DebugData> debugDatas { get { return debugDataDict.Values.ToList(); } }
        public static Dictionary<Object, DebugData> debugDataDict { get { return m_debugDataDict; } }
        static Dictionary<Object, DebugData> m_debugDataDict = new Dictionary<Object, DebugData>();
        private static Object m_currentSender;
        private static Color m_activeDrawColor = Color.white;
        private static Color m_inactiveDrawColor = Color.grey;
        // static DebugData debugData = new DebugData();

        /// <summary>
        /// Save visual debug data. Call this when finished creating frames.
        /// </summary>
        [Conditional(runningInUnityEditor)]
        public static void Save()
        {
            VisualDebugSaveLoad.Save(m_debugDataDict.Values.ToArray());
        }

        /// <summary>
        /// Clear any previously loaded data. This should be done before calling BeginFrame() for the first time.
        /// This is identical to calling Clear().
        /// </summary>
        [Conditional(runningInUnityEditor)]
        public static void BeginRecord(Object sender)
        {
            DebugData targetDebugData;
            m_currentSender = sender;
            if (m_debugDataDict.TryGetValue(sender, out targetDebugData))
            {
                targetDebugData.frames.Clear();
            }

            // string loggerClass = methodBased.DeclaringType.ToString();

            // if (m_debugDataDict[loggerClass] != null)
            // {
            //     m_debugDataDict[methodBased.DeclaringType.ToString()] = new DebugData();
            // }
            // else
            // {
            //     m_debugDataDict.Add(loggerClass, new DebugData());

            // }
            // Clear();
        }

        /// <summary>
        /// Clear any previously loaded data. This should be done before calling BeginFrame() for the first time.
        /// This is identical to calling Initialize().
        /// </summary>
        [Conditional(runningInUnityEditor)]
        [System.Obsolete("Not necessary. Data will be refreshed when BeginRecord is called")]
        public static void EndRecord()
        {
            // debugData = new DebugData();
            // DebugData targetDebugData;
            // if (m_debugDataDict.TryGetValue(m_currentSender, out targetDebugData))
            // {
            //     targetDebugData.frames.Clear();
            // }
        }

        /// <summary>
        /// Begin a new frame.
        /// </summary>
        /// <param name="description">Description.</param>
        /// <param name="keepInBackground">If true, this frame will remain in the background when the next frames are drawn, i.e. it won't be erased.</param>
        [Conditional(runningInUnityEditor)]
        public static void BeginFrame(string description, Tag tag = Tag.None, bool keepInBackground = false)
        {
            // m_currentSender = sender;
            DebugData targetDebugData;
            if (!m_debugDataDict.TryGetValue(m_currentSender, out targetDebugData))
            {
                targetDebugData = new DebugData();
                m_debugDataDict.Add(m_currentSender, targetDebugData);
            }
            targetDebugData.logger = m_currentSender;
            targetDebugData.dontShowNextElementWhenFrameIsInBackground = false;
            targetDebugData.frames.Add(new Frame(description, keepInBackground, targetDebugData.frames.Count));
        }

        /// <summary>
        /// The next element drawn to the current frame will not be shown when the frame is in the background.
        /// (Only applicable to frames where keepInBackground is true).
        /// </summary>
        [Conditional(runningInUnityEditor)]
        public static void DontShowNextElementWhenFrameIsInBackground()
        {
            DebugData targetDebugData;
            if (m_debugDataDict.TryGetValue(m_currentSender, out targetDebugData))
            {
                targetDebugData.dontShowNextElementWhenFrameIsInBackground = true;
            }
        }

        /// <summary>
        /// Set the active and inactive colours.
        /// </summary>
        /// <param name="activeColour">Colour to use when frame is active.</param>
        /// <param name="inactiveColor">Colour to use when frame is in background (only applies to frames where keepInBackground is true).</param>
		[Conditional(runningInUnityEditor)]
        public static void SetColour(Color activeColour, Color inactiveColor)
        {
            m_activeDrawColor = activeColour;
            m_inactiveDrawColor = inactiveColor;
            // DebugData targetDebugData;
            // if (m_debugDataDict.TryGetValue(m_currentSender, out targetDebugData))
            // {
            //     var lastestFrame = targetDebugData.frames[targetDebugData.frames.Count - 1];
            //     lastestFrame.artists[lastestFrame.artists.Count - 1].SetColour(activeColour, inactiveColor);
            // }

        }


        /// <summary>
        /// Set the active and inactive colours.
        /// </summary>
        /// <param name="activeHexColour">Colour to use when frame is active.</param>
        /// <param name="inactiveHexColour">Colour to use when frame is in background (only applies to frames where keepInBackground is true).</param>
        [Conditional(runningInUnityEditor)]
        public static void SetColour(string activeHexColour, string inactiveHexColour)
        {
            SetColour(Colours.HexStringToColour(activeHexColour), Colours.HexStringToColour(inactiveHexColour));
        }

        [Conditional(runningInUnityEditor)]
        public static void SetColour(Color colour)
        {
            SetColour(colour, colour);
        }

        [Conditional(runningInUnityEditor)]
        public static void SetColour(string hexColour)
        {
            SetColour(Colours.HexStringToColour(hexColour), Colours.HexStringToColour(hexColour));
        }

        // [Conditional(runningInUnityEditor)]
        // public static void SetDefaultFontSize(int fontSize)
        // {
        //     StackTrace stackTrace = new StackTrace();
        //     MethodBase methodBased = stackTrace.GetFrame(1).GetMethod();
        //     string loggerClass = methodBased.DeclaringType.ToString();
        //     DebugData targetDebugData;
        //     if (m_debugDataDict.TryGetValue(loggerClass, out targetDebugData))
        //     {
        //         targetDebugData.currentFontSize = fontSize;
        //     }

        // }

        // TODO: MakeGLOBAL
        // [Conditional(runningInUnityEditor)]
        // public static void ResetDefaultFontSize()
        // {
        //     StackTrace stackTrace = new StackTrace();
        //     MethodBase methodBased = stackTrace.GetFrame(1).GetMethod();
        //     string loggerClass = methodBased.DeclaringType.ToString();
        //     DebugData targetDebugData;
        //     if (m_debugDataDict.TryGetValue(loggerClass, out targetDebugData))
        //     {
        //         targetDebugData.currentFontSize = DebugData.defaultFontSize;
        //     }
        // }

        static void AddArtistToCurrentFrame(SceneArtist artist)
        {
            DebugData targetDebugData;
            if (m_debugDataDict.TryGetValue(m_currentSender, out targetDebugData))
            {
                if (targetDebugData.frames.Count == 0)
                {
                    BeginFrame("");
                }

                artist.SetColour(m_activeDrawColor, m_inactiveDrawColor);
                artist.showWhenInBackground = !targetDebugData.dontShowNextElementWhenFrameIsInBackground;
                targetDebugData.frames[targetDebugData.frames.Count - 1].AddArtist(artist);
            }
        }

        static IEnumerable<Vector3> EnumerableVector2ToVector3(IEnumerable<Vector2> v2)
        {
            return v2.Select(v => new Vector3(v.x, v.y, 0));
        }
    }
}