using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CheckAndUpdate : MonoBehaviour
{
    [SerializeField] private AssetBundleLoader assetBundleLoader;

    private const string superBundleLink = "https://drive.google.com/uc?export=download&id=16LA9H8MdZU6BOkb5JKyGPf76WQzIVHHw";
    private const string superBundleManifestLink = "https://drive.google.com/uc?export=download&id=1HjGOxIPuqgCaG7GbbHQ0AqP81Qteha47";

    private Dictionary<string,string> modelUrls = new Dictionary<string, string>
    {
        { "bluemodel" , "https://drive.google.com/uc?export=download&id=1rjRkXPlWB7dpvoEWo_V6ivZLQxmkxVNb" },
        { "neonmodel" , "https://drive.google.com/uc?export=download&id=11S4qxpbby5zRye4y73hdNyyF7GAOO7tL" },
        { "standartmodel" , "https://drive.google.com/uc?export=download&id=1ibWkw06QGTFloXen0KSrZ-nmyF9aVCoQ"}        
    };

    private const string nameSuperBundle = "AssetBundle";

    private AssetBundle superAssetBundle;
    private AssetBundleManifest manifest;

    private List<string> namesBundles = new List<string>();
    private List<Hash128> hashesBundles = new List<Hash128>();

    private string superManifest = string.Empty;

    private void Awake()
    {
        CacheClass.Init();

        assetBundleLoader.StartLoadSuperBundle(superBundleLink, nameSuperBundle, () =>
        {
            superAssetBundle = AssetBundle.LoadFromFile(CacheClass.namePath + nameSuperBundle);

            manifest = superAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            namesBundles.AddRange(manifest.GetAllAssetBundles());

            foreach (var name in namesBundles)
            {
                StartLoadModel(name);
            }
        });

        assetBundleLoader.StartLoadSuperManifest(superBundleManifestLink, CheckManifest);

        //File.Exists(CacheClass.namePath + name)
        //AssetBundle.LoadFromFile(CacheClass.namePath + name);
    }

    private void StartLoadModel(string name)
    {
        Hash128 hash = manifest.GetAssetBundleHash(name);
        hashesBundles.Add(hash);
        assetBundleLoader.StartLoadAsset(modelUrls[name], name, hash);
    }


    private void OnLoadAsset(AssetBundle assetBundle)
    {

    }
         
    public void CheckManifest(string manifest)
    {        
        long crc = ParseCRC(manifest);
        //AssetBundle.LoadFromFile(CacheClass.namePath, 0);

    }

    //tested
    private long ParseCRC(string manifest)
    {
        //получаем crc
        var crcRow = manifest.Split("\n".ToCharArray())[1];
        var crc = long.Parse(crcRow.Split(':')[1].Trim());
        return crc;
    }

    public void CheckSuperBundle()
    {
        //superAssetBundle
    }

    private void CheckCache()
    {
        CacheClass.Init();

        if (CacheClass.isInited)
        {

        }
    }
}




/*
 * https://sites.google.com/site/gdocs2direct/home
 * https://habr.com/ru/post/433366/
 * https://www.labnol.org/internet/direct-links-for-google-drive/28356/
 * https://docs.unity3d.com/ScriptReference/BuildPipeline.GetCRCForAssetBundle.html
 * https://docs.unity3d.com/ScriptReference/Caching.html !!!
 
1: я качаю супер-бандл и супер-манифест + говной. И сохраняю супер-бандл, супер-манифест в кэш

2: я качаю супер-манифест, чекаю CRC;

2.1: если CRC не изменилось - break.

2.2: если CRC изменилось у супер-бандла - качаем супер-бандл
    
2.2.1: качаем манифест всех говной из кеша.

2.2.1.1: если CRC совпадают - break.

2.2.1.2: если CRC не совпадают чистим кеш от стартого говной. И качаем новое говной и сохраняем.
*/
