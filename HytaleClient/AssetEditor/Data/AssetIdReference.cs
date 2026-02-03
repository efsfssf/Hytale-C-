using System;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD5 RID: 3029
	public struct AssetIdReference
	{
		// Token: 0x06005F6A RID: 24426 RVA: 0x001ECEBC File Offset: 0x001EB0BC
		public AssetIdReference(string type, string id)
		{
			this.Type = type;
			this.Id = id;
		}

		// Token: 0x04003B61 RID: 15201
		public static readonly AssetIdReference None = default(AssetIdReference);

		// Token: 0x04003B62 RID: 15202
		public readonly string Type;

		// Token: 0x04003B63 RID: 15203
		public readonly string Id;
	}
}
