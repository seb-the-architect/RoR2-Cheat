using UnityEngine;

namespace HellTaker
{
    public class Loader
    {
        public static GameObject _Load;

        public static void Init()
        {
            _Load = new GameObject();

            _Load.AddComponent<MyMod>();

            Object.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            Object.Destroy(_Load);
        }

    }
}