﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maumer.Internal
{
    [System.Obsolete("use the editor window version instead")]
    public class DebugViewer : MonoBehaviour
    {
        // public List<Frame> frames;
        // public int frameIndex = -1;
        // public float timeBetweenFrames = .5f;
        // public bool loop;

        // public void Load()
        // {
		// 	if (VisualDebugSaveLoad.HasSaveFile())
		// 	{
        //         frames = new List<Frame>(VisualDebugSaveLoad.Load());
		// 	}

        //     if (frames != null)
        //     {
        //         if (frameIndex >= frames.Count)
        //         {
        //             frameIndex = -1;
        //         }
        //     }
        // }

        // public Frame CurrentFrame
        // {
        //     get
        //     {
        //         if (frameIndex >= 0 && frameIndex < frames.Count)
        //         {
        //             return frames[frameIndex];
        //         }
        //         return null;
        //     }

        // }

        // public int NumFrames
        // {
        //     get
        //     {
        //         if (frames == null)
        //         {
        //             return 0;
        //         }
        //         return frames.Count;

        //     }
        // }
    }
}