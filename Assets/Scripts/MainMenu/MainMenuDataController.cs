using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class MainMenuDataController : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public List<string> songNames;
    public List<Texture2D> backgroundImages;

    public GameObject selectionMenu;
    public GameObject mask;

    public PlayerSetting playerSetting;

    public SongData data;


    private void Awake()
    {
        StartCoroutine(LoadAsset());

        selectionMenu.SetActive(false);
    }

    public SongData SongDataLoadedFromJson()
    {
        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "SongDatas");

        path = Path.Combine(path, GameInfo.songName);

        path = Path.Combine(path, "SongData" + ".txt");

        if (!File.Exists(path))
        {
            return new SongData();
        }

        string loadData;

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<SongData>(loadData);

    }

    public PlayerSetting PlayerSettingLoadedFromJson()
    {
        // Get data from path : Application.dataPath/SongDatas/[songName]/NoteData.txt
        string path = Path.Combine(Application.dataPath, "Player.txt");

        if (!File.Exists(path))
        {
            PlayerSetting data = new PlayerSetting();

            // Data To Json String
            string jsonInfo = JsonUtility.ToJson(data, true);

            // Json String Save in text file
            File.WriteAllText(path, jsonInfo);

            return data;
        }

        string loadData;

        loadData = File.ReadAllText(path);

        //把字串轉換成Data物件
        return JsonUtility.FromJson<PlayerSetting>(loadData);

    }

    private IEnumerator LoadAsset()
    {
        string path = Path.Combine(Application.dataPath, "SongDatas");

        DirectoryInfo info = new DirectoryInfo(path);

        DirectoryInfo[] folders = info.GetDirectories();

        foreach(DirectoryInfo folder in folders)
        {
            AudioClip myClip = null;

            path = Path.Combine(folder.FullName, "music.mp3");

#if UNITY_STANDALONE_OSX

        string url = "file://" + path;

#endif

#if UNITY_STANDALONE_LINUX

        string url = "file://" + path;

#endif

#if UNITY_STANDALONE_WIN

            string url = "file:///" + path;

#endif

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    myClip = DownloadHandlerAudioClip.GetContent(www);
                    myClip.name = "music";
                    audioClips.Add(myClip);
                }
            }

            path = Path.Combine(folder.FullName, "bg.jpg");

#if UNITY_STANDALONE_OSX

            url = "file://" + path;

#endif

#if UNITY_STANDALONE_LINUX

            url = "file://" + path;

#endif

#if UNITY_STANDALONE_WIN

            url = "file:///" + path;

#endif

            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    backgroundImages.Add(texture);
                }
            }

           
            songNames.Add(folder.Name);

            
        }

        selectionMenu.SetActive(true);

        selectionMenu.GetComponent<SelectionMenu>().SelectionMenuUpdate();

        mask.GetComponent<Animator>().SetBool("Start", true);

        mask.transform.GetChild(0).gameObject.SetActive(false);
    }

}
