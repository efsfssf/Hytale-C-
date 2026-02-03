using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000081 RID: 129
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReportInputStateOptionsInternal : ISettable<ReportInputStateOptions>, IDisposable
	{
		// Token: 0x170000CA RID: 202
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x000079F6 File Offset: 0x00005BF6
		public InputStateButtonFlags ButtonDownFlags
		{
			set
			{
				this.m_ButtonDownFlags = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x00007A00 File Offset: 0x00005C00
		public bool AcceptIsFaceButtonRight
		{
			set
			{
				Helper.Set(value, ref this.m_AcceptIsFaceButtonRight);
			}
		}

		// Token: 0x170000CC RID: 204
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00007A10 File Offset: 0x00005C10
		public bool MouseButtonDown
		{
			set
			{
				Helper.Set(value, ref this.m_MouseButtonDown);
			}
		}

		// Token: 0x170000CD RID: 205
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00007A20 File Offset: 0x00005C20
		public uint MousePosX
		{
			set
			{
				this.m_MousePosX = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00007A2A File Offset: 0x00005C2A
		public uint MousePosY
		{
			set
			{
				this.m_MousePosY = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x00007A34 File Offset: 0x00005C34
		public uint GamepadIndex
		{
			set
			{
				this.m_GamepadIndex = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00007A3E File Offset: 0x00005C3E
		public float LeftStickX
		{
			set
			{
				this.m_LeftStickX = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x00007A48 File Offset: 0x00005C48
		public float LeftStickY
		{
			set
			{
				this.m_LeftStickY = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00007A52 File Offset: 0x00005C52
		public float RightStickX
		{
			set
			{
				this.m_RightStickX = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00007A5C File Offset: 0x00005C5C
		public float RightStickY
		{
			set
			{
				this.m_RightStickY = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00007A66 File Offset: 0x00005C66
		public float LeftTrigger
		{
			set
			{
				this.m_LeftTrigger = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00007A70 File Offset: 0x00005C70
		public float RightTrigger
		{
			set
			{
				this.m_RightTrigger = value;
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00007A7C File Offset: 0x00005C7C
		public void Set(ref ReportInputStateOptions other)
		{
			this.m_ApiVersion = 2;
			this.ButtonDownFlags = other.ButtonDownFlags;
			this.AcceptIsFaceButtonRight = other.AcceptIsFaceButtonRight;
			this.MouseButtonDown = other.MouseButtonDown;
			this.MousePosX = other.MousePosX;
			this.MousePosY = other.MousePosY;
			this.GamepadIndex = other.GamepadIndex;
			this.LeftStickX = other.LeftStickX;
			this.LeftStickY = other.LeftStickY;
			this.RightStickX = other.RightStickX;
			this.RightStickY = other.RightStickY;
			this.LeftTrigger = other.LeftTrigger;
			this.RightTrigger = other.RightTrigger;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00007B30 File Offset: 0x00005D30
		public void Set(ref ReportInputStateOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.ButtonDownFlags = other.Value.ButtonDownFlags;
				this.AcceptIsFaceButtonRight = other.Value.AcceptIsFaceButtonRight;
				this.MouseButtonDown = other.Value.MouseButtonDown;
				this.MousePosX = other.Value.MousePosX;
				this.MousePosY = other.Value.MousePosY;
				this.GamepadIndex = other.Value.GamepadIndex;
				this.LeftStickX = other.Value.LeftStickX;
				this.LeftStickY = other.Value.LeftStickY;
				this.RightStickX = other.Value.RightStickX;
				this.RightStickY = other.Value.RightStickY;
				this.LeftTrigger = other.Value.LeftTrigger;
				this.RightTrigger = other.Value.RightTrigger;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00007C50 File Offset: 0x00005E50
		public void Dispose()
		{
		}

		// Token: 0x040002AC RID: 684
		private int m_ApiVersion;

		// Token: 0x040002AD RID: 685
		private InputStateButtonFlags m_ButtonDownFlags;

		// Token: 0x040002AE RID: 686
		private int m_AcceptIsFaceButtonRight;

		// Token: 0x040002AF RID: 687
		private int m_MouseButtonDown;

		// Token: 0x040002B0 RID: 688
		private uint m_MousePosX;

		// Token: 0x040002B1 RID: 689
		private uint m_MousePosY;

		// Token: 0x040002B2 RID: 690
		private uint m_GamepadIndex;

		// Token: 0x040002B3 RID: 691
		private float m_LeftStickX;

		// Token: 0x040002B4 RID: 692
		private float m_LeftStickY;

		// Token: 0x040002B5 RID: 693
		private float m_RightStickX;

		// Token: 0x040002B6 RID: 694
		private float m_RightStickY;

		// Token: 0x040002B7 RID: 695
		private float m_LeftTrigger;

		// Token: 0x040002B8 RID: 696
		private float m_RightTrigger;
	}
}
