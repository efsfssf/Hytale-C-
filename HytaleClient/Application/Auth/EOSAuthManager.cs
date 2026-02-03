using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using HytaleClient.Application.Services;
using NLog;

namespace HytaleClient.Application.Auth
{
	// Token: 0x02000C03 RID: 3075
	public class EOSAuthManager
	{
		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06006226 RID: 25126 RVA: 0x00205A93 File Offset: 0x00203C93
		// (set) Token: 0x06006227 RID: 25127 RVA: 0x00205A9B File Offset: 0x00203C9B
		public bool IsLoggedIn { get; private set; }

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06006228 RID: 25128 RVA: 0x00205AA4 File Offset: 0x00203CA4
		// (set) Token: 0x06006229 RID: 25129 RVA: 0x00205AAC File Offset: 0x00203CAC
		public EpicAccountId LocalUserId { get; private set; }

		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x0600622A RID: 25130 RVA: 0x00205AB5 File Offset: 0x00203CB5
		// (set) Token: 0x0600622B RID: 25131 RVA: 0x00205ABD File Offset: 0x00203CBD
		public ProductUserId ProductUserId { get; private set; }

		// Token: 0x0600622C RID: 25132 RVA: 0x00205AC8 File Offset: 0x00203CC8
		public EOSAuthManager(EOSPlatformManager platformManager)
		{
			this._platformManager = platformManager;
			bool isInitialized = this._platformManager.IsInitialized;
			if (isInitialized)
			{
				this._authInterface = this._platformManager.Platform.GetAuthInterface();
				this._connectInterface = this._platformManager.Platform.GetConnectInterface();
			}
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x00205B24 File Offset: 0x00203D24
		public void LoginWithDeviceId(Action<Result> callback)
		{
			bool flag = this._connectInterface == null;
			if (flag)
			{
				EOSAuthManager.Logger.Error("Connect interface not available");
				Action<Result> callback2 = callback;
				if (callback2 != null)
				{
					callback2(Result.NotConfigured);
				}
			}
			else
			{
				CreateDeviceIdOptions createDeviceIdOptions = new CreateDeviceIdOptions
				{
					DeviceModel = "PC"
				};
				Epic.OnlineServices.Connect.OnLoginCallback <>9__1;
				this._connectInterface.CreateDeviceId(ref createDeviceIdOptions, null, delegate(ref CreateDeviceIdCallbackInfo createCallbackInfo)
				{
					Result resultCode = createCallbackInfo.ResultCode;
					bool flag2 = resultCode != Result.Success && resultCode != Result.DuplicateNotAllowed;
					if (flag2)
					{
						EOSAuthManager.Logger.Error(string.Format("Failed to create device ID: {0}", resultCode));
						Action<Result> callback3 = callback;
						if (callback3 != null)
						{
							callback3(resultCode);
						}
					}
					else
					{
						Epic.OnlineServices.Connect.LoginOptions loginOptions = new Epic.OnlineServices.Connect.LoginOptions
						{
							Credentials = new Epic.OnlineServices.Connect.Credentials?(new Epic.OnlineServices.Connect.Credentials
							{
								Type = ExternalCredentialType.DeviceidAccessToken,
								Token = null
							})
						};
						ConnectInterface connectInterface = this._connectInterface;
						object clientData = null;
						Epic.OnlineServices.Connect.OnLoginCallback completionDelegate;
						if ((completionDelegate = <>9__1) == null)
						{
							completionDelegate = (<>9__1 = delegate(ref Epic.OnlineServices.Connect.LoginCallbackInfo loginCallbackInfo)
							{
								Result resultCode2 = loginCallbackInfo.ResultCode;
								ProductUserId localUserId = loginCallbackInfo.LocalUserId;
								bool flag3 = resultCode2 == Result.Success;
								if (flag3)
								{
									this.ProductUserId = localUserId;
									this.IsLoggedIn = true;
									EOSAuthManager.Logger.Info(string.Format("Successfully logged in with Product User ID: {0}", this.ProductUserId));
								}
								else
								{
									EOSAuthManager.Logger.Error(string.Format("Failed to login: {0}", resultCode2));
								}
								Action<Result> callback4 = callback;
								if (callback4 != null)
								{
									callback4(resultCode2);
								}
							});
						}
						connectInterface.Login(ref loginOptions, clientData, completionDelegate);
					}
				});
			}
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x00205BB4 File Offset: 0x00203DB4
		public void LoginWithEpicAccount(LoginCredentialType credentialType, string id, string token, Action<Result> callback)
		{
			bool flag = this._authInterface == null;
			if (flag)
			{
				EOSAuthManager.Logger.Error("Auth interface not available");
				Action<Result> callback2 = callback;
				if (callback2 != null)
				{
					callback2(Result.NotConfigured);
				}
			}
			else
			{
				Epic.OnlineServices.Auth.LoginOptions loginOptions = new Epic.OnlineServices.Auth.LoginOptions
				{
					Credentials = new Epic.OnlineServices.Auth.Credentials?(new Epic.OnlineServices.Auth.Credentials
					{
						Type = credentialType,
						Id = id,
						Token = token
					}),
					ScopeFlags = (AuthScopeFlags.BasicProfile | AuthScopeFlags.FriendsList | AuthScopeFlags.Presence)
				};
				this._authInterface.Login(ref loginOptions, null, delegate(ref Epic.OnlineServices.Auth.LoginCallbackInfo authCallbackInfo)
				{
					Result resultCode = authCallbackInfo.ResultCode;
					EpicAccountId localUserId = authCallbackInfo.LocalUserId;
					bool flag2 = resultCode == Result.Success;
					if (flag2)
					{
						this.LocalUserId = localUserId;
						EOSAuthManager.Logger.Info(string.Format("Successfully logged in with Epic Account ID: {0}", this.LocalUserId));
						this.LinkEpicAccountToConnect(delegate
						{
							Action<Result> callback4 = callback;
							if (callback4 != null)
							{
								callback4(resultCode);
							}
						});
					}
					else
					{
						EOSAuthManager.Logger.Error(string.Format("Failed to login to Epic account: {0}", resultCode));
						Action<Result> callback3 = callback;
						if (callback3 != null)
						{
							callback3(resultCode);
						}
					}
				});
			}
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x00205C78 File Offset: 0x00203E78
		private void LinkEpicAccountToConnect(Action onComplete)
		{
			bool flag = this._authInterface == null || this.LocalUserId == null || this._connectInterface == null;
			if (flag)
			{
				Action onComplete2 = onComplete;
				if (onComplete2 != null)
				{
					onComplete2();
				}
			}
			else
			{
				Epic.OnlineServices.Auth.CopyIdTokenOptions copyIdTokenOptions = new Epic.OnlineServices.Auth.CopyIdTokenOptions
				{
					AccountId = this.LocalUserId
				};
				Epic.OnlineServices.Auth.IdToken? idToken;
				bool flag2 = this._authInterface.CopyIdToken(ref copyIdTokenOptions, out idToken) == Result.Success;
				if (flag2)
				{
					Epic.OnlineServices.Connect.LoginOptions loginOptions = new Epic.OnlineServices.Connect.LoginOptions
					{
						Credentials = new Epic.OnlineServices.Connect.Credentials?(new Epic.OnlineServices.Connect.Credentials
						{
							Type = ExternalCredentialType.EpicIdToken,
							Token = idToken.Value.JsonWebToken
						})
					};
					this._connectInterface.Login(ref loginOptions, null, delegate(ref Epic.OnlineServices.Connect.LoginCallbackInfo connectCallbackInfo)
					{
						Result resultCode = connectCallbackInfo.ResultCode;
						ProductUserId localUserId = connectCallbackInfo.LocalUserId;
						bool flag3 = resultCode == Result.Success;
						if (flag3)
						{
							this.ProductUserId = localUserId;
							EOSAuthManager.Logger.Info(string.Format("Successfully linked Epic account to Connect interface with Product User ID: {0}", this.ProductUserId));
						}
						else
						{
							bool flag4 = resultCode == Result.InvalidUser;
							if (flag4)
							{
								this.CreateConnectUser(onComplete);
								return;
							}
							EOSAuthManager.Logger.Error(string.Format("Failed to link Epic account to Connect: {0}", resultCode));
						}
						Action onComplete4 = onComplete;
						if (onComplete4 != null)
						{
							onComplete4();
						}
					});
					idToken = null;
				}
				else
				{
					EOSAuthManager.Logger.Error("Failed to copy ID token for Connect login");
					Action onComplete3 = onComplete;
					if (onComplete3 != null)
					{
						onComplete3();
					}
				}
			}
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x00205DA4 File Offset: 0x00203FA4
		private void CreateConnectUser(Action onComplete)
		{
			bool flag = this._authInterface == null || this.LocalUserId == null || this._connectInterface == null;
			if (flag)
			{
				Action onComplete2 = onComplete;
				if (onComplete2 != null)
				{
					onComplete2();
				}
			}
			else
			{
				CopyUserAuthTokenOptions copyUserAuthTokenOptions = default(CopyUserAuthTokenOptions);
				Token? token;
				bool flag2 = this._authInterface.CopyUserAuthToken(ref copyUserAuthTokenOptions, this.LocalUserId, out token) == Result.Success;
				if (flag2)
				{
					CreateUserOptions createUserOptions = new CreateUserOptions
					{
						ContinuanceToken = null
					};
					this._connectInterface.CreateUser(ref createUserOptions, null, delegate(ref CreateUserCallbackInfo createUserCallbackInfo)
					{
						Result resultCode = createUserCallbackInfo.ResultCode;
						ProductUserId localUserId = createUserCallbackInfo.LocalUserId;
						bool flag3 = resultCode == Result.Success;
						if (flag3)
						{
							this.ProductUserId = localUserId;
							EOSAuthManager.Logger.Info(string.Format("Successfully created Connect user with Product User ID: {0}", this.ProductUserId));
						}
						else
						{
							EOSAuthManager.Logger.Error(string.Format("Failed to create Connect user: {0}", resultCode));
						}
						Action onComplete4 = onComplete;
						if (onComplete4 != null)
						{
							onComplete4();
						}
					});
					token = null;
				}
				else
				{
					EOSAuthManager.Logger.Error("Failed to copy user auth token for Connect user creation");
					Action onComplete3 = onComplete;
					if (onComplete3 != null)
					{
						onComplete3();
					}
				}
			}
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x00205E94 File Offset: 0x00204094
		public void Logout(Action<Result> callback)
		{
			bool flag = this._authInterface != null && this.LocalUserId != null;
			if (flag)
			{
				Epic.OnlineServices.Auth.LogoutOptions logoutOptions = new Epic.OnlineServices.Auth.LogoutOptions
				{
					LocalUserId = this.LocalUserId
				};
				this._authInterface.Logout(ref logoutOptions, null, delegate(ref Epic.OnlineServices.Auth.LogoutCallbackInfo authCallbackInfo)
				{
					Result resultCode = authCallbackInfo.ResultCode;
					bool flag2 = resultCode == Result.Success;
					if (flag2)
					{
						this.LocalUserId = null;
						this.IsLoggedIn = false;
						EOSAuthManager.Logger.Info("Successfully logged out");
					}
					Action<Result> callback3 = callback;
					if (callback3 != null)
					{
						callback3(resultCode);
					}
				});
			}
			else
			{
				Action<Result> callback2 = callback;
				if (callback2 != null)
				{
					callback2(Result.NotFound);
				}
			}
		}

		// Token: 0x04003D14 RID: 15636
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003D15 RID: 15637
		private readonly EOSPlatformManager _platformManager;

		// Token: 0x04003D16 RID: 15638
		private AuthInterface _authInterface;

		// Token: 0x04003D17 RID: 15639
		private ConnectInterface _connectInterface;
	}
}
