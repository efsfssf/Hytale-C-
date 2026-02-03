using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x020007AE RID: 1966
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SocketIdInternal : IGettable<SocketId>, ISettable<SocketId>, IDisposable
	{
		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x060032EE RID: 13038 RVA: 0x0004BEB8 File Offset: 0x0004A0B8
		// (set) Token: 0x060032EF RID: 13039 RVA: 0x0004BED9 File Offset: 0x0004A0D9
		public string SocketName
		{
			get
			{
				string result;
				Helper.Get(this.m_SocketName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SocketName, 33);
			}
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x0004BEEB File Offset: 0x0004A0EB
		public void Set(ref SocketId other)
		{
			this.m_ApiVersion = 1;
			this.SocketName = other.SocketName;
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x0004BF04 File Offset: 0x0004A104
		public void Set(ref SocketId? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SocketName = other.Value.SocketName;
			}
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0004BF3A File Offset: 0x0004A13A
		public void Dispose()
		{
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x0004BF3D File Offset: 0x0004A13D
		public void Get(out SocketId output)
		{
			output = default(SocketId);
			output.Set(ref this);
		}

		// Token: 0x040016DA RID: 5850
		private int m_ApiVersion;

		// Token: 0x040016DB RID: 5851
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
		private byte[] m_SocketName;
	}
}
