using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
public class ForceUpdate : MonoBehaviour
{
    [SerializeField] GameObject versionPanel;
    string version;
    void Start()
    {
        versionPanel.SetActive(false);
        using (var req = new HttpClient())
        {
            var response = req.GetAsync("https://api.tmgrup.com.tr/config/471");
            var jsonText = response.Result.Content.ReadAsStringAsync().Result;
            JObject jo = JObject.Parse(jsonText);
            version = jo.SelectToken("data").SelectToken("tetris").SelectToken("androidversion").ToString();
        }
        string myVersion = Application.version.ToString();
        Debug.Log(myVersion);
        Debug.Log(version + "api");
        if (version != myVersion)
        {
            Debug.Log("Version Control");
            versionPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void getTolink()
    {
        string url = "https://www.google.com";
        Application.OpenURL(url);
    }
}
