using System;
using Coherent.UI.Binding;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF4 RID: 2804
	[CoherentType]
	public class ClientItemIconProperties
	{
		// Token: 0x0600583C RID: 22588 RVA: 0x001AC370 File Offset: 0x001AA570
		public ClientItemIconProperties(ItemBase.ItemIconProperties properties)
		{
			this.Scale = properties.Scale;
			bool flag = properties.Translation != null;
			if (flag)
			{
				this.Translation = new Vector2?(new Vector2(properties.Translation.X, properties.Translation.Y));
			}
			bool flag2 = properties.Rotation != null;
			if (flag2)
			{
				this.Rotation = new Vector3?(new Vector3(properties.Rotation.X, properties.Rotation.Y, properties.Rotation.Z));
			}
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x001AC402 File Offset: 0x001AA602
		public ClientItemIconProperties()
		{
		}

		// Token: 0x040036B5 RID: 14005
		[CoherentProperty("scale")]
		public float Scale;

		// Token: 0x040036B6 RID: 14006
		[CoherentProperty("translation")]
		public Vector2? Translation;

		// Token: 0x040036B7 RID: 14007
		[CoherentProperty("rotation")]
		public Vector3? Rotation;
	}
}
