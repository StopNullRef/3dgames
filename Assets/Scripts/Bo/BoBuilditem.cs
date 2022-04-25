using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DB
{
    [Serializable]
    public class BoBuildItem
    {
        public SDBuildItem sdBuildItem;

        public BoBuildItem(SDBuildItem buildItem)
        {
            this.sdBuildItem = buildItem;
        }
    }
}
