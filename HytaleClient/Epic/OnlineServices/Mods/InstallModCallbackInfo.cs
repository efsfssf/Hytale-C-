using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000333 RID: 819
	public struct InstallModCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00020BB2 File Offset: 0x0001EDB2
		// (set) Token: 0x0600166F RID: 5743 RVA: 0x00020BBA File Offset: 0x0001EDBA
		public Result ResultCode { get; set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00020BC3 File Offset: 0x0001EDC3
		// (set) Token: 0x06001671 RID: 5745 RVA: 0x00020BCB File Offset: 0x0001EDCB
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x00020BD4 File Offset: 0x0001EDD4
		// (set) Token: 0x06001673 RID: 5747 RVA: 0x00020BDC File Offset: 0x0001EDDC
		public object ClientData { get; set; }

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x00020BE5 File Offset: 0x0001EDE5
		// (set) Token: 0x06001675 RID: 5749 RVA: 0x00020BED File Offset: 0x0001EDED
		public ModIdentifier? Mod { get; set; }

		// Token: 0x06001676 RID: 5750 RVA: 0x00020BF8 File Offset: 0x0001EDF8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00020C15 File Offset: 0x0001EE15
		internal void Set(ref InstallModCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Mod = other.Mod;
		}
	}
}
