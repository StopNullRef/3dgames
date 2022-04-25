using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    /// <summary>
    /// ĳ���� State���Ͽ������
    /// ĳ���� ���� ������
    /// </summary>
    public enum State
    {
        Idle,
        Move,
        Action, // ���� �ൿ�Ҷ�??
    }

    /// <summary>
    /// buildindex �������
    /// ������ Scene����
    /// </summary>
    public enum Scene
    {
        LoadingScene,
        ForestScene,
        MineScene,
        HomeScene,
    }

    /// <summary>
    /// GameObject�� ���� Tag
    /// </summary>
    public enum TagName
    {
        Tree,
        Rock,
        Grass,
    }


    /// <summary>
    /// ���ҽ� �����ؿ� ���ҽ� ��θ� ��� �ִ� Ŭ����
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
    /// GameObject�� ����� Layer ��ȣ���� ��� �ִ� Ŭ����
    /// </summary>
    public class LayerNum
    {
        public const int objLayer = 1 << 10;
        public const int UILayer = 1 << 5;
        public const int grassLayer = 1 << 11;
    }

    /// <summary>
    /// ������ Ÿ���� ��Ÿ���� ������
    /// </summary>
    public enum ItemType
    {
        None,       // �⺻ �ƹ��͵� �ƴҶ�
        Equipment,  // ���
        Potion,     // ����
        Ingredient, // ��� ������
    }

    /// <summary>
    /// Game�� �ִ� ����
    /// </summary>
    public class MaxCount
    {
        public const int ingredient = 999;
        public const int equipment = 1;
        public const int potion = 999;
        /// <summary>
        /// object�� ĳ���� ���� �Ÿ��� ��Ÿ���� ��� ��
        /// </summary>
        public const float objectToDistance = 0.5f;

        /// <summary>
        /// ������Ʈ���� �������� �ִ� �������
        /// </summary>
        public const int ObjectMaxDrop = 900;

        public const int invenSlotCount = 25;

    }

    /// <summary>
    /// SciptableObject ���·� �������ִ�
    /// �������� �̸����� ������
    /// </summary>
    public enum ScriptableItem
    {
        Stone,
        Wood,
        None,
    }

    /// <summary>
    /// InGame���� ���̿���Ű�� ã�� string ���� ����� ���� Ŭ����
    /// </summary>
    public class FindDataString
    {
        public const string ingameUI = "InGameUICanvas";
    }

    /// <summary>
    /// �տ� ������ ����ִ��� üũ�ϴ� ������
    /// </summary>
    public enum Tool

    {
        None,    // �ƹ��͵�������
        Axe,     // ����
        PickAxe, // ���
    }

    /// <summary>
    /// ĳ���� �ִϸ��̼� Ʈ�������� ������ �ִ� ������
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
    /// Object Pool�� ����� poolType ������
    /// </summary>
    public enum PoolType
    {
        None,
        Object,
        Monster,
        Item,
    }

    /// <summary>
    /// �κ��丮�� ���� ������ ������ ������ ���� ����ü
    /// </summary>
    public struct ItemSaveInfo
    {
        public Define.ScriptableItem itemInfo;
        public int Count;
    }

    /// <summary>
    /// ī�޶� ����
    /// </summary>
    public enum CameraState
    {
        Build,
        None,
    }
    

    /// <summary>
    /// �ش� ĵ������ Ÿ��
    /// </summary>
    public enum CanvasType
    {
        None,
        Loading,
        Building,
        Ingame,
    }

    /// <summary>
    /// UIŸ��
    /// </summary>
    public enum UIType
    {
        None,
        Building,
        Ingame,
    }

    public enum ColorType
    {
        Invisible, // �Ⱥ��̰�
        Translucent, // ������
        Visible,  // ���̰�
    }

}
