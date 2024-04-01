using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgRoles))]
    [FriendClass(typeof (RoleInfosComponent))]
    [FriendClass(typeof (RoleInfo))]
    public static class DlgRolesSystem
    {
        public static void RegisterUIEvent(this DlgRoles self)
        {
            self.View.E_DeleteRoleButton.AddListenerAsync(self.OnDeleteRoleClickHandler);
            self.View.E_CreateRoleButton.AddListenerAsync(self.OnCreateRoleClickHandler);
            self.View.E_StartGameButton.AddListener(self.OnStartGameClickHandler);
            self.View.E_RoleInfoListLoopHorizontalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
            {
                self.OnRoleListRefreshHandler(transform, index);
            });
        }

        public static void ShowWindow(this DlgRoles self, Entity contextData = null)
        {
            self.RefreshRoleList();
        }

        public static void RefreshRoleList(this DlgRoles self)
        {
            int count = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos.Count;
            self.AddUIScrollItems(ref self.RolesCellDic, count);
            self.View.E_RoleInfoListLoopHorizontalScrollRect.SetVisible(true, count);
        }

        public static void OnRoleListRefreshHandler(this DlgRoles self, Transform transform, int index)
        {
            Scroll_Item_Role item = self.RolesCellDic[index].BindTrans(transform);
            RoleInfo info = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos[index];

            item.EButton_SelectImage.color = info.Id == self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId? Color.green : Color.gray;

            item.ELabel_NameText.SetText(info.Name);
            item.EButton_SelectButton.AddListener(() => { self.OnRoleItemClickHandler(info.Id); });
        }

        public static void OnRoleItemClickHandler(this DlgRoles self, long roleId)
        {
            self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = roleId;
            self.View.E_RoleInfoListLoopHorizontalScrollRect.RefillCells();
        }

        public static async ETTask OnDeleteRoleClickHandler(this DlgRoles self)
        {
            long roleId = self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId;
            if (roleId == 0)
            {
                Log.Error("请选择角色");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.DeleteRole(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                self.RefreshRoleList();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static async ETTask OnCreateRoleClickHandler(this DlgRoles self)
        {
            Log.Console("OnCreateRoleClickHandler");
            string name = self.View.E_nameInputInputField.text;

            if (string.IsNullOrEmpty(name))
            {
                Log.Error("名字不能为空");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.CreateRole(self.ZoneScene(), name);
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                self.RefreshRoleList();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void OnStartGameClickHandler(this DlgRoles self)
        {
            if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
            {
                Log.Error("请选择角色");
                return;
            }

            // try
            // {
            //     //int errorCode = await LoginHelper
            // }
        }
    }
}