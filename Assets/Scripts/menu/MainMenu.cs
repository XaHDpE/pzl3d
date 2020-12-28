using loaders.scenes;
using UnityEngine;

namespace menu
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            ChapterSelectLoader.LoadScene(new ChapterSelectInputParams()
            {
                param1 = "test2"
            }, (outputParams) =>
            {
                if (outputParams is ChapterSelectOutParams outParams) print($"test done, params: {outParams.outParam1}");
            });
        }

        public void QuitGame()
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    
}
