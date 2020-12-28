using System;
using UnityEngine;

namespace loaders.scenes
{
    public class ChapterSelectLoader : SceneBehaviour
    {
        public ChapterSelectInputParams inputParamsForInspector;
        
        public static void LoadScene(ChapterSelectInputParams sceneInputParams, Action<ISceneOutputParams> callback)
        {
            LoadMyScene("ChapterSelection", sceneInputParams, callback);
        }

        public void EndScene(ChapterSelectOutParams outputParams)
        {
            print("something is done also");
            base.EndScene(outputParams);
        }

        private void Start()
        {
            inputParamsForInspector = sceneInputParams as ChapterSelectInputParams;
            print($"let me see: {(sceneInputParams as ChapterSelectInputParams)?.param1}");
        }
    }
    
    [Serializable]
    public class ChapterSelectInputParams : SceneInputParams
    {
        public string param1;
    }
    
    [Serializable]
    public class ChapterSelectOutParams : SceneOutputParams
    {
        public string outParam1;
    }
    
    
}