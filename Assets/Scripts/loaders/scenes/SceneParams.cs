using System;
using UnityEngine;

namespace loaders.scenes
{
    [Serializable]
    public class SceneInputParams : ISceneInputParams
    {
        public Action<ISceneOutputParams> Callback { get; set; }
    }
    
    public class SceneOutputParams : ISceneOutputParams
    {
        // public Action<SceneOutputParamsAbstract> Callback;
    }
    
}