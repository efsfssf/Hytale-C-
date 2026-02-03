using System;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD7 RID: 3031
	public struct AssetReference : IEquatable<AssetReference>
	{
		// Token: 0x06005F81 RID: 24449 RVA: 0x001EDB51 File Offset: 0x001EBD51
		public AssetReference(string type, string filePath)
		{
			this.Type = type;
			this.FilePath = filePath;
		}

		// Token: 0x06005F82 RID: 24450 RVA: 0x001EDB64 File Offset: 0x001EBD64
		public bool Equals(AssetReference other)
		{
			return this.Type == other.Type && this.FilePath == other.FilePath;
		}

		// Token: 0x06005F83 RID: 24451 RVA: 0x001EDBA0 File Offset: 0x001EBDA0
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is AssetReference)
			{
				AssetReference other = (AssetReference)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005F84 RID: 24452 RVA: 0x001EDBCC File Offset: 0x001EBDCC
		public override int GetHashCode()
		{
			return ((this.Type != null) ? this.Type.GetHashCode() : 0) * 397 ^ ((this.FilePath != null) ? this.FilePath.GetHashCode() : 0);
		}

		// Token: 0x06005F85 RID: 24453 RVA: 0x001EDC14 File Offset: 0x001EBE14
		public override string ToString()
		{
			return "Type: " + this.Type + ", FilePath: " + this.FilePath;
		}

		// Token: 0x04003B69 RID: 15209
		public static readonly AssetReference None = new AssetReference(null, null);

		// Token: 0x04003B6A RID: 15210
		public readonly string Type;

		// Token: 0x04003B6B RID: 15211
		public readonly string FilePath;
	}
}
