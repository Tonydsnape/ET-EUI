using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgServer))]
    [FriendClass(typeof (ServerInfosComponent))]
    [FriendClass(typeof (ServerInfo))]
    public static class DlgServerSystem
    {
        public static void RegisterUIEvent(this DlgServer self)
        {
            self.View.E_EnterServerButton.AddListenerAsync(self.OnConfirmClickHandler);
            self.View.E_ServerListLoopVerticalScrollRect.AddItemRefreshListener((Transform Transform, int Index) =>
            {
                self.OnLoopListItemRefresh(Transform, Index);
            });
        }

        public static void ShowWindow(this DlgServer self, Entity contextData = null)
        {
            int count = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfosList.Count;
            self.AddUIScrollItems(ref self.serverCellDic, count);
            self.View.E_ServerListLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void HideWindow(this DlgServer self)
        {
            self.RemoveUIScrollItems(ref self.serverCellDic);
        }

        public static async ETTask OnConfirmClickHandler(this DlgServer self)
        {
            bool isSelect = self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId != 0;
            if (!isSelect)
            {
                Log.Error("请选择服务器");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.GetRoles(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }
                
                self.ZoneScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Roles);
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Server);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            
        }
        
        public static void OnSelectServerItemHandler(this DlgServer self, long serverId)
        {
            self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId = int.Parse(serverId.ToString());
            Log.Debug($"当前选择的服务器 Id 是{self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId}");
            self.View.E_ServerListLoopVerticalScrollRect.RefillCells();
        }

        public static void OnLoopListItemRefresh(this DlgServer self, Transform transform, int index)
        {
            Scroll_Item_serverCell item = self.serverCellDic[index].BindTrans(transform);
            ServerInfo info = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfosList[index];
            item.EImageBgImage.color = info.Id == self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId? Color.red : Color.cyan;
            item.E_ServerNameText.SetText(info.ServerName);
            item.E_SelectButtonButton.AddListener(() => { self.OnSelectServerItemHandler(info.Id); });
        }
    }
}