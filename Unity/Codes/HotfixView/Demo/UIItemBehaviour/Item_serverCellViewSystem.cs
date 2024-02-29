
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class Scroll_Item_serverCellDestroySystem : DestroySystem<Scroll_Item_serverCell> 
	{
		public override void Destroy( Scroll_Item_serverCell self )
		{
			self.DestroyWidget();
		}
	}
}
