using UnityEngine;
using System.IO;
using System.Linq;

namespace Maumer.Internal
{

    /*
     * Handles saving/loading of Frame arrays.
     */

    public static class VisualDebugSaveLoad
    {

        public static bool HasNewSaveWaiting { get; private set; }

        public static void Save(DebugData[] debugDataArray)
        {
            HasNewSaveWaiting = true;

            SaveData saveData = new SaveData(debugDataArray);
            string saveString = JsonUtility.ToJson(saveData);

            StreamWriter writer = new StreamWriter(SavePath, false);
            writer.Write(saveString);
            writer.Close();
        }

        public static bool HasSaveFile()
        {
            return File.Exists(SavePath);
        }

        public static DebugData[] Load()
        {
            HasNewSaveWaiting = false;

            StreamReader reader = new StreamReader(SavePath);
            string saveString = reader.ReadToEnd();
            reader.Close();

            SaveData saveData = JsonUtility.FromJson<SaveData>(saveString);
            saveData.ProcessLoadedData();

            return saveData.debugSaveDataArray.Select(d => d.debugData).ToArray();

        }

        static string SavePath
        {
            get
            {
                string folderPath = Application.persistentDataPath + "/VisualDebugSaveData";
                Directory.CreateDirectory(folderPath); // create folder (if it doesn't already exist)

                string fileName = "VisualDebugData.txt";
                return folderPath + "/" + fileName;
            }
        }

        [System.Serializable]
        public class DebugDataSaveData
        {
            public DebugData debugData;
            public string[] artistJsonStrings;

            public DebugDataSaveData(DebugData data)
            {
                this.debugData = data;
                foreach (var frame in data.frames)
                {
                    if (frame.artists != null)
                    {
                        artistJsonStrings = new string[frame.artists.Count];
                        for (int i = 0; i < frame.artists.Count; i++)
                        {
                            artistJsonStrings[i] = JsonUtility.ToJson(frame.artists[i]);
                        }
                    }
                }

            }

        }


        [System.Serializable]
        public class SaveData
        {
            public DebugDataSaveData[] debugSaveDataArray;

            public SaveData(DebugData[] debugDataArray)
            {
                debugSaveDataArray = debugDataArray.Select(d => new DebugDataSaveData(d)).ToArray();
            }

            public void ProcessLoadedData()
            {
                // foreach (DebugDataSaveData saveData in debugSaveDataArray)
                // {
                //     foreach (string artistSaveString in saveData.artistJsonStrings)
                //     {
                //         SceneArtist baseArtist = JsonUtility.FromJson<SceneArtist>(artistSaveString);

                //         if (!string.IsNullOrEmpty(baseArtist.artistType) && System.Type.GetType(baseArtist.artistType) != null)
                //         {
                //             SceneArtist artist = JsonUtility.FromJson(artistSaveString, System.Type.GetType(baseArtist.artistType)) as SceneArtist;
                //             f.debugData frame.AddArtist(artist);
                //         }
                //     }
                // }


            }
        }
    }
}