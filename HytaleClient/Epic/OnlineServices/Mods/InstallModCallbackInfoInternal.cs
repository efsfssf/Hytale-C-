using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000334 RID: 820
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct InstallModCallbackInfoInternal : ICallbackInfoInternal, IGettable<InstallModCallbackInfo>, ISettable<InstallModCallbackInfo>, IDisposable
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00020C4C File Offset: 0x0001EE4C
		// (set) Token: 0x06001679 RID: 5753 RVA: 0x00020C64 File Offset: 0x0001EE64
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x00020C70 File Offset: 0x0001EE70
		// (set) Token: 0x0600167B RID: 5755 RVA: 0x00020C91 File Offset: 0x0001EE91
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x00020CA4 File Offset: 0x0001EEA4
		// (set) Token: 0x0600167D RID: 5757 RVA: 0x00020CC5 File Offset: 0x0001EEC5
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x00020CD8 File Offset: 0x0001EED8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x00020CF0 File Offset: 0x0001EEF0
		// (set) Token: 0x06001680 RID: 5760 RVA: 0x00020D11 File Offset: 0x0001EF11
		public ModIdentifier? Mod
		{
			get
			{
				ModIdentifier? result;
				Helper.Get<ModIdentifierInternal, ModIdentifier>(this.m_Mod, out result);
				return result;
			}
			set
			{
				Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref this.m_Mod);
			}
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00020D22 File Offset: 0x0001EF22
		public void Set(ref InstallModCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Mod = other.Mod;
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00020D5C File Offset: 0x0001EF5C
		public void Set(ref InstallModCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.LocalUserId = other.Value.LocalUserId;
				this.ClientData = other.Value.ClientData;
				this.Mod = other.Value.Mod;
			}
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00020DCA File Offset: 0x0001EFCA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_Mod);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00020DF1 File Offset: 0x0001EFF1
		public void Get(out InstallModCallbackInfo output)
		{
			output = default(InstallModCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040009CE RID: 2510
		private Result m_ResultCode;

		// Token: 0x040009CF RID: 2511
		private IntPtr m_LocalUserId;

		// Token: 0x040009D0 RID: 2512
		private IntPtr m_ClientData;

		// Token: 0x040009D1 RID: 2513
		private IntPtr m_Mod;
	}
}
