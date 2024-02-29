
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[EnableMethod]
	public  class Scroll_Item_serverCell : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_Item_serverCell BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Image EImageBgImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_EImageBgImage == null )
     				{
		    			this.m_EImageBgImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EImageBg");
     				}
     				return this.m_EImageBgImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EImageBg");
     			}
     		}
     	}

		public UnityEngine.UI.Text E_ServerNameText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_E_ServerNameText == null )
     				{
		    			this.m_E_ServerNameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_ServerName");
     				}
     				return this.m_E_ServerNameText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_ServerName");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EImageBgImage = null;
			this.m_E_ServerNameText = null;
			this.uiTransform = null;
			this.DataId = 0;
		}

		private UnityEngine.UI.Image m_EImageBgImage = null;
		private UnityEngine.UI.Text m_E_ServerNameText = null;
		public Transform uiTransform = null;
	}
}
