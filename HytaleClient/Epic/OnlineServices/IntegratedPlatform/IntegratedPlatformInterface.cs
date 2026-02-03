using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004B9 RID: 1209
	public sealed class IntegratedPlatformInterface : Handle
	{
		// Token: 0x06001F78 RID: 8056 RVA: 0x0002E0C7 File Offset: 0x0002C2C7
		public IntegratedPlatformInterface()
		{
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0002E0D1 File Offset: 0x0002C2D1
		public IntegratedPlatformInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0002E0DC File Offset: 0x0002C2DC
		public ulong AddNotifyUserLoginStatusChanged(ref AddNotifyUserLoginStatusChangedOptions options, object clientData, OnUserLoginStatusChangedCallback callbackFunction)
		{
			AddNotifyUserLoginStatusChangedOptionsInternal addNotifyUserLoginStatusChangedOptionsInternal = default(AddNotifyUserLoginStatusChangedOptionsInternal);
			addNotifyUserLoginStatusChangedOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUserLoginStatusChangedCallbackInternal onUserLoginStatusChangedCallbackInternal = new OnUserLoginStatusChangedCallbackInternal(IntegratedPlatformInterface.OnUserLoginStatusChangedCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, callbackFunction, onUserLoginStatusChangedCallbackInternal, Array.Empty<Delegate>());
			ulong num = Bindings.EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged(base.InnerHandle, ref addNotifyUserLoginStatusChangedOptionsInternal, zero, onUserLoginStatusChangedCallbackInternal);
			Helper.Dispose<AddNotifyUserLoginStatusChangedOptionsInternal>(ref addNotifyUserLoginStatusChangedOptionsInternal);
			Helper.AssignNotificationIdToCallback(zero, num);
			return num;
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0002E148 File Offset: 0x0002C348
		public void ClearUserPreLogoutCallback(ref ClearUserPreLogoutCallbackOptions options)
		{
			ClearUserPreLogoutCallbackOptionsInternal clearUserPreLogoutCallbackOptionsInternal = default(ClearUserPreLogoutCallbackOptionsInternal);
			clearUserPreLogoutCallbackOptionsInternal.Set(ref options);
			Bindings.EOS_IntegratedPlatform_ClearUserPreLogoutCallback(base.InnerHandle, ref clearUserPreLogoutCallbackOptionsInternal);
			Helper.Dispose<ClearUserPreLogoutCallbackOptionsInternal>(ref clearUserPreLogoutCallbackOptionsInternal);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0002E180 File Offset: 0x0002C380
		public static Result CreateIntegratedPlatformOptionsContainer(ref CreateIntegratedPlatformOptionsContainerOptions options, out IntegratedPlatformOptionsContainer outIntegratedPlatformOptionsContainerHandle)
		{
			CreateIntegratedPlatformOptionsContainerOptionsInternal createIntegratedPlatformOptionsContainerOptionsInternal = default(CreateIntegratedPlatformOptionsContainerOptionsInternal);
			createIntegratedPlatformOptionsContainerOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer(ref createIntegratedPlatformOptionsContainerOptionsInternal, ref zero);
			Helper.Dispose<CreateIntegratedPlatformOptionsContainerOptionsInternal>(ref createIntegratedPlatformOptionsContainerOptionsInternal);
			Helper.Get<IntegratedPlatformOptionsContainer>(zero, out outIntegratedPlatformOptionsContainerHandle);
			return result;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0002E1C4 File Offset: 0x0002C3C4
		public Result FinalizeDeferredUserLogout(ref FinalizeDeferredUserLogoutOptions options)
		{
			FinalizeDeferredUserLogoutOptionsInternal finalizeDeferredUserLogoutOptionsInternal = default(FinalizeDeferredUserLogoutOptionsInternal);
			finalizeDeferredUserLogoutOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_IntegratedPlatform_FinalizeDeferredUserLogout(base.InnerHandle, ref finalizeDeferredUserLogoutOptionsInternal);
			Helper.Dispose<FinalizeDeferredUserLogoutOptionsInternal>(ref finalizeDeferredUserLogoutOptionsInternal);
			return result;
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0002E1FE File Offset: 0x0002C3FE
		public void RemoveNotifyUserLoginStatusChanged(ulong notificationId)
		{
			Bindings.EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0002E218 File Offset: 0x0002C418
		public Result SetUserLoginStatus(ref SetUserLoginStatusOptions options)
		{
			SetUserLoginStatusOptionsInternal setUserLoginStatusOptionsInternal = default(SetUserLoginStatusOptionsInternal);
			setUserLoginStatusOptionsInternal.Set(ref options);
			Result result = Bindings.EOS_IntegratedPlatform_SetUserLoginStatus(base.InnerHandle, ref setUserLoginStatusOptionsInternal);
			Helper.Dispose<SetUserLoginStatusOptionsInternal>(ref setUserLoginStatusOptionsInternal);
			return result;
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0002E254 File Offset: 0x0002C454
		public Result SetUserPreLogoutCallback(ref SetUserPreLogoutCallbackOptions options, object clientData, OnUserPreLogoutCallback callbackFunction)
		{
			SetUserPreLogoutCallbackOptionsInternal setUserPreLogoutCallbackOptionsInternal = default(SetUserPreLogoutCallbackOptionsInternal);
			setUserPreLogoutCallbackOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUserPreLogoutCallbackInternal onUserPreLogoutCallbackInternal = new OnUserPreLogoutCallbackInternal(IntegratedPlatformInterface.OnUserPreLogoutCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, callbackFunction, onUserPreLogoutCallbackInternal, Array.Empty<Delegate>());
			Result result = Bindings.EOS_IntegratedPlatform_SetUserPreLogoutCallback(base.InnerHandle, ref setUserPreLogoutCallbackOptionsInternal, zero, onUserPreLogoutCallbackInternal);
			Helper.Dispose<SetUserPreLogoutCallbackOptionsInternal>(ref setUserPreLogoutCallbackOptionsInternal);
			return result;
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0002E2B8 File Offset: 0x0002C4B8
		[MonoPInvokeCallback(typeof(OnUserLoginStatusChangedCallbackInternal))]
		internal static void OnUserLoginStatusChangedCallbackInternalImplementation(ref UserLoginStatusChangedCallbackInfoInternal data)
		{
			OnUserLoginStatusChangedCallback onUserLoginStatusChangedCallback;
			UserLoginStatusChangedCallbackInfo userLoginStatusChangedCallbackInfo;
			bool flag = Helper.TryGetCallback<UserLoginStatusChangedCallbackInfoInternal, OnUserLoginStatusChangedCallback, UserLoginStatusChangedCallbackInfo>(ref data, out onUserLoginStatusChangedCallback, out userLoginStatusChangedCallbackInfo);
			if (flag)
			{
				onUserLoginStatusChangedCallback(ref userLoginStatusChangedCallbackInfo);
			}
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0002E2E0 File Offset: 0x0002C4E0
		[MonoPInvokeCallback(typeof(OnUserPreLogoutCallbackInternal))]
		internal static IntegratedPlatformPreLogoutAction OnUserPreLogoutCallbackInternalImplementation(ref UserPreLogoutCallbackInfoInternal data)
		{
			OnUserPreLogoutCallback onUserPreLogoutCallback;
			UserPreLogoutCallbackInfo userPreLogoutCallbackInfo;
			bool flag = Helper.TryGetCallback<UserPreLogoutCallbackInfoInternal, OnUserPreLogoutCallback, UserPreLogoutCallbackInfo>(ref data, out onUserPreLogoutCallback, out userPreLogoutCallbackInfo);
			IntegratedPlatformPreLogoutAction result;
			if (flag)
			{
				IntegratedPlatformPreLogoutAction integratedPlatformPreLogoutAction = onUserPreLogoutCallback(ref userPreLogoutCallbackInfo);
				result = integratedPlatformPreLogoutAction;
			}
			else
			{
				result = Helper.GetDefault<IntegratedPlatformPreLogoutAction>();
			}
			return result;
		}

		// Token: 0x04000DAA RID: 3498
		public const int AddnotifyuserloginstatuschangedApiLatest = 1;

		// Token: 0x04000DAB RID: 3499
		public const int ClearuserprelogoutcallbackApiLatest = 1;

		// Token: 0x04000DAC RID: 3500
		public const int CreateintegratedplatformoptionscontainerApiLatest = 1;

		// Token: 0x04000DAD RID: 3501
		public const int FinalizedeferreduserlogoutApiLatest = 1;

		// Token: 0x04000DAE RID: 3502
		public const int OptionsApiLatest = 1;

		// Token: 0x04000DAF RID: 3503
		public const int SetuserloginstatusApiLatest = 1;

		// Token: 0x04000DB0 RID: 3504
		public const int SetuserprelogoutcallbackApiLatest = 1;

		// Token: 0x04000DB1 RID: 3505
		public const int SteamMaxSteamapiinterfaceversionsarraySize = 4096;

		// Token: 0x04000DB2 RID: 3506
		public const int SteamOptionsApiLatest = 3;
	}
}
