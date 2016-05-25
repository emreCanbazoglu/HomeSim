using UnityEngine;
using System.Collections;

public class ActionFileBrowser : MonoBehaviour 
{
    string message = "";
    float alpha = 1.0f;
    char pathChar = '/';

    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            pathChar = '\\';
        }

        UniFileBrowser.use.allowMultiSelect = false;
        UniFileBrowser.use.OpenFileWindow(OpenFile);
    }


    void OpenFile(string pathToFile)
    {
        ActionReader.Instance.SetFilePath(pathToFile);

        Fade();
    }

    void Fade()
    {
        StopCoroutine("FadeAlpha");	// Interrupt FadeAlpha if it's already running, so only one instance at a time can run
        StartCoroutine("FadeAlpha");
    }

    IEnumerator FadeAlpha()
    {
        alpha = 1.0f;
        yield return new WaitForSeconds(5.0f);
        for (alpha = 1.0f; alpha > 0.0f; alpha -= Time.deltaTime / 4)
        {
            yield return null;
        }
        message = "";
    }
}
