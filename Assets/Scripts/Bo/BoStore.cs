using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DB
{
    [Serializable]
    public class BoStore
    {
        public SDStore sdStore;

        /// <summary>
        /// 유저와 상호작용중인지 체크하는 불타입변수
        /// </summary>
        public bool interaction;

        public BoStore(SDStore sdStore)
        {
            this.sdStore = sdStore;
        }
    }
}
