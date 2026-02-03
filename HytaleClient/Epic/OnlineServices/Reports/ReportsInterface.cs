using System;

namespace Epic.OnlineServices.Reports
{
	// Token: 0x020002A0 RID: 672
	public sealed class ReportsInterface : Handle
	{
		// Token: 0x060012C9 RID: 4809 RVA: 0x0001B5E8 File Offset: 0x000197E8
		public ReportsInterface()
		{
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0001B5F2 File Offset: 0x000197F2
		public ReportsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0001B600 File Offset: 0x00019800
		public void SendPlayerBehaviorReport(ref SendPlayerBehaviorReportOptions options, object clientData, OnSendPlayerBehaviorReportCompleteCallback completionDelegate)
		{
			SendPlayerBehaviorReportOptionsInternal sendPlayerBehaviorReportOptionsInternal = default(SendPlayerBehaviorReportOptionsInternal);
			sendPlayerBehaviorReportOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnSendPlayerBehaviorReportCompleteCallbackInternal onSendPlayerBehaviorReportCompleteCallbackInternal = new OnSendPlayerBehaviorReportCompleteCallbackInternal(ReportsInterface.OnSendPlayerBehaviorReportCompleteCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onSendPlayerBehaviorReportCompleteCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Reports_SendPlayerBehaviorReport(base.InnerHandle, ref sendPlayerBehaviorReportOptionsInternal, zero, onSendPlayerBehaviorReportCompleteCallbackInternal);
			Helper.Dispose<SendPlayerBehaviorReportOptionsInternal>(ref sendPlayerBehaviorReportOptionsInternal);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0001B65C File Offset: 0x0001985C
		[MonoPInvokeCallback(typeof(OnSendPlayerBehaviorReportCompleteCallbackInternal))]
		internal static void OnSendPlayerBehaviorReportCompleteCallbackInternalImplementation(ref SendPlayerBehaviorReportCompleteCallbackInfoInternal data)
		{
			OnSendPlayerBehaviorReportCompleteCallback onSendPlayerBehaviorReportCompleteCallback;
			SendPlayerBehaviorReportCompleteCallbackInfo sendPlayerBehaviorReportCompleteCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<SendPlayerBehaviorReportCompleteCallbackInfoInternal, OnSendPlayerBehaviorReportCompleteCallback, SendPlayerBehaviorReportCompleteCallbackInfo>(ref data, out onSendPlayerBehaviorReportCompleteCallback, out sendPlayerBehaviorReportCompleteCallbackInfo);
			if (flag)
			{
				onSendPlayerBehaviorReportCompleteCallback(ref sendPlayerBehaviorReportCompleteCallbackInfo);
			}
		}

		// Token: 0x04000840 RID: 2112
		public const int ReportcontextMaxLength = 4096;

		// Token: 0x04000841 RID: 2113
		public const int ReportmessageMaxLength = 512;

		// Token: 0x04000842 RID: 2114
		public const int SendplayerbehaviorreportApiLatest = 2;
	}
}
