using System;
using UnityEngine;

namespace loaders.scenes
{
    
    public interface ISceneInputParams
    {
        Action<ISceneOutputParams> Callback { get; set; }
    }
    
    public interface ISceneOutputParams
    {
        // public Action<SceneOutputParamsAbstract> Callback;
    }
    

    
}