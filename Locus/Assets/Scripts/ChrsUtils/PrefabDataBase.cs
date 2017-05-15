using UnityEngine;

namespace ChrsUtils
{
        namespace PrefabDataBase
        {
        [CreateAssetMenu (menuName = "Prefab DB")]
        public class PrefabDataBase : ScriptableObject
        {

            [SerializeField] private GameObject _player;
            public GameObject Player
            {
                get { return _player; }
            }

            [SerializeField] private GameObject[] _locus;
            public GameObject[] Locus
            {
                get { return _locus; }
            }
        }
    }
}