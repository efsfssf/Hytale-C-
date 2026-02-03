using System;

namespace Epic.OnlineServices.Logging
{
	// Token: 0x02000359 RID: 857
	public sealed class LoggingInterface
	{
		// Token: 0x06001762 RID: 5986 RVA: 0x00022378 File Offset: 0x00020578
		public static Result SetCallback(LogMessageFunc callback)
		{
			LogMessageFuncInternal logMessageFuncInternal = new LogMessageFuncInternal(LoggingInterface.LogMessageFuncInternalImplementation);
			Helper.AddStaticCallback("LogMessageFuncInternalImplementation", callback, logMessageFuncInternal);
			return Bindings.EOS_Logging_SetCallback(logMessageFuncInternal);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000223AC File Offset: 0x000205AC
		public static Result SetLogLevel(LogCategory logCategory, LogLevel logLevel)
		{
			return Bindings.EOS_Logging_SetLogLevel(logCategory, logLevel);
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x000223C8 File Offset: 0x000205C8
		[MonoPInvokeCallback(typeof(LogMessageFuncInternal))]
		internal static void LogMessageFuncInternalImplementation(ref LogMessageInternal message)
		{
			LogMessageFunc logMessageFunc;
			bool flag = Helper.TryGetStaticCallback<LogMessageFunc>("LogMessageFuncInternalImplementation", out logMessageFunc);
			if (flag)
			{
				LogMessage logMessage;
				Helper.Get<LogMessageInternal, LogMessage>(ref message, out logMessage);
				logMessageFunc(ref logMessage);
			}
		}
	}
}
