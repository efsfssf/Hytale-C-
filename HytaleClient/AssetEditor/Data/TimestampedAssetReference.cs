using System;
using HytaleClient.Protocol;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BDD RID: 3037
	public class TimestampedAssetReference
	{
		// Token: 0x06005FAC RID: 24492 RVA: 0x001EFC2E File Offset: 0x001EDE2E
		public TimestampedAssetReference(string path, string timestamp)
		{
			this.Path = path;
			this.Timestamp = timestamp;
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x001EFC48 File Offset: 0x001EDE48
		public TimestampedAssetReference ToPacket()
		{
			return new TimestampedAssetReference(this.Path, this.Timestamp);
		}

		// Token: 0x04003BBA RID: 15290
		public readonly string Path;

		// Token: 0x04003BBB RID: 15291
		public readonly string Timestamp;
	}
}
