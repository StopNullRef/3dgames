using Project.DB;
using Project.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class StoreSlot :  MonoBehaviour, IPoolableObject
    {
        // TODO 03/08 버튼클릭으로 구현하지 말고
        // 마우스가 해당 범위안에 들어왔을때
        // 마우스 우클릭이냐 좌클릭이냐 에따라 다르게 처리
        // ex) 좌클릭시 한번만 구매 우클릭이 임이의 창이 떠서 몇개 구매할건지 입력후 구매

        public Image saleImage;
        public Text saleItemName;

        public Image haveImage;
        public Text haveText;

        public Image costImage;
        public Text costText;

        public bool CanRecycle { get; set; }

        public void Start()
        {
                        
        }

        public void Initilaize(BoStore bostore)
        {
            //TODO 구현하기
        }
    }
}
