using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DB
{
    [Serializable]
    public class BoBuilditem
    {
        public SDBuildItem sdBuildItem;

        public BoBuilditem(SDBuildItem buildItem)
        {
            this.sdBuildItem = buildItem;
        }
    }
}
