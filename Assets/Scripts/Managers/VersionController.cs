using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

public class VersionController : MonoBehaviour
{
    string version, version1;
    void Start()
    {
        using (var req = new HttpClient())
        {
            var response = req.GetAsync("https://api.tmgrup.com.tr/config/351");
            var jsonText = response.Result.Content.ReadAsStringAsync().Result;
            JObject jo = JObject.Parse(jsonText);
            version = jo.SelectToken("data").SelectToken("version").ToString();
            version1 = version;
        }
        string myVersion = Application.version.ToString();
        if (myVersion != version1)
            Debug.Log("final version");
    }
}
