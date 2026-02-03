using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200034A RID: 842
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateModCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateModCallbackInfo>, ISettable<UpdateModCallbackInfo>, IDisposable
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x00021964 File Offset: 0x0001FB64
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x0002197C File Offset: 0x0001FB7C
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

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x00021988 File Offset: 0x0001FB88
		// (set) Token: 0x0600170F RID: 5903 RVA: 0x000219A9 File Offset: 0x0001FBA9
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

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001710 RID: 5904 RVA: 0x000219BC File Offset: 0x0001FBBC
		// (set) Token: 0x06001711 RID: 5905 RVA: 0x000219DD File Offset: 0x0001FBDD
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

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x000219F0 File Offset: 0x0001FBF0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00021A08 File Offset: 0x0001FC08
		// (set) Token: 0x06001714 RID: 5908 RVA: 0x00021A29 File Offset: 0x0001FC29
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

		// Token: 0x06001715 RID: 5909 RVA: 0x00021A3A File Offset: 0x0001FC3A
		public void Set(ref UpdateModCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Mod = other.Mod;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00021A74 File Offset: 0x0001FC74
		public void Set(ref UpdateModCallbackInfo? other)
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

		// Token: 0x06001717 RID: 5911 RVA: 0x00021AE2 File Offset: 0x0001FCE2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_Mod);
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x00021B09 File Offset: 0x0001FD09
		public void Get(out UpdateModCallbackInfo output)
		{
			output = default(UpdateModCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000A05 RID: 2565
		private Result m_ResultCode;

		// Token: 0x04000A06 RID: 2566
		private IntPtr m_LocalUserId;

		// Token: 0x04000A07 RID: 2567
		private IntPtr m_ClientData;

		// Token: 0x04000A08 RID: 2568
		private IntPtr m_Mod;
	}
}
