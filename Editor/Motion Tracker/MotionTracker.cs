using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maumer
{

    public static class MotionTracker
    {

        public static Transform target;
        [Header("Temporary Trail")]
        public static int MaxRecordFrames = 80;
        [Header("Lifetime Trail")]
        public static float RecordingInterval;
        public static AnimationCurve PostionCurve;
        public static float totalRecordTime { get; private set; }
        public static bool IsRecording { get; private set; }
        private static float lastRecordTime;
        public static Mode mode;

        public static List<Vector3> positions { get; private set; }
        public static List<Vector3> deltaPositions { get; private set; }
        /// <summary>
        /// the speed of the target that's being tracked
        /// </summary>
        /// <value></value>
        public static float targetSpeed { get; private set; }
        public static Vector3 targetDeltaPosition { get; private set; }
        private static Vector3 prevPosition;


        public static void Update()
        {
            if (IsRecording)
            {
                positions.Insert(0, target.position);
                if (positions.Count > MaxRecordFrames)
                {
                    positions.RemoveAt(positions.Count - 1);
                }

                totalRecordTime += Time.deltaTime;
                if (totalRecordTime > lastRecordTime + RecordingInterval)
                {
                    record();
                    lastRecordTime = totalRecordTime;
                }
            }

        }

        private static void updatePositionData()
        {
            targetDeltaPosition = target.position - prevPosition;
            targetSpeed = targetDeltaPosition.magnitude;
            prevPosition = target.position;

        }

        public static void RecordPostion()
        {
            // prevCurlPositions.Add(TrackedObject.position);
            for (int i = 0; i < positions.Count; i++)
            {
                float value = PostionCurve.Evaluate((float)i / positions.Count);
            }
        }

        public static void Clear()
        {
            positions.Clear();

            totalRecordTime = 0;
            lastRecordTime = 0;
        }


        /// <summary>
        /// Start recording
        /// </summary>
        public static void Start()
        {
            if (IsRecording)
                Clear();

            if (positions == null)
                positions = new List<Vector3>();
            else
                positions.Clear();
            if (deltaPositions == null)
                deltaPositions = new List<Vector3>();
            else
                deltaPositions.Clear();

            IsRecording = true;
        }

        /// <summary>
        /// Stop recording
        /// </summary>
        public static void Stop()
        {
            // you can smooth the curve by using SmoothAllTangents
            // path.SmoothAllTangents(0);
            IsRecording = false;
        }

        public static Vector3 GetTrailPositionAtFrame(int numFrames)
        {
            if (numFrames >= positions.Count)
            {
                return target.position;
            }
            return positions[numFrames];
        }

        private static void record()
        {
            if (target)
            {
                // DO something
            }
        }

        public enum Mode
        {
            Automatic,
            Manual
        }

    }

}