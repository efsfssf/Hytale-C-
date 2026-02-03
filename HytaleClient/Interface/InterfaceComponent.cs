using System;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface
{
	// Token: 0x02000804 RID: 2052
	internal abstract class InterfaceComponent : Element
	{
		// Token: 0x06003900 RID: 14592 RVA: 0x0007693F File Offset: 0x00074B3F
		public InterfaceComponent(Interface @interface, Element parent) : base(@interface.Desktop, parent)
		{
			this.Interface = @interface;
		}

		// Token: 0x040018BD RID: 6333
		public readonly Interface Interface;
	}
}
