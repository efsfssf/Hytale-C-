using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000713 RID: 1811
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct InitializeThreadAffinityInternal : IGettable<InitializeThreadAffinity>, ISettable<InitializeThreadAffinity>, IDisposable
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x00045440 File Offset: 0x00043640
		// (set) Token: 0x06002ECB RID: 11979 RVA: 0x00045458 File Offset: 0x00043658
		public ulong NetworkWork
		{
			get
			{
				return this.m_NetworkWork;
			}
			set
			{
				this.m_NetworkWork = value;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06002ECC RID: 11980 RVA: 0x00045464 File Offset: 0x00043664
		// (set) Token: 0x06002ECD RID: 11981 RVA: 0x0004547C File Offset: 0x0004367C
		public ulong StorageIo
		{
			get
			{
				return this.m_StorageIo;
			}
			set
			{
				this.m_StorageIo = value;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x00045488 File Offset: 0x00043688
		// (set) Token: 0x06002ECF RID: 11983 RVA: 0x000454A0 File Offset: 0x000436A0
		public ulong WebSocketIo
		{
			get
			{
				return this.m_WebSocketIo;
			}
			set
			{
				this.m_WebSocketIo = value;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x000454AC File Offset: 0x000436AC
		// (set) Token: 0x06002ED1 RID: 11985 RVA: 0x000454C4 File Offset: 0x000436C4
		public ulong P2PIo
		{
			get
			{
				return this.m_P2PIo;
			}
			set
			{
				this.m_P2PIo = value;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000454D0 File Offset: 0x000436D0
		// (set) Token: 0x06002ED3 RID: 11987 RVA: 0x000454E8 File Offset: 0x000436E8
		public ulong HttpRequestIo
		{
			get
			{
				return this.m_HttpRequestIo;
			}
			set
			{
				this.m_HttpRequestIo = value;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x000454F4 File Offset: 0x000436F4
		// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x0004550C File Offset: 0x0004370C
		public ulong RTCIo
		{
			get
			{
				return this.m_RTCIo;
			}
			set
			{
				this.m_RTCIo = value;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x00045518 File Offset: 0x00043718
		// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x00045530 File Offset: 0x00043730
		public ulong EmbeddedOverlayMainThread
		{
			get
			{
				return this.m_EmbeddedOverlayMainThread;
			}
			set
			{
				this.m_EmbeddedOverlayMainThread = value;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x0004553C File Offset: 0x0004373C
		// (set) Token: 0x06002ED9 RID: 11993 RVA: 0x00045554 File Offset: 0x00043754
		public ulong EmbeddedOverlayWorkerThreads
		{
			get
			{
				return this.m_EmbeddedOverlayWorkerThreads;
			}
			set
			{
				this.m_EmbeddedOverlayWorkerThreads = value;
			}
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x00045560 File Offset: 0x00043760
		public void Set(ref InitializeThreadAffinity other)
		{
			this.m_ApiVersion = 3;
			this.NetworkWork = other.NetworkWork;
			this.StorageIo = other.StorageIo;
			this.WebSocketIo = other.WebSocketIo;
			this.P2PIo = other.P2PIo;
			this.HttpRequestIo = other.HttpRequestIo;
			this.RTCIo = other.RTCIo;
			this.EmbeddedOverlayMainThread = other.EmbeddedOverlayMainThread;
			this.EmbeddedOverlayWorkerThreads = other.EmbeddedOverlayWorkerThreads;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000455E0 File Offset: 0x000437E0
		public void Set(ref InitializeThreadAffinity? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.NetworkWork = other.Value.NetworkWork;
				this.StorageIo = other.Value.StorageIo;
				this.WebSocketIo = other.Value.WebSocketIo;
				this.P2PIo = other.Value.P2PIo;
				this.HttpRequestIo = other.Value.HttpRequestIo;
				this.RTCIo = other.Value.RTCIo;
				this.EmbeddedOverlayMainThread = other.Value.EmbeddedOverlayMainThread;
				this.EmbeddedOverlayWorkerThreads = other.Value.EmbeddedOverlayWorkerThreads;
			}
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000456AC File Offset: 0x000438AC
		public void Dispose()
		{
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000456AF File Offset: 0x000438AF
		public void Get(out InitializeThreadAffinity output)
		{
			output = default(InitializeThreadAffinity);
			output.Set(ref this);
		}

		// Token: 0x040014BD RID: 5309
		private int m_ApiVersion;

		// Token: 0x040014BE RID: 5310
		private ulong m_NetworkWork;

		// Token: 0x040014BF RID: 5311
		private ulong m_StorageIo;

		// Token: 0x040014C0 RID: 5312
		private ulong m_WebSocketIo;

		// Token: 0x040014C1 RID: 5313
		private ulong m_P2PIo;

		// Token: 0x040014C2 RID: 5314
		private ulong m_HttpRequestIo;

		// Token: 0x040014C3 RID: 5315
		private ulong m_RTCIo;

		// Token: 0x040014C4 RID: 5316
		private ulong m_EmbeddedOverlayMainThread;

		// Token: 0x040014C5 RID: 5317
		private ulong m_EmbeddedOverlayWorkerThreads;
	}
}
