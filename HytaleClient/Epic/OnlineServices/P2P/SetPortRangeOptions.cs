using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007AA RID: 1962
	public struct SetPortRangeOptions
	{
		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x0004BDA2 File Offset: 0x00049FA2
		// (set) Token: 0x060032E0 RID: 13024 RVA: 0x0004BDAA File Offset: 0x00049FAA
		public ushort Port { get; set; }

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x060032E1 RID: 13025 RVA: 0x0004BDB3 File Offset: 0x00049FB3
		// (set) Token: 0x060032E2 RID: 13026 RVA: 0x0004BDBB File Offset: 0x00049FBB
		public ushort MaxAdditionalPortsToTry { get; set; }
	}
}
