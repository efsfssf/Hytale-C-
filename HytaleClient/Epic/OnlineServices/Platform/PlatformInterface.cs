using System;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatServer;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.CustomInvites;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.KWS;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.ProgressionSnapshot;
using Epic.OnlineServices.Reports;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAdmin;
using Epic.OnlineServices.Sanctions;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000706 RID: 1798
	public sealed class PlatformInterface : Handle
	{
		// Token: 0x06002E4A RID: 11850 RVA: 0x000445CC File Offset: 0x000427CC
		public static Result Initialize(ref AndroidInitializeOptions options)
		{
			AndroidInitializeOptionsInternal androidInitializeOptionsInternal = default(AndroidInitializeOptionsInternal);
			androidInitializeOptionsInternal.Set(ref options);
			Result result = AndroidBindings.EOS_Initialize(ref androidInitializeOptionsInternal);
			Helper.Dispose<AndroidInitializeOptionsInternal>(ref androidInitializeOptionsInternal);
			return result;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x00044600 File Offset: 0x00042800
		public PlatformInterface()
		{
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x0004460A File Offset: 0x0004280A
		public PlatformInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x00044618 File Offset: 0x00042818
		public Result CheckForLauncherAndRestart()
		{
			return Bindings.EOS_Platform_CheckForLauncherAndRestart(base.InnerHandle);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x00044638 File Offset: 0x00042838
		public static PlatformInterface Create(ref Options options)
		{
			OptionsInternal optionsInternal = default(OptionsInternal);
			optionsInternal.Set(ref options);
			IntPtr from = Bindings.EOS_Platform_Create(ref optionsInternal);
			Helper.Dispose<OptionsInternal>(ref optionsInternal);
			PlatformInterface result;
			Helper.Get<PlatformInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x00044678 File Offset: 0x00042878
		public AchievementsInterface GetAchievementsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetAchievementsInterface(base.InnerHandle);
			AchievementsInterface result;
			Helper.Get<AchievementsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000446A0 File Offset: 0x000428A0
		public Result GetActiveCountryCode(EpicAccountId localUserId, out Utf8String outBuffer)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			int size = 5;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Platform_GetActiveCountryCode(base.InnerHandle, zero, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000446F0 File Offset: 0x000428F0
		public Result GetActiveLocaleCode(EpicAccountId localUserId, out Utf8String outBuffer)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(localUserId, ref zero);
			int size = 10;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Platform_GetActiveLocaleCode(base.InnerHandle, zero, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x00044740 File Offset: 0x00042940
		public AntiCheatClientInterface GetAntiCheatClientInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetAntiCheatClientInterface(base.InnerHandle);
			AntiCheatClientInterface result;
			Helper.Get<AntiCheatClientInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x00044768 File Offset: 0x00042968
		public AntiCheatServerInterface GetAntiCheatServerInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetAntiCheatServerInterface(base.InnerHandle);
			AntiCheatServerInterface result;
			Helper.Get<AntiCheatServerInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x00044790 File Offset: 0x00042990
		public ApplicationStatus GetApplicationStatus()
		{
			return Bindings.EOS_Platform_GetApplicationStatus(base.InnerHandle);
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000447B0 File Offset: 0x000429B0
		public AuthInterface GetAuthInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetAuthInterface(base.InnerHandle);
			AuthInterface result;
			Helper.Get<AuthInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000447D8 File Offset: 0x000429D8
		public ConnectInterface GetConnectInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetConnectInterface(base.InnerHandle);
			ConnectInterface result;
			Helper.Get<ConnectInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x00044800 File Offset: 0x00042A00
		public CustomInvitesInterface GetCustomInvitesInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetCustomInvitesInterface(base.InnerHandle);
			CustomInvitesInterface result;
			Helper.Get<CustomInvitesInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x00044828 File Offset: 0x00042A28
		public Result GetDesktopCrossplayStatus(ref GetDesktopCrossplayStatusOptions options, out DesktopCrossplayStatusInfo outDesktopCrossplayStatusInfo)
		{
			GetDesktopCrossplayStatusOptionsInternal getDesktopCrossplayStatusOptionsInternal = default(GetDesktopCrossplayStatusOptionsInternal);
			getDesktopCrossplayStatusOptionsInternal.Set(ref options);
			DesktopCrossplayStatusInfoInternal @default = Helper.GetDefault<DesktopCrossplayStatusInfoInternal>();
			Result result = Bindings.EOS_Platform_GetDesktopCrossplayStatus(base.InnerHandle, ref getDesktopCrossplayStatusOptionsInternal, ref @default);
			Helper.Dispose<GetDesktopCrossplayStatusOptionsInternal>(ref getDesktopCrossplayStatusOptionsInternal);
			Helper.Get<DesktopCrossplayStatusInfoInternal, DesktopCrossplayStatusInfo>(ref @default, out outDesktopCrossplayStatusInfo);
			return result;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x00044874 File Offset: 0x00042A74
		public EcomInterface GetEcomInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetEcomInterface(base.InnerHandle);
			EcomInterface result;
			Helper.Get<EcomInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x0004489C File Offset: 0x00042A9C
		public FriendsInterface GetFriendsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetFriendsInterface(base.InnerHandle);
			FriendsInterface result;
			Helper.Get<FriendsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000448C4 File Offset: 0x00042AC4
		public IntegratedPlatformInterface GetIntegratedPlatformInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetIntegratedPlatformInterface(base.InnerHandle);
			IntegratedPlatformInterface result;
			Helper.Get<IntegratedPlatformInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000448EC File Offset: 0x00042AEC
		public KWSInterface GetKWSInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetKWSInterface(base.InnerHandle);
			KWSInterface result;
			Helper.Get<KWSInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x00044914 File Offset: 0x00042B14
		public LeaderboardsInterface GetLeaderboardsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetLeaderboardsInterface(base.InnerHandle);
			LeaderboardsInterface result;
			Helper.Get<LeaderboardsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x0004493C File Offset: 0x00042B3C
		public LobbyInterface GetLobbyInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetLobbyInterface(base.InnerHandle);
			LobbyInterface result;
			Helper.Get<LobbyInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x00044964 File Offset: 0x00042B64
		public MetricsInterface GetMetricsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetMetricsInterface(base.InnerHandle);
			MetricsInterface result;
			Helper.Get<MetricsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0004498C File Offset: 0x00042B8C
		public ModsInterface GetModsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetModsInterface(base.InnerHandle);
			ModsInterface result;
			Helper.Get<ModsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000449B4 File Offset: 0x00042BB4
		public NetworkStatus GetNetworkStatus()
		{
			return Bindings.EOS_Platform_GetNetworkStatus(base.InnerHandle);
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000449D4 File Offset: 0x00042BD4
		public Result GetOverrideCountryCode(out Utf8String outBuffer)
		{
			int size = 5;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Platform_GetOverrideCountryCode(base.InnerHandle, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x00044A10 File Offset: 0x00042C10
		public Result GetOverrideLocaleCode(out Utf8String outBuffer)
		{
			int size = 10;
			IntPtr intPtr = Helper.AddAllocation(size);
			Result result = Bindings.EOS_Platform_GetOverrideLocaleCode(base.InnerHandle, intPtr, ref size);
			Helper.Get(intPtr, out outBuffer);
			Helper.Dispose(ref intPtr);
			return result;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x00044A4C File Offset: 0x00042C4C
		public P2PInterface GetP2PInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetP2PInterface(base.InnerHandle);
			P2PInterface result;
			Helper.Get<P2PInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x00044A74 File Offset: 0x00042C74
		public PlayerDataStorageInterface GetPlayerDataStorageInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetPlayerDataStorageInterface(base.InnerHandle);
			PlayerDataStorageInterface result;
			Helper.Get<PlayerDataStorageInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x00044A9C File Offset: 0x00042C9C
		public PresenceInterface GetPresenceInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetPresenceInterface(base.InnerHandle);
			PresenceInterface result;
			Helper.Get<PresenceInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x00044AC4 File Offset: 0x00042CC4
		public ProgressionSnapshotInterface GetProgressionSnapshotInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetProgressionSnapshotInterface(base.InnerHandle);
			ProgressionSnapshotInterface result;
			Helper.Get<ProgressionSnapshotInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x00044AEC File Offset: 0x00042CEC
		public RTCAdminInterface GetRTCAdminInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetRTCAdminInterface(base.InnerHandle);
			RTCAdminInterface result;
			Helper.Get<RTCAdminInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x00044B14 File Offset: 0x00042D14
		public RTCInterface GetRTCInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetRTCInterface(base.InnerHandle);
			RTCInterface result;
			Helper.Get<RTCInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x00044B3C File Offset: 0x00042D3C
		public ReportsInterface GetReportsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetReportsInterface(base.InnerHandle);
			ReportsInterface result;
			Helper.Get<ReportsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x00044B64 File Offset: 0x00042D64
		public SanctionsInterface GetSanctionsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetSanctionsInterface(base.InnerHandle);
			SanctionsInterface result;
			Helper.Get<SanctionsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x00044B8C File Offset: 0x00042D8C
		public SessionsInterface GetSessionsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetSessionsInterface(base.InnerHandle);
			SessionsInterface result;
			Helper.Get<SessionsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x00044BB4 File Offset: 0x00042DB4
		public StatsInterface GetStatsInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetStatsInterface(base.InnerHandle);
			StatsInterface result;
			Helper.Get<StatsInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x00044BDC File Offset: 0x00042DDC
		public TitleStorageInterface GetTitleStorageInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetTitleStorageInterface(base.InnerHandle);
			TitleStorageInterface result;
			Helper.Get<TitleStorageInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x00044C04 File Offset: 0x00042E04
		public UIInterface GetUIInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetUIInterface(base.InnerHandle);
			UIInterface result;
			Helper.Get<UIInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x00044C2C File Offset: 0x00042E2C
		public UserInfoInterface GetUserInfoInterface()
		{
			IntPtr from = Bindings.EOS_Platform_GetUserInfoInterface(base.InnerHandle);
			UserInfoInterface result;
			Helper.Get<UserInfoInterface>(from, out result);
			return result;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x00044C54 File Offset: 0x00042E54
		public static Result Initialize(ref InitializeOptions options)
		{
			InitializeOptionsInternal initializeOptionsInternal = default(InitializeOptionsInternal);
			initializeOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_Initialize(ref initializeOptionsInternal);
			Helper.Dispose<InitializeOptionsInternal>(ref initializeOptionsInternal);
			return result;
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x00044C88 File Offset: 0x00042E88
		public void Release()
		{
			Bindings.EOS_Platform_Release(base.InnerHandle);
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x00044C98 File Offset: 0x00042E98
		public Result SetApplicationStatus(ApplicationStatus newStatus)
		{
			return Bindings.EOS_Platform_SetApplicationStatus(base.InnerHandle, newStatus);
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x00044CB8 File Offset: 0x00042EB8
		public Result SetNetworkStatus(NetworkStatus newStatus)
		{
			return Bindings.EOS_Platform_SetNetworkStatus(base.InnerHandle, newStatus);
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x00044CD8 File Offset: 0x00042ED8
		public Result SetOverrideCountryCode(Utf8String newCountryCode)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(newCountryCode, ref zero);
			Result result = Bindings.EOS_Platform_SetOverrideCountryCode(base.InnerHandle, zero);
			Helper.Dispose(ref zero);
			return result;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x00044D10 File Offset: 0x00042F10
		public Result SetOverrideLocaleCode(Utf8String newLocaleCode)
		{
			IntPtr zero = IntPtr.Zero;
			Helper.Set(newLocaleCode, ref zero);
			Result result = Bindings.EOS_Platform_SetOverrideLocaleCode(base.InnerHandle, zero);
			Helper.Dispose(ref zero);
			return result;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x00044D48 File Offset: 0x00042F48
		public static Result Shutdown()
		{
			return Bindings.EOS_Shutdown();
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x00044D61 File Offset: 0x00042F61
		public void Tick()
		{
			Bindings.EOS_Platform_Tick(base.InnerHandle);
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x00044D70 File Offset: 0x00042F70
		public static Utf8String ToString(ApplicationStatus applicationStatus)
		{
			IntPtr from = Bindings.EOS_EApplicationStatus_ToString(applicationStatus);
			Utf8String result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x00044D94 File Offset: 0x00042F94
		public static Utf8String ToString(NetworkStatus networkStatus)
		{
			IntPtr from = Bindings.EOS_ENetworkStatus_ToString(networkStatus);
			Utf8String result;
			Helper.Get(from, out result);
			return result;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x00044DB8 File Offset: 0x00042FB8
		public static PlatformInterface Create(ref WindowsOptions options)
		{
			WindowsOptionsInternal windowsOptionsInternal = default(WindowsOptionsInternal);
			windowsOptionsInternal.Set(ref options);
			IntPtr from = WindowsBindings.EOS_Platform_Create(ref windowsOptionsInternal);
			Helper.Dispose<WindowsOptionsInternal>(ref windowsOptionsInternal);
			PlatformInterface result;
			Helper.Get<PlatformInterface>(from, out result);
			return result;
		}

		// Token: 0x04001478 RID: 5240
		public const int AndroidInitializeoptionssysteminitializeoptionsApiLatest = 2;

		// Token: 0x04001479 RID: 5241
		public static readonly Utf8String CheckforlauncherandrestartEnvVar = "EOS_LAUNCHED_BY_EPIC";

		// Token: 0x0400147A RID: 5242
		public const int ClientcredentialsClientidMaxLength = 64;

		// Token: 0x0400147B RID: 5243
		public const int ClientcredentialsClientsecretMaxLength = 64;

		// Token: 0x0400147C RID: 5244
		public const int CountrycodeMaxBufferLen = 5;

		// Token: 0x0400147D RID: 5245
		public const int CountrycodeMaxLength = 4;

		// Token: 0x0400147E RID: 5246
		public const int GetdesktopcrossplaystatusApiLatest = 1;

		// Token: 0x0400147F RID: 5247
		public const int InitializeApiLatest = 4;

		// Token: 0x04001480 RID: 5248
		public const int InitializeThreadaffinityApiLatest = 3;

		// Token: 0x04001481 RID: 5249
		public const int InitializeoptionsProductnameMaxLength = 64;

		// Token: 0x04001482 RID: 5250
		public const int InitializeoptionsProductversionMaxLength = 64;

		// Token: 0x04001483 RID: 5251
		public const int LocalecodeMaxBufferLen = 10;

		// Token: 0x04001484 RID: 5252
		public const int LocalecodeMaxLength = 9;

		// Token: 0x04001485 RID: 5253
		public const int OptionsApiLatest = 14;

		// Token: 0x04001486 RID: 5254
		public const int OptionsDeploymentidMaxLength = 64;

		// Token: 0x04001487 RID: 5255
		public const int OptionsEncryptionkeyLength = 64;

		// Token: 0x04001488 RID: 5256
		public const int OptionsProductidMaxLength = 64;

		// Token: 0x04001489 RID: 5257
		public const int OptionsSandboxidMaxLength = 64;

		// Token: 0x0400148A RID: 5258
		public const int RtcoptionsApiLatest = 2;

		// Token: 0x0400148B RID: 5259
		public const int WindowsRtcoptionsplatformspecificoptionsApiLatest = 1;
	}
}
