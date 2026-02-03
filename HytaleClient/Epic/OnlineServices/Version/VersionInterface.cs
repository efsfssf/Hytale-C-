using System;

namespace Epic.OnlineServices.Version
{
	// Token: 0x02000023 RID: 35
	public sealed class VersionInterface
	{
		// Token: 0x0600034D RID: 845 RVA: 0x00004AB0 File Offset: 0x00002CB0
		public static Utf8String GetVersion()
		{
			IntPtr from = Bindings.EOS_GetVersion();
			Utf8String result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x04000145 RID: 325
		public static readonly Utf8String CompanyName = "Epic Games, Inc.";

		// Token: 0x04000146 RID: 326
		public static readonly Utf8String CopyrightString = "Copyright Epic Games, Inc. All Rights Reserved.";

		// Token: 0x04000147 RID: 327
		public const int MajorVersion = 1;

		// Token: 0x04000148 RID: 328
		public const int MinorVersion = 17;

		// Token: 0x04000149 RID: 329
		public const int PatchVersion = 0;

		// Token: 0x0400014A RID: 330
		public static readonly Utf8String ProductIdentifier = "Epic Online Services SDK";

		// Token: 0x0400014B RID: 331
		public static readonly Utf8String ProductName = "Epic Online Services SDK";
	}
}
