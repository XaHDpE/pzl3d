using System;
using UnityEngine;

namespace loaders.scenes
{
    public abstract class SceneBehaviour: MonoBehaviour {
        
        private static ISceneInputParams loadSceneInputRegister;
        public ISceneInputParams sceneInputParams;

        protected static void LoadMyScene(string sceneName, ISceneInputParams sceneInputParams, Action<ISceneOutputParams> callback) {
            loadSceneInputRegister = sceneInputParams;
            sceneInputParams.Callback = callback;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void Awake() {
            if (loadSceneInputRegister != null) sceneInputParams = loadSceneInputRegister;
            loadSceneInputRegister = null; // the register has served its purpose, clear the state
        }

        protected void EndScene(ISceneOutputParams outputParams) {
            sceneInputParams.Callback?.Invoke(outputParams);
            sceneInputParams.Callback = null;
        }

    }
    
}