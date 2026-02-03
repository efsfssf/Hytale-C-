using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000346 RID: 838
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UninstallModCallbackInfoInternal : ICallbackInfoInternal, IGettable<UninstallModCallbackInfo>, ISettable<UninstallModCallbackInfo>, IDisposable
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060016EC RID: 5868 RVA: 0x00021644 File Offset: 0x0001F844
		// (set) Token: 0x060016ED RID: 5869 RVA: 0x0002165C File Offset: 0x0001F85C
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

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x00021668 File Offset: 0x0001F868
		// (set) Token: 0x060016EF RID: 5871 RVA: 0x00021689 File Offset: 0x0001F889
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

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x0002169C File Offset: 0x0001F89C
		// (set) Token: 0x060016F1 RID: 5873 RVA: 0x000216BD File Offset: 0x0001F8BD
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

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x000216D0 File Offset: 0x0001F8D0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x000216E8 File Offset: 0x0001F8E8
		// (set) Token: 0x060016F4 RID: 5876 RVA: 0x00021709 File Offset: 0x0001F909
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

		// Token: 0x060016F5 RID: 5877 RVA: 0x0002171A File Offset: 0x0001F91A
		public void Set(ref UninstallModCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Mod = other.Mod;
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00021754 File Offset: 0x0001F954
		public void Set(ref UninstallModCallbackInfo? other)
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

		// Token: 0x060016F7 RID: 5879 RVA: 0x000217C2 File Offset: 0x0001F9C2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_Mod);
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x000217E9 File Offset: 0x0001F9E9
		public void Get(out UninstallModCallbackInfo output)
		{
			output = default(UninstallModCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040009F8 RID: 2552
		private Result m_ResultCode;

		// Token: 0x040009F9 RID: 2553
		private IntPtr m_LocalUserId;

		// Token: 0x040009FA RID: 2554
		private IntPtr m_ClientData;

		// Token: 0x040009FB RID: 2555
		private IntPtr m_Mod;
	}
}
