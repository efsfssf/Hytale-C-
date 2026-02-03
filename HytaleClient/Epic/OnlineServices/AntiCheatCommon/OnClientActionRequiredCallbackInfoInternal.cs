using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006BE RID: 1726
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnClientActionRequiredCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientActionRequiredCallbackInfo>, ISettable<OnClientActionRequiredCallbackInfo>, IDisposable
	{
		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06002CBF RID: 11455 RVA: 0x0004217C File Offset: 0x0004037C
		// (set) Token: 0x06002CC0 RID: 11456 RVA: 0x0004219D File Offset: 0x0004039D
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

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06002CC1 RID: 11457 RVA: 0x000421B0 File Offset: 0x000403B0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x000421C8 File Offset: 0x000403C8
		// (set) Token: 0x06002CC3 RID: 11459 RVA: 0x000421E0 File Offset: 0x000403E0
		public IntPtr ClientHandle
		{
			get
			{
				return this.m_ClientHandle;
			}
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000421EC File Offset: 0x000403EC
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x00042204 File Offset: 0x00040404
		public AntiCheatCommonClientAction ClientAction
		{
			get
			{
				return this.m_ClientAction;
			}
			set
			{
				this.m_ClientAction = value;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x00042210 File Offset: 0x00040410
		// (set) Token: 0x06002CC7 RID: 11463 RVA: 0x00042228 File Offset: 0x00040428
		public AntiCheatCommonClientActionReason ActionReasonCode
		{
			get
			{
				return this.m_ActionReasonCode;
			}
			set
			{
				this.m_ActionReasonCode = value;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x00042234 File Offset: 0x00040434
		// (set) Token: 0x06002CC9 RID: 11465 RVA: 0x00042255 File Offset: 0x00040455
		public Utf8String ActionReasonDetailsString
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ActionReasonDetailsString, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ActionReasonDetailsString);
			}
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x00042268 File Offset: 0x00040468
		public void Set(ref OnClientActionRequiredCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.ClientHandle = other.ClientHandle;
			this.ClientAction = other.ClientAction;
			this.ActionReasonCode = other.ActionReasonCode;
			this.ActionReasonDetailsString = other.ActionReasonDetailsString;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000422B8 File Offset: 0x000404B8
		public void Set(ref OnClientActionRequiredCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.ClientHandle = other.Value.ClientHandle;
				this.ClientAction = other.Value.ClientAction;
				this.ActionReasonCode = other.Value.ActionReasonCode;
				this.ActionReasonDetailsString = other.Value.ActionReasonDetailsString;
			}
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x0004233B File Offset: 0x0004053B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_ActionReasonDetailsString);
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x00042362 File Offset: 0x00040562
		public void Get(out OnClientActionRequiredCallbackInfo output)
		{
			output = default(OnClientActionRequiredCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040013B5 RID: 5045
		private IntPtr m_ClientData;

		// Token: 0x040013B6 RID: 5046
		private IntPtr m_ClientHandle;

		// Token: 0x040013B7 RID: 5047
		private AntiCheatCommonClientAction m_ClientAction;

		// Token: 0x040013B8 RID: 5048
		private AntiCheatCommonClientActionReason m_ActionReasonCode;

		// Token: 0x040013B9 RID: 5049
		private IntPtr m_ActionReasonDetailsString;
	}
}
