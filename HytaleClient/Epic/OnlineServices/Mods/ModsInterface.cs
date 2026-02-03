using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200033C RID: 828
	public sealed class ModsInterface : Handle
	{
		// Token: 0x060016B7 RID: 5815 RVA: 0x00021324 File Offset: 0x0001F524
		public ModsInterface()
		{
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0002132E File Offset: 0x0001F52E
		public ModsInterface(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0002133C File Offset: 0x0001F53C
		public Result CopyModInfo(ref CopyModInfoOptions options, out ModInfo? outEnumeratedMods)
		{
			CopyModInfoOptionsInternal copyModInfoOptionsInternal = default(CopyModInfoOptionsInternal);
			copyModInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_Mods_CopyModInfo(base.InnerHandle, ref copyModInfoOptionsInternal, ref zero);
			Helper.Dispose<CopyModInfoOptionsInternal>(ref copyModInfoOptionsInternal);
			Helper.Get<ModInfoInternal, ModInfo>(zero, out outEnumeratedMods);
			bool flag = outEnumeratedMods != null;
			if (flag)
			{
				Bindings.EOS_Mods_ModInfo_Release(zero);
			}
			return result;
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0002139C File Offset: 0x0001F59C
		public void EnumerateMods(ref EnumerateModsOptions options, object clientData, OnEnumerateModsCallback completionDelegate)
		{
			EnumerateModsOptionsInternal enumerateModsOptionsInternal = default(EnumerateModsOptionsInternal);
			enumerateModsOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnEnumerateModsCallbackInternal onEnumerateModsCallbackInternal = new OnEnumerateModsCallbackInternal(ModsInterface.OnEnumerateModsCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onEnumerateModsCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Mods_EnumerateMods(base.InnerHandle, ref enumerateModsOptionsInternal, zero, onEnumerateModsCallbackInternal);
			Helper.Dispose<EnumerateModsOptionsInternal>(ref enumerateModsOptionsInternal);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x000213F8 File Offset: 0x0001F5F8
		public void InstallMod(ref InstallModOptions options, object clientData, OnInstallModCallback completionDelegate)
		{
			InstallModOptionsInternal installModOptionsInternal = default(InstallModOptionsInternal);
			installModOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnInstallModCallbackInternal onInstallModCallbackInternal = new OnInstallModCallbackInternal(ModsInterface.OnInstallModCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onInstallModCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Mods_InstallMod(base.InnerHandle, ref installModOptionsInternal, zero, onInstallModCallbackInternal);
			Helper.Dispose<InstallModOptionsInternal>(ref installModOptionsInternal);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00021454 File Offset: 0x0001F654
		public void UninstallMod(ref UninstallModOptions options, object clientData, OnUninstallModCallback completionDelegate)
		{
			UninstallModOptionsInternal uninstallModOptionsInternal = default(UninstallModOptionsInternal);
			uninstallModOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUninstallModCallbackInternal onUninstallModCallbackInternal = new OnUninstallModCallbackInternal(ModsInterface.OnUninstallModCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUninstallModCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Mods_UninstallMod(base.InnerHandle, ref uninstallModOptionsInternal, zero, onUninstallModCallbackInternal);
			Helper.Dispose<UninstallModOptionsInternal>(ref uninstallModOptionsInternal);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000214B0 File Offset: 0x0001F6B0
		public void UpdateMod(ref UpdateModOptions options, object clientData, OnUpdateModCallback completionDelegate)
		{
			UpdateModOptionsInternal updateModOptionsInternal = default(UpdateModOptionsInternal);
			updateModOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			OnUpdateModCallbackInternal onUpdateModCallbackInternal = new OnUpdateModCallbackInternal(ModsInterface.OnUpdateModCallbackInternalImplementation);
			Helper.AddCallback(out zero, clientData, completionDelegate, onUpdateModCallbackInternal, Array.Empty<Delegate>());
			Bindings.EOS_Mods_UpdateMod(base.InnerHandle, ref updateModOptionsInternal, zero, onUpdateModCallbackInternal);
			Helper.Dispose<UpdateModOptionsInternal>(ref updateModOptionsInternal);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0002150C File Offset: 0x0001F70C
		[MonoPInvokeCallback(typeof(OnEnumerateModsCallbackInternal))]
		internal static void OnEnumerateModsCallbackInternalImplementation(ref EnumerateModsCallbackInfoInternal data)
		{
			OnEnumerateModsCallback onEnumerateModsCallback;
			EnumerateModsCallbackInfo enumerateModsCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<EnumerateModsCallbackInfoInternal, OnEnumerateModsCallback, EnumerateModsCallbackInfo>(ref data, out onEnumerateModsCallback, out enumerateModsCallbackInfo);
			if (flag)
			{
				onEnumerateModsCallback(ref enumerateModsCallbackInfo);
			}
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00021534 File Offset: 0x0001F734
		[MonoPInvokeCallback(typeof(OnInstallModCallbackInternal))]
		internal static void OnInstallModCallbackInternalImplementation(ref InstallModCallbackInfoInternal data)
		{
			OnInstallModCallback onInstallModCallback;
			InstallModCallbackInfo installModCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<InstallModCallbackInfoInternal, OnInstallModCallback, InstallModCallbackInfo>(ref data, out onInstallModCallback, out installModCallbackInfo);
			if (flag)
			{
				onInstallModCallback(ref installModCallbackInfo);
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0002155C File Offset: 0x0001F75C
		[MonoPInvokeCallback(typeof(OnUninstallModCallbackInternal))]
		internal static void OnUninstallModCallbackInternalImplementation(ref UninstallModCallbackInfoInternal data)
		{
			OnUninstallModCallback onUninstallModCallback;
			UninstallModCallbackInfo uninstallModCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UninstallModCallbackInfoInternal, OnUninstallModCallback, UninstallModCallbackInfo>(ref data, out onUninstallModCallback, out uninstallModCallbackInfo);
			if (flag)
			{
				onUninstallModCallback(ref uninstallModCallbackInfo);
			}
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00021584 File Offset: 0x0001F784
		[MonoPInvokeCallback(typeof(OnUpdateModCallbackInternal))]
		internal static void OnUpdateModCallbackInternalImplementation(ref UpdateModCallbackInfoInternal data)
		{
			OnUpdateModCallback onUpdateModCallback;
			UpdateModCallbackInfo updateModCallbackInfo;
			bool flag = Helper.TryGetAndRemoveCallback<UpdateModCallbackInfoInternal, OnUpdateModCallback, UpdateModCallbackInfo>(ref data, out onUpdateModCallback, out updateModCallbackInfo);
			if (flag)
			{
				onUpdateModCallback(ref updateModCallbackInfo);
			}
		}

		// Token: 0x040009ED RID: 2541
		public const int CopymodinfoApiLatest = 1;

		// Token: 0x040009EE RID: 2542
		public const int EnumeratemodsApiLatest = 1;

		// Token: 0x040009EF RID: 2543
		public const int InstallmodApiLatest = 1;

		// Token: 0x040009F0 RID: 2544
		public const int ModIdentifierApiLatest = 1;

		// Token: 0x040009F1 RID: 2545
		public const int ModinfoApiLatest = 1;

		// Token: 0x040009F2 RID: 2546
		public const int UninstallmodApiLatest = 1;

		// Token: 0x040009F3 RID: 2547
		public const int UpdatemodApiLatest = 1;
	}
}
