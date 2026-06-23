using UnityEngine;
using UnityEngine.SceneManagement;

// scenemanager from unityengine.scenemanagement is unity's api for swapping between scenes
// LoadScene(string scenename) looks a scene with that exact name in the build settings and switches to it
// application.quit() doesn't do anything does nothing in unity editor, so the #if block swaps the code depending-
// -on where its running, so it works in both testing and in a full release
public class TitleScreenManager : MonoBehaviour
{
    public string gamesceneName = "Main game"; 
    
    // Connected to the play button's onlclick event 
    public void PlayGame()
    {
        SceneManager.LoadScene(gamesceneName);
    }

    // Connected to the exit buttons onclock event 
    public void QuitGame()
    {
        Debug.Log("Quiting game"); // Confirms that the buttin actually worked since application.quit() does nothing in the editor 

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // stops play mode when testing inside the Unity editor
        #else
            Application.Quit(); // actually closes the app once its a real build 
        #endif
    }

}
