using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    /// <summary>
    /// 캐릭터 State패턴에사용할
    /// 캐릭터 상태 열거형
    /// </summary>
    public enum State
    {
        Idle,
        Move,
        Action, // 무언가 행동할때??
    }

    /// <summary>
    /// buildindex 순서대로
    /// 열거한 Scene종류
    /// </summary>
    public enum Scene
    {
        LoadingScene,
        ForestScene,
        MineScene,
        HomeScene,
    }

    /// <summary>
    /// GameObject에 넣을 Tag
    /// </summary>
    public enum TagName
    {
        Tree,
        Rock,
        Grass,
    }


    /// <summary>
    /// 리소스 참조해올 리소스 경로를 담고 있는 클래스
    /// </summary>
    public class ResourcePath
    {
        public const string storeSlot = "Prefabs/UI/SaleSlot";
        #region ItemPath
        public const string woodItem = "Texture/ItemIcon/Material/Wooden Plank";
        #endregion

        #region CursorPath
        public const string basicCursor = "Texture/Cursors/1_Basic";
        public const string treeCursor = "Texture/Cursors/2_Tree";
        #endregion
    }

    public class StaticDataPath
    {
        public const string SDPath = "Assets/StaticData";
        public const string SDJson = "Assets/StaticData/Json";
        public const string SDExcel = "Assets/StaticData/Excel";
    }

    /// <summary>
    /// GameObject에 사용할 Layer 번호들을 담고 있는 클래스
    /// </summary>
    public class LayerNum
    {
        public const int objLayer = 1 << 10;
        public const int UILayer = 1 << 5;
        public const int grassLayer = 1 << 11;
    }

    /// <summary>
    /// 아이템 타입을 나타내는 열거형
    /// </summary>
    public enum ItemType
    {
        None,       // 기본 아무것도 아닐때
        Equipment,  // 장비
        Potion,     // 포션
        Ingredient, // 재료 아이템
    }

    /// <summary>
    /// Game상에 최대 갯수
    /// </summary>
    public class MaxCount
    {
        public const int ingredient = 999;
        public const int equipment = 1;
        public const int potion = 999;
        /// <summary>
        /// object와 캐릭터 사이 거리를 나타내는 상수 값
        /// </summary>
        public const float objectToDistance = 0.5f;

        /// <summary>
        /// 오브젝트에서 재료아이템 최대 드랍갯수
        /// </summary>
        public const int ObjectMaxDrop = 900;

        public const int invenSlotCount = 25;

    }

    /// <summary>
    /// SciptableObject 형태로 가지고있는
    /// 아이템의 이름들의 열거형
    /// </summary>
    public enum ScriptableItem
    {
        Stone,
        Wood,
        None,
    }

    /// <summary>
    /// InGame도중 하이에라키에 찾을 string 값을 상수로 갖는 클래스
    /// </summary>
    public class FindDataString
    {
        public const string ingameUI = "InGameUICanvas";
    }

    /// <summary>
    /// 손에 무엇을 들고있는지 체크하는 열거형
    /// </summary>
    public enum Tool

    {
        None,    // 아무것도없을때
        Axe,     // 도끼
        PickAxe, // 곡괭이
    }

    /// <summary>
    /// 캐릭터 애니메이션 트랜지션을 가지고 있는 열거형
    /// </summary>
    public enum CharacerAnimTransition
    {
        IsMoving,
        IsSalute,
        IsWood,
        IsMining,
        End,
    }

    /// <summary>
    /// Object Pool에 사용할 poolType 열거형
    /// </summary>
    public enum PoolType
    {
        None,
        Object,
        Monster,
        Item,
    }

    /// <summary>
    /// 인벤토리를 통해 저장할 아이템 정보들 담은 구조체
    /// </summary>
    public struct ItemSaveInfo
    {
        public Define.ScriptableItem itemInfo;
        public int Count;
    }

    /// <summary>
    /// 카메라 상태
    /// </summary>
    public enum CameraState
    {
        Build,
        None,
    }
    

    /// <summary>
    /// 해당 캔버스의 타입
    /// </summary>
    public enum CanvasType
    {
        None,
        Loading,
        Building,
        Ingame,
    }

    /// <summary>
    /// UI타입
    /// </summary>
    public enum UIType
    {
        None,
        Building,
        Ingame,
    }

    public enum ColorType
    {
        Invisible, // 안보이게
        Translucent, // 반투명
        Visible,  // 보이게
    }

}
