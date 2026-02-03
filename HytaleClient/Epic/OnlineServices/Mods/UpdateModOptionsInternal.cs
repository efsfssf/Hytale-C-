using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200034C RID: 844
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateModOptionsInternal : ISettable<UpdateModOptions>, IDisposable
	{
		// Token: 0x17000655 RID: 1621
		// (set) Token: 0x0600171D RID: 5917 RVA: 0x00021B3D File Offset: 0x0001FD3D
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000656 RID: 1622
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x00021B4D File Offset: 0x0001FD4D
		public ModIdentifier? Mod
		{
			set
			{
				Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref this.m_Mod);
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x00021B5E File Offset: 0x0001FD5E
		public void Set(ref UpdateModOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Mod = other.Mod;
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00021B84 File Offset: 0x0001FD84
		public void Set(ref UpdateModOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Mod = other.Value.Mod;
			}
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00021BCF File Offset: 0x0001FDCF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Mod);
		}

		// Token: 0x04000A0B RID: 2571
		private int m_ApiVersion;

		// Token: 0x04000A0C RID: 2572
		private IntPtr m_LocalUserId;

		// Token: 0x04000A0D RID: 2573
		private IntPtr m_Mod;
	}
}
