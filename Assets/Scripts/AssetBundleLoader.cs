using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
    private string url = "https://drive.google.com/uc?export=download&id=1ZftkpwH2XwELKOaT3KiWPTx9zu_JWvLq";
    [SerializeField] private List<string> nameFiles = new List<string>();
     
    public void StartLoadAsset(string url, string name, Hash128 hash, Action<AssetBundle> onLoadAssetBundle = null)
    {
        StartCoroutine(LoadAssetBundle(url,name, hash, onLoadAssetBundle));
    }

    private IEnumerator LoadAssetBundle(string url, string name, Hash128 hash, Action<AssetBundle> onLoadAssetBundle)
    {
        //TODO: чистка кеша от старых моделей
        var request = UnityWebRequestAssetBundle.GetAssetBundle(url, hash);
        
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);

            foreach (var item in assetBundle.GetAllAssetNames())
            {
                Debug.Log(item);
            }
            onLoadAssetBundle?.Invoke(assetBundle);            
        }
    }

    //tested
    public void StartLoadSuperBundle(string url, string name, Action onLoadAssetBundle = null)
    {
        StartCoroutine(LoadSuperBundle(url, name,  onLoadAssetBundle));
    }

    //tested
    private IEnumerator LoadSuperBundle(string url, string name, Action onLoadAssetBundle = null)
    {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {                        
            File.WriteAllBytes(CacheClass.namePath + name, request.downloadHandler.data);
            onLoadAssetBundle?.Invoke();
        }
    }
    
    //tested
    public void StartLoadSuperManifest(string url, Action<string> onLoadManifest = null)
    {
        StartCoroutine(LoadManifest(url, onLoadManifest));
    }
    //tested
    private IEnumerator LoadManifest(string url, Action<string> onLoadManifest = null)
    {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {
            var superManifest = request.downloadHandler.text;
            onLoadManifest?.Invoke(superManifest);
        }
    }
}
