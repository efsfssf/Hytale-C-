using System;
using HytaleClient.Interface.Messages;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BDE RID: 3038
	public class TrackedAsset
	{
		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x06005FAE RID: 24494 RVA: 0x001EFC6B File Offset: 0x001EDE6B
		public bool IsAvailable
		{
			get
			{
				return !this.IsLoading && this.FetchError == null && this.Data != null;
			}
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x001EFC89 File Offset: 0x001EDE89
		public TrackedAsset(AssetReference assetReference, object data)
		{
			this.Reference = assetReference;
			this.Data = data;
		}

		// Token: 0x04003BBC RID: 15292
		public readonly AssetReference Reference;

		// Token: 0x04003BBD RID: 15293
		public object Data;

		// Token: 0x04003BBE RID: 15294
		public bool IsLoading;

		// Token: 0x04003BBF RID: 15295
		public FormattedMessage FetchError;
	}
}
