using System;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD3 RID: 3027
	internal struct AssetDiagnostics
	{
		// Token: 0x06005F64 RID: 24420 RVA: 0x001ECE1A File Offset: 0x001EB01A
		public AssetDiagnostics(AssetDiagnosticMessage[] errors, AssetDiagnosticMessage[] warnings)
		{
			this.Errors = errors;
			this.Warnings = warnings;
		}

		// Token: 0x04003B59 RID: 15193
		public static readonly AssetDiagnostics None = new AssetDiagnostics(null, null);

		// Token: 0x04003B5A RID: 15194
		public readonly AssetDiagnosticMessage[] Errors;

		// Token: 0x04003B5B RID: 15195
		public readonly AssetDiagnosticMessage[] Warnings;
	}
}
