using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField]
    private string PathName;

    void Start()
    {
        PlayerVideo();
    }

    public void PlayerVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        
        if(videoPlayer != null)
        {
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, PathName);
            Debug.Log(path);
            videoPlayer.url = path;
            videoPlayer.Play();
        }
    }
}
