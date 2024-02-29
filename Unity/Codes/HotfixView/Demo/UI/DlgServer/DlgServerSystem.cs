using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgServer))]
    [FriendClass(typeof (ServerInfosComponent))]
    public static class DlgServerSystem
    {
        public static void RegisterUIEvent(this DlgServer self)
        {
            self.View.E_EnterServerButton.AddListener(self.OnEnterServerClickHandler);
            self.View.E_ServerListLoopVerticalScrollRect.AddItemRefreshListener((Transform Transform, int Index) =>
            {
                self.OnLoopListItemRefresh(Transform, Index);
            });
        }

        public static void ShowWindow(this DlgServer self, Entity contextData = null)
        {
            int count = 10;
            self.AddUIScrollItems(ref self.serverCellDic, count);
            self.View.E_ServerListLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void HideWindow(this DlgServer self)
        {
            self.RemoveUIScrollItems(ref self.serverCellDic);
        }

        public static void OnEnterServerClickHandler(this DlgServer self)
        {
            Log.Debug("进入服务器按钮被点击了");
        }

        public static void OnLoopListItemRefresh(this DlgServer self, Transform Transform, int Index)
        {
            Scroll_Item_serverCell item = self.serverCellDic[Index];
            item.E_ServerNameText.text = $"{Index}服";
        }
    }
}