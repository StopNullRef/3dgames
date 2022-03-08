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


        public BoStore(SDStore sdStore)
        {
            this.sdStore = sdStore;
        }
    }
}
