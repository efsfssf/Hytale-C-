using System;
using HytaleClient.AssetEditor.Interface.Config;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD2 RID: 3026
	internal struct AssetDiagnosticMessage
	{
		// Token: 0x06005F62 RID: 24418 RVA: 0x001ECDF8 File Offset: 0x001EAFF8
		public AssetDiagnosticMessage(PropertyPath property, string message)
		{
			this.Property = property;
			this.Message = message;
		}

		// Token: 0x06005F63 RID: 24419 RVA: 0x001ECE09 File Offset: 0x001EB009
		public AssetDiagnosticMessage(string property, string message)
		{
			this = new AssetDiagnosticMessage(PropertyPath.FromString(property), message);
		}

		// Token: 0x04003B57 RID: 15191
		public readonly string Message;

		// Token: 0x04003B58 RID: 15192
		public readonly PropertyPath Property;
	}
}
