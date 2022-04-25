using Project.DB;
using Project.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Object
{
    public class BuildItem : MonoBehaviour,IPoolableObject
    {
        public BoBuildItem boItem;

        public float count;

        public bool CanRecycle { get ; set ; }

        public void Init()
        {
           boItem = new BoBuildItem(GameManager.SD.sdBuildItems.Find(_ => _.name == gameObject.name));
        }

        public void PoolInit()
        {
            Init();
        }
    }
}
