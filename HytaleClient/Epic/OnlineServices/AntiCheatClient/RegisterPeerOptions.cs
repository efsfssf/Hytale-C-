using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006FA RID: 1786
	public struct RegisterPeerOptions
	{
		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06002DF4 RID: 11764 RVA: 0x00043D51 File Offset: 0x00041F51
		// (set) Token: 0x06002DF5 RID: 11765 RVA: 0x00043D59 File Offset: 0x00041F59
		public IntPtr PeerHandle { get; set; }

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06002DF6 RID: 11766 RVA: 0x00043D62 File Offset: 0x00041F62
		// (set) Token: 0x06002DF7 RID: 11767 RVA: 0x00043D6A File Offset: 0x00041F6A
		public AntiCheatCommonClientType ClientType { get; set; }

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06002DF8 RID: 11768 RVA: 0x00043D73 File Offset: 0x00041F73
		// (set) Token: 0x06002DF9 RID: 11769 RVA: 0x00043D7B File Offset: 0x00041F7B
		public AntiCheatCommonClientPlatform ClientPlatform { get; set; }

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06002DFA RID: 11770 RVA: 0x00043D84 File Offset: 0x00041F84
		// (set) Token: 0x06002DFB RID: 11771 RVA: 0x00043D8C File Offset: 0x00041F8C
		public uint AuthenticationTimeout { get; set; }

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06002DFC RID: 11772 RVA: 0x00043D95 File Offset: 0x00041F95
		// (set) Token: 0x06002DFD RID: 11773 RVA: 0x00043D9D File Offset: 0x00041F9D
		internal Utf8String AccountId_DEPRECATED { get; set; }

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06002DFE RID: 11774 RVA: 0x00043DA6 File Offset: 0x00041FA6
		// (set) Token: 0x06002DFF RID: 11775 RVA: 0x00043DAE File Offset: 0x00041FAE
		public Utf8String IpAddress { get; set; }

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x00043DB7 File Offset: 0x00041FB7
		// (set) Token: 0x06002E01 RID: 11777 RVA: 0x00043DBF File Offset: 0x00041FBF
		public ProductUserId PeerProductUserId { get; set; }
	}
}
