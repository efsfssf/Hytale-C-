using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006BA RID: 1722
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogPlayerUseWeaponDataInternal : IGettable<LogPlayerUseWeaponData>, ISettable<LogPlayerUseWeaponData>, IDisposable
	{
		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06002C9D RID: 11421 RVA: 0x00041DD4 File Offset: 0x0003FFD4
		// (set) Token: 0x06002C9E RID: 11422 RVA: 0x00041DEC File Offset: 0x0003FFEC
		public IntPtr PlayerHandle
		{
			get
			{
				return this.m_PlayerHandle;
			}
			set
			{
				this.m_PlayerHandle = value;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x00041DF8 File Offset: 0x0003FFF8
		// (set) Token: 0x06002CA0 RID: 11424 RVA: 0x00041E19 File Offset: 0x00040019
		public Vec3f? PlayerPosition
		{
			get
			{
				Vec3f? result;
				Helper.Get<Vec3fInternal, Vec3f>(this.m_PlayerPosition, out result);
				return result;
			}
			set
			{
				Helper.Set<Vec3f, Vec3fInternal>(ref value, ref this.m_PlayerPosition);
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x00041E2C File Offset: 0x0004002C
		// (set) Token: 0x06002CA2 RID: 11426 RVA: 0x00041E4D File Offset: 0x0004004D
		public Quat? PlayerViewRotation
		{
			get
			{
				Quat? result;
				Helper.Get<QuatInternal, Quat>(this.m_PlayerViewRotation, out result);
				return result;
			}
			set
			{
				Helper.Set<Quat, QuatInternal>(ref value, ref this.m_PlayerViewRotation);
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x00041E60 File Offset: 0x00040060
		// (set) Token: 0x06002CA4 RID: 11428 RVA: 0x00041E81 File Offset: 0x00040081
		public bool IsPlayerViewZoomed
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsPlayerViewZoomed, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsPlayerViewZoomed);
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x00041E94 File Offset: 0x00040094
		// (set) Token: 0x06002CA6 RID: 11430 RVA: 0x00041EB5 File Offset: 0x000400B5
		public bool IsMeleeAttack
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsMeleeAttack, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsMeleeAttack);
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x00041EC8 File Offset: 0x000400C8
		// (set) Token: 0x06002CA8 RID: 11432 RVA: 0x00041EE9 File Offset: 0x000400E9
		public Utf8String WeaponName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_WeaponName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_WeaponName);
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x00041EFC File Offset: 0x000400FC
		public void Set(ref LogPlayerUseWeaponData other)
		{
			this.PlayerHandle = other.PlayerHandle;
			this.PlayerPosition = other.PlayerPosition;
			this.PlayerViewRotation = other.PlayerViewRotation;
			this.IsPlayerViewZoomed = other.IsPlayerViewZoomed;
			this.IsMeleeAttack = other.IsMeleeAttack;
			this.WeaponName = other.WeaponName;
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x00041F58 File Offset: 0x00040158
		public void Set(ref LogPlayerUseWeaponData? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.PlayerHandle = other.Value.PlayerHandle;
				this.PlayerPosition = other.Value.PlayerPosition;
				this.PlayerViewRotation = other.Value.PlayerViewRotation;
				this.IsPlayerViewZoomed = other.Value.IsPlayerViewZoomed;
				this.IsMeleeAttack = other.Value.IsMeleeAttack;
				this.WeaponName = other.Value.WeaponName;
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x00041FF3 File Offset: 0x000401F3
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PlayerHandle);
			Helper.Dispose(ref this.m_PlayerPosition);
			Helper.Dispose(ref this.m_PlayerViewRotation);
			Helper.Dispose(ref this.m_WeaponName);
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x00042026 File Offset: 0x00040226
		public void Get(out LogPlayerUseWeaponData output)
		{
			output = default(LogPlayerUseWeaponData);
			output.Set(ref this);
		}

		// Token: 0x040013A7 RID: 5031
		private IntPtr m_PlayerHandle;

		// Token: 0x040013A8 RID: 5032
		private IntPtr m_PlayerPosition;

		// Token: 0x040013A9 RID: 5033
		private IntPtr m_PlayerViewRotation;

		// Token: 0x040013AA RID: 5034
		private int m_IsPlayerViewZoomed;

		// Token: 0x040013AB RID: 5035
		private int m_IsMeleeAttack;

		// Token: 0x040013AC RID: 5036
		private IntPtr m_WeaponName;
	}
}
