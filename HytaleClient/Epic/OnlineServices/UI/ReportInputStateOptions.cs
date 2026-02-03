using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000080 RID: 128
	public struct ReportInputStateOptions
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0000792A File Offset: 0x00005B2A
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00007932 File Offset: 0x00005B32
		public InputStateButtonFlags ButtonDownFlags { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0000793B File Offset: 0x00005B3B
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00007943 File Offset: 0x00005B43
		public bool AcceptIsFaceButtonRight { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0000794C File Offset: 0x00005B4C
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x00007954 File Offset: 0x00005B54
		public bool MouseButtonDown { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0000795D File Offset: 0x00005B5D
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00007965 File Offset: 0x00005B65
		public uint MousePosX { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0000796E File Offset: 0x00005B6E
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x00007976 File Offset: 0x00005B76
		public uint MousePosY { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0000797F File Offset: 0x00005B7F
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00007987 File Offset: 0x00005B87
		public uint GamepadIndex { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00007990 File Offset: 0x00005B90
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00007998 File Offset: 0x00005B98
		public float LeftStickX { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x000079A1 File Offset: 0x00005BA1
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x000079A9 File Offset: 0x00005BA9
		public float LeftStickY { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x000079B2 File Offset: 0x00005BB2
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x000079BA File Offset: 0x00005BBA
		public float RightStickX { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x000079C3 File Offset: 0x00005BC3
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x000079CB File Offset: 0x00005BCB
		public float RightStickY { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x000079D4 File Offset: 0x00005BD4
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x000079DC File Offset: 0x00005BDC
		public float LeftTrigger { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x000079E5 File Offset: 0x00005BE5
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x000079ED File Offset: 0x00005BED
		public float RightTrigger { get; set; }
	}
}
