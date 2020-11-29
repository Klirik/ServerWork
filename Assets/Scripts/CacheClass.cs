using System.IO;
using UnityEngine;

public static class CacheClass
{
    public static string namePath { get; private set; } = "CacheFolder";
    public static bool isInited { get; private set; } = false;

    public static Cache cache { get; private set; }

    public static void Init()
    {
        if (!isInited)
        {
            if (!Directory.Exists(namePath))
            {
                Directory.CreateDirectory(namePath);
            }
            //сохранили место куда мы пишем и откуда получаем
            cache = Caching.AddCache(namePath);

            //кешей может быть, но несколько, но пишу в один
            if (cache.valid)
            {
                Caching.currentCacheForWriting = cache;
                isInited = true;
            }
            else
            {
                Debug.LogError("namePath doesn't valid " + namePath);
            }
        }
    }
}
