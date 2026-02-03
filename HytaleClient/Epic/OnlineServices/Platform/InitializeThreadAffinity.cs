using System;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000712 RID: 1810
	public struct InitializeThreadAffinity
	{
		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x0004533E File Offset: 0x0004353E
		// (set) Token: 0x06002EBA RID: 11962 RVA: 0x00045346 File Offset: 0x00043546
		public ulong NetworkWork { get; set; }

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x0004534F File Offset: 0x0004354F
		// (set) Token: 0x06002EBC RID: 11964 RVA: 0x00045357 File Offset: 0x00043557
		public ulong StorageIo { get; set; }

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x00045360 File Offset: 0x00043560
		// (set) Token: 0x06002EBE RID: 11966 RVA: 0x00045368 File Offset: 0x00043568
		public ulong WebSocketIo { get; set; }

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x00045371 File Offset: 0x00043571
		// (set) Token: 0x06002EC0 RID: 11968 RVA: 0x00045379 File Offset: 0x00043579
		public ulong P2PIo { get; set; }

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x00045382 File Offset: 0x00043582
		// (set) Token: 0x06002EC2 RID: 11970 RVA: 0x0004538A File Offset: 0x0004358A
		public ulong HttpRequestIo { get; set; }

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x00045393 File Offset: 0x00043593
		// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x0004539B File Offset: 0x0004359B
		public ulong RTCIo { get; set; }

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000453A4 File Offset: 0x000435A4
		// (set) Token: 0x06002EC6 RID: 11974 RVA: 0x000453AC File Offset: 0x000435AC
		public ulong EmbeddedOverlayMainThread { get; set; }

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x000453B5 File Offset: 0x000435B5
		// (set) Token: 0x06002EC8 RID: 11976 RVA: 0x000453BD File Offset: 0x000435BD
		public ulong EmbeddedOverlayWorkerThreads { get; set; }

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000453C8 File Offset: 0x000435C8
		internal void Set(ref InitializeThreadAffinityInternal other)
		{
			this.NetworkWork = other.NetworkWork;
			this.StorageIo = other.StorageIo;
			this.WebSocketIo = other.WebSocketIo;
			this.P2PIo = other.P2PIo;
			this.HttpRequestIo = other.HttpRequestIo;
			this.RTCIo = other.RTCIo;
			this.EmbeddedOverlayMainThread = other.EmbeddedOverlayMainThread;
			this.EmbeddedOverlayWorkerThreads = other.EmbeddedOverlayWorkerThreads;
		}
	}
}
