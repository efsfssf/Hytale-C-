using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000626 RID: 1574
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct VerifyIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyIdTokenCallbackInfo>, ISettable<VerifyIdTokenCallbackInfo>, IDisposable
	{
		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x0003BB34 File Offset: 0x00039D34
		// (set) Token: 0x060028C3 RID: 10435 RVA: 0x0003BB4C File Offset: 0x00039D4C
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

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x0003BB58 File Offset: 0x00039D58
		// (set) Token: 0x060028C5 RID: 10437 RVA: 0x0003BB79 File Offset: 0x00039D79
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

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x0003BB8C File Offset: 0x00039D8C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060028C7 RID: 10439 RVA: 0x0003BBA4 File Offset: 0x00039DA4
		// (set) Token: 0x060028C8 RID: 10440 RVA: 0x0003BBC5 File Offset: 0x00039DC5
		public ProductUserId ProductUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ProductUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductUserId);
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x0003BBD8 File Offset: 0x00039DD8
		// (set) Token: 0x060028CA RID: 10442 RVA: 0x0003BBF9 File Offset: 0x00039DF9
		public bool IsAccountInfoPresent
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsAccountInfoPresent, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsAccountInfoPresent);
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x0003BC0C File Offset: 0x00039E0C
		// (set) Token: 0x060028CC RID: 10444 RVA: 0x0003BC24 File Offset: 0x00039E24
		public ExternalAccountType AccountIdType
		{
			get
			{
				return this.m_AccountIdType;
			}
			set
			{
				this.m_AccountIdType = value;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x0003BC30 File Offset: 0x00039E30
		// (set) Token: 0x060028CE RID: 10446 RVA: 0x0003BC51 File Offset: 0x00039E51
		public Utf8String AccountId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_AccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AccountId);
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x0003BC64 File Offset: 0x00039E64
		// (set) Token: 0x060028D0 RID: 10448 RVA: 0x0003BC85 File Offset: 0x00039E85
		public Utf8String Platform
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Platform, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Platform);
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x0003BC98 File Offset: 0x00039E98
		// (set) Token: 0x060028D2 RID: 10450 RVA: 0x0003BCB9 File Offset: 0x00039EB9
		public Utf8String DeviceType
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DeviceType, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DeviceType);
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x0003BCCC File Offset: 0x00039ECC
		// (set) Token: 0x060028D4 RID: 10452 RVA: 0x0003BCED File Offset: 0x00039EED
		public Utf8String ClientId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ClientId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientId);
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x0003BD00 File Offset: 0x00039F00
		// (set) Token: 0x060028D6 RID: 10454 RVA: 0x0003BD21 File Offset: 0x00039F21
		public Utf8String ProductId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ProductId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductId);
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x0003BD34 File Offset: 0x00039F34
		// (set) Token: 0x060028D8 RID: 10456 RVA: 0x0003BD55 File Offset: 0x00039F55
		public Utf8String SandboxId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SandboxId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SandboxId);
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x0003BD68 File Offset: 0x00039F68
		// (set) Token: 0x060028DA RID: 10458 RVA: 0x0003BD89 File Offset: 0x00039F89
		public Utf8String DeploymentId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DeploymentId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DeploymentId);
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x0003BD9C File Offset: 0x00039F9C
		public void Set(ref VerifyIdTokenCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.ProductUserId = other.ProductUserId;
			this.IsAccountInfoPresent = other.IsAccountInfoPresent;
			this.AccountIdType = other.AccountIdType;
			this.AccountId = other.AccountId;
			this.Platform = other.Platform;
			this.DeviceType = other.DeviceType;
			this.ClientId = other.ClientId;
			this.ProductId = other.ProductId;
			this.SandboxId = other.SandboxId;
			this.DeploymentId = other.DeploymentId;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x0003BE48 File Offset: 0x0003A048
		public void Set(ref VerifyIdTokenCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.ProductUserId = other.Value.ProductUserId;
				this.IsAccountInfoPresent = other.Value.IsAccountInfoPresent;
				this.AccountIdType = other.Value.AccountIdType;
				this.AccountId = other.Value.AccountId;
				this.Platform = other.Value.Platform;
				this.DeviceType = other.Value.DeviceType;
				this.ClientId = other.Value.ClientId;
				this.ProductId = other.Value.ProductId;
				this.SandboxId = other.Value.SandboxId;
				this.DeploymentId = other.Value.DeploymentId;
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x0003BF64 File Offset: 0x0003A164
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ProductUserId);
			Helper.Dispose(ref this.m_AccountId);
			Helper.Dispose(ref this.m_Platform);
			Helper.Dispose(ref this.m_DeviceType);
			Helper.Dispose(ref this.m_ClientId);
			Helper.Dispose(ref this.m_ProductId);
			Helper.Dispose(ref this.m_SandboxId);
			Helper.Dispose(ref this.m_DeploymentId);
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0003BFDE File Offset: 0x0003A1DE
		public void Get(out VerifyIdTokenCallbackInfo output)
		{
			output = default(VerifyIdTokenCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400117B RID: 4475
		private Result m_ResultCode;

		// Token: 0x0400117C RID: 4476
		private IntPtr m_ClientData;

		// Token: 0x0400117D RID: 4477
		private IntPtr m_ProductUserId;

		// Token: 0x0400117E RID: 4478
		private int m_IsAccountInfoPresent;

		// Token: 0x0400117F RID: 4479
		private ExternalAccountType m_AccountIdType;

		// Token: 0x04001180 RID: 4480
		private IntPtr m_AccountId;

		// Token: 0x04001181 RID: 4481
		private IntPtr m_Platform;

		// Token: 0x04001182 RID: 4482
		private IntPtr m_DeviceType;

		// Token: 0x04001183 RID: 4483
		private IntPtr m_ClientId;

		// Token: 0x04001184 RID: 4484
		private IntPtr m_ProductId;

		// Token: 0x04001185 RID: 4485
		private IntPtr m_SandboxId;

		// Token: 0x04001186 RID: 4486
		private IntPtr m_DeploymentId;
	}
}
