using System;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x0200034D RID: 845
	public struct BeginPlayerSessionOptions
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00021BEA File Offset: 0x0001FDEA
		// (set) Token: 0x06001723 RID: 5923 RVA: 0x00021BF2 File Offset: 0x0001FDF2
		public BeginPlayerSessionOptionsAccountId AccountId { get; set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00021BFB File Offset: 0x0001FDFB
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x00021C03 File Offset: 0x0001FE03
		public Utf8String DisplayName { get; set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x00021C0C File Offset: 0x0001FE0C
		// (set) Token: 0x06001727 RID: 5927 RVA: 0x00021C14 File Offset: 0x0001FE14
		public UserControllerType ControllerType { get; set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x00021C1D File Offset: 0x0001FE1D
		// (set) Token: 0x06001729 RID: 5929 RVA: 0x00021C25 File Offset: 0x0001FE25
		public Utf8String ServerIp { get; set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x00021C2E File Offset: 0x0001FE2E
		// (set) Token: 0x0600172B RID: 5931 RVA: 0x00021C36 File Offset: 0x0001FE36
		public Utf8String GameSessionId { get; set; }
	}
}
