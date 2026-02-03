using System;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF6 RID: 3062
	public class EOSPlatformManager : IDisposable
	{
		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x06006161 RID: 24929 RVA: 0x002002C9 File Offset: 0x001FE4C9
		public PlatformInterface Platform
		{
			get
			{
				return this._platformInterface;
			}
		}

		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x06006162 RID: 24930 RVA: 0x002002D1 File Offset: 0x001FE4D1
		public bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		// Token: 0x06006163 RID: 24931 RVA: 0x002002D9 File Offset: 0x001FE4D9
		public EOSPlatformManager()
		{
			LoggingInterface.SetLogLevel(LogCategory.AllCategories, Epic.OnlineServices.Logging.LogLevel.Verbose);
			LoggingInterface.SetCallback(delegate(ref LogMessage message)
			{
				Epic.OnlineServices.Logging.LogLevel level = message.Level;
				Epic.OnlineServices.Logging.LogLevel logLevel = level;
				if (logLevel <= Epic.OnlineServices.Logging.LogLevel.Warning)
				{
					if (logLevel != Epic.OnlineServices.Logging.LogLevel.Error)
					{
						if (logLevel == Epic.OnlineServices.Logging.LogLevel.Warning)
						{
							EOSPlatformManager.Logger.Warn(string.Format("[EOS] {0}", message.Message));
						}
					}
					else
					{
						EOSPlatformManager.Logger.Error(string.Format("[EOS] {0}", message.Message));
					}
				}
				else if (logLevel != Epic.OnlineServices.Logging.LogLevel.Info)
				{
					if (logLevel == Epic.OnlineServices.Logging.LogLevel.Verbose || logLevel == Epic.OnlineServices.Logging.LogLevel.VeryVerbose)
					{
						EOSPlatformManager.Logger.Debug(string.Format("[EOS] {0}", message.Message));
					}
				}
				else
				{
					EOSPlatformManager.Logger.Info(string.Format("[EOS] {0}", message.Message));
				}
			});
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x00200318 File Offset: 0x001FE518
		public Result Initialize()
		{
			bool isInitialized = this._isInitialized;
			Result result;
			if (isInitialized)
			{
				EOSPlatformManager.Logger.Warn("EOS Platform is already initialized");
				result = Result.Success;
			}
			else
			{
				try
				{
					InitializeOptions initializeOptions = new InitializeOptions
					{
						ProductName = "testing",
						ProductVersion = "1.0.0",
						Reserved = IntPtr.Zero,
						AllocateMemoryFunction = IntPtr.Zero,
						ReallocateMemoryFunction = IntPtr.Zero,
						ReleaseMemoryFunction = IntPtr.Zero,
						SystemInitializeOptions = IntPtr.Zero,
						OverrideThreadAffinity = null
					};
					Result result2 = PlatformInterface.Initialize(ref initializeOptions);
					bool flag = result2 > Result.Success;
					if (flag)
					{
						EOSPlatformManager.Logger.Error(string.Format("Failed to initialize EOS SDK: {0}", result2));
						result = result2;
					}
					else
					{
						Options options = new Options
						{
							Reserved = IntPtr.Zero,
							ProductId = "2478e3fa38a64759ae0a92c9e82f97db",
							SandboxId = "357cc3ff28e64b81b2e54bce94f83658",
							ClientCredentials = new ClientCredentials
							{
								ClientId = "xyza789191612kVFquMeGDt4TY3ANrrv",
								ClientSecret = "ULcoQJfUztdw+Vsfs8Q0oIdiu4VH6DdMAouzCpboguI"
							},
							IsServer = false,
							DeploymentId = "93e892227d32478e8b2d5cba0afca1cb",
							Flags = PlatformFlags.None,
							CacheDirectory = Path.Combine(Paths.UserData, "EOSCache"),
							TickBudgetInMilliseconds = 0U,
							RTCOptions = null,
							IntegratedPlatformOptionsContainerHandle = null,
							SystemSpecificOptions = IntPtr.Zero,
							TaskNetworkTimeoutSeconds = new double?(30.0)
						};
						this._platformInterface = PlatformInterface.Create(ref options);
						bool flag2 = this._platformInterface == null;
						if (flag2)
						{
							EOSPlatformManager.Logger.Error("Failed to create EOS Platform Interface");
							result = Result.UnexpectedError;
						}
						else
						{
							this._isInitialized = true;
							EOSPlatformManager.Logger.Info("EOS Platform initialized successfully");
							result = Result.Success;
						}
					}
				}
				catch (Exception exception)
				{
					EOSPlatformManager.Logger.Error(exception, "Exception during EOS initialization");
					result = Result.UnexpectedError;
				}
			}
			return result;
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x0020058C File Offset: 0x001FE78C
		public void Tick()
		{
			bool flag = !this._isInitialized || this._isShuttingDown;
			if (!flag)
			{
				PlatformInterface platformInterface = this._platformInterface;
				if (platformInterface != null)
				{
					platformInterface.Tick();
				}
			}
		}

		// Token: 0x06006166 RID: 24934 RVA: 0x002005C4 File Offset: 0x001FE7C4
		public void Shutdown()
		{
			bool flag = !this._isInitialized || this._isShuttingDown;
			if (!flag)
			{
				this._isShuttingDown = true;
				EOSPlatformManager.Logger.Info("Shutting down EOS Platform");
				try
				{
					PlatformInterface platformInterface = this._platformInterface;
					if (platformInterface != null)
					{
						platformInterface.Release();
					}
					this._platformInterface = null;
					PlatformInterface.Shutdown();
					this._isInitialized = false;
					EOSPlatformManager.Logger.Info("EOS Platform shut down successfully");
				}
				catch (Exception exception)
				{
					EOSPlatformManager.Logger.Error(exception, "Exception during EOS shutdown");
				}
			}
		}

		// Token: 0x06006167 RID: 24935 RVA: 0x00200660 File Offset: 0x001FE860
		public void Dispose()
		{
			this.Shutdown();
		}

		// Token: 0x04003C93 RID: 15507
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003C94 RID: 15508
		private PlatformInterface _platformInterface;

		// Token: 0x04003C95 RID: 15509
		private bool _isInitialized;

		// Token: 0x04003C96 RID: 15510
		private bool _isShuttingDown;

		// Token: 0x04003C97 RID: 15511
		private const string PRODUCT_ID = "2478e3fa38a64759ae0a92c9e82f97db";

		// Token: 0x04003C98 RID: 15512
		private const string SANDBOX_ID = "357cc3ff28e64b81b2e54bce94f83658";

		// Token: 0x04003C99 RID: 15513
		private const string DEPLOYMENT_ID = "93e892227d32478e8b2d5cba0afca1cb";

		// Token: 0x04003C9A RID: 15514
		private const string CLIENT_ID = "xyza789191612kVFquMeGDt4TY3ANrrv";

		// Token: 0x04003C9B RID: 15515
		private const string CLIENT_SECRET = "ULcoQJfUztdw+Vsfs8Q0oIdiu4VH6DdMAouzCpboguI";
	}
}
