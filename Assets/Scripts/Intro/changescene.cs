
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class changescene : MonoBehaviour
{

    VideoPlayer video;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();

        video.loopPointReached += CheckOver;


    }


    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("DesktopTest");
    }
}