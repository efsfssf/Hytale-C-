using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SDL2;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD7 RID: 2775
	internal sealed class Settings
	{
		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x0600577D RID: 22397 RVA: 0x001A88D0 File Offset: 0x001A6AD0
		// (set) Token: 0x0600577E RID: 22398 RVA: 0x001A88F8 File Offset: 0x001A6AF8
		public int CurrentAdventureInteractionDistance
		{
			get
			{
				return this.PlaceBlocksAtRangeInAdventureMode ? this.currentAdventureInteractionDistance : this.minimumInteractionDistance;
			}
			set
			{
				this.currentAdventureInteractionDistance = value;
				this.currentAdventureInteractionDistance = MathHelper.Clamp(this.currentAdventureInteractionDistance, this.minimumInteractionDistance, this.adventureInteractionDistance);
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x001A8920 File Offset: 0x001A6B20
		// (set) Token: 0x06005780 RID: 22400 RVA: 0x001A8948 File Offset: 0x001A6B48
		public int CurrentCreativeInteractionDistance
		{
			get
			{
				return this._placeBlocksAtRange ? this.currentCreativeInteractionDistance : this.minimumInteractionDistance;
			}
			set
			{
				this.currentCreativeInteractionDistance = value;
				this.currentCreativeInteractionDistance = MathHelper.Clamp(this.currentCreativeInteractionDistance, this.minimumInteractionDistance, this.creativeInteractionDistance);
			}
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x001A8970 File Offset: 0x001A6B70
		public bool InteractionDistanceIsMinimum(GameMode currentMode)
		{
			bool flag = currentMode == 0;
			bool result;
			if (flag)
			{
				bool placeBlocksAtRangeInAdventureMode = this.PlaceBlocksAtRangeInAdventureMode;
				result = (!placeBlocksAtRangeInAdventureMode || this.currentAdventureInteractionDistance == this.minimumInteractionDistance);
			}
			else
			{
				bool flag2 = currentMode == 1;
				if (flag2)
				{
					bool placeBlocksAtRange = this._placeBlocksAtRange;
					result = (!placeBlocksAtRange || this.currentCreativeInteractionDistance == this.minimumInteractionDistance);
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x001A89D8 File Offset: 0x001A6BD8
		public bool PlaceBlocksAtRange(GameMode currentMode)
		{
			bool flag = currentMode == 0;
			bool result;
			if (flag)
			{
				result = this.PlaceBlocksAtRangeInAdventureMode;
			}
			else
			{
				result = this._placeBlocksAtRange;
			}
			return result;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x001A8A04 File Offset: 0x001A6C04
		private Settings()
		{
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x001A8D30 File Offset: 0x001A6F30
		public static Settings MakeDefaults()
		{
			Settings settings = new Settings();
			settings.Initialize();
			return settings;
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x001A8D50 File Offset: 0x001A6F50
		public static Settings Load()
		{
			string text;
			try
			{
				text = File.ReadAllText(Settings.SettingsPath, Encoding.UTF8);
			}
			catch (FileNotFoundException)
			{
				return Settings.MakeDefaults();
			}
			catch (Exception exception)
			{
				Settings.Logger.Error(exception, "Failed to load settings:");
				return Settings.MakeDefaults();
			}
			JObject jobject = JObject.Parse(text);
			Settings.MigrateParsedSettings(jobject);
			Settings settings = jobject.ToObject<Settings>(JsonSerializer.Create(Settings.SerializerSettings));
			bool flag = settings.FormatVersion != 3;
			if (flag)
			{
				Settings.Logger.Info<int, int>("Migrated settings from format version {0} to {1}", settings.FormatVersion, 3);
				settings.FormatVersion = 3;
				settings.DidUndergoFormatMigration = true;
			}
			settings.Initialize();
			return settings;
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x001A8E1C File Offset: 0x001A701C
		public static void MigrateParsedSettings(JObject parsedSettings)
		{
			int num = 0;
			JToken jtoken;
			bool flag = parsedSettings.TryGetValue("formatVersion", ref jtoken) || parsedSettings.TryGetValue("FormatVersion", ref jtoken);
			if (flag)
			{
				num = (int)((long)((JValue)jtoken).Value);
			}
			bool flag2 = num > 3;
			if (flag2)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Invalid settings format version, found ",
					num.ToString(),
					" but current is ",
					3.ToString(),
					"."
				}));
			}
			bool flag3 = num == 0;
			if (flag3)
			{
				JToken jtoken2;
				JToken jtoken3;
				bool flag4 = parsedSettings.TryGetValue("inputBindings", ref jtoken2) && ((JObject)jtoken2).TryGetValue("toggleHudVisibility", ref jtoken3);
				if (flag4)
				{
					((JObject)jtoken2).Add("switchHudVisibility", jtoken3);
					((JObject)jtoken2).Remove("toggleHudVisibility");
				}
				num = 1;
			}
			bool flag5 = num == 1;
			if (flag5)
			{
				parsedSettings["VSync"] = parsedSettings["vsync"];
				parsedSettings.Remove("vsync");
				parsedSettings.Remove("DidUndergoFormatMigration");
				parsedSettings = Settings.<MigrateParsedSettings>g__RecursivelyCapitalize|113_0(parsedSettings);
				num = 2;
			}
			bool flag6 = num == 2;
			if (flag6)
			{
				parsedSettings["InputBindings"]["SwitchCameraMode"]["Keycode"] = JToken.FromObject(SDL.SDL_Keycode.SDLK_v);
			}
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x001A8F90 File Offset: 0x001A7190
		private void Initialize()
		{
			bool flag = this.InputBindings == null;
			if (flag)
			{
				this.InputBindings = new InputBindings();
				this.InputBindings.ResetDefaults();
			}
			else
			{
				this.InputBindings.Setup();
			}
			this.AudioSettings.Initialize();
			bool didUndergoFormatMigration = this.DidUndergoFormatMigration;
			if (didUndergoFormatMigration)
			{
				this.Save();
			}
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x001A8FF4 File Offset: 0x001A71F4
		public void Save()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			Settings.Logger.Info("Saving settings...");
			string text = JsonConvert.SerializeObject(this, 1, Settings.SerializerSettings);
			File.WriteAllText(Settings.SettingsPath + ".new", text);
			bool flag = File.Exists(Settings.SettingsPath);
			if (flag)
			{
				File.Replace(Settings.SettingsPath + ".new", Settings.SettingsPath, Settings.SettingsPath + ".bak");
			}
			else
			{
				File.Move(Settings.SettingsPath + ".new", Settings.SettingsPath);
			}
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x001A9094 File Offset: 0x001A7294
		public Settings Clone()
		{
			return new Settings
			{
				PlacementPreviewMode = this.PlacementPreviewMode,
				FormatVersion = this.FormatVersion,
				Fullscreen = this.Fullscreen,
				UseBorderlessForFullscreen = this.UseBorderlessForFullscreen,
				DynamicUIScaling = this.DynamicUIScaling,
				StaticUIScale = this.StaticUIScale,
				Maximized = this.Maximized,
				VSync = this.VSync,
				FpsLimit = this.FpsLimit,
				UnlimitedFps = this.UnlimitedFps,
				ViewDistance = this.ViewDistance,
				FieldOfView = this.FieldOfView,
				SprintFovEffect = this.SprintFovEffect,
				SprintFovIntensity = this.SprintFovIntensity,
				ViewBobbingEffect = this.ViewBobbingEffect,
				ViewBobbingIntensity = this.ViewBobbingIntensity,
				CameraShakeEffect = this.CameraShakeEffect,
				FirstPersonCameraShakeIntensity = this.FirstPersonCameraShakeIntensity,
				ThirdPersonCameraShakeIntensity = this.ThirdPersonCameraShakeIntensity,
				AutomaticRenderScale = this.AutomaticRenderScale,
				RenderScale = this.RenderScale,
				ScreenResolution = this.ScreenResolution,
				MaxChatMessages = this.MaxChatMessages,
				DiagnosticMode = this.DiagnosticMode,
				InputBindings = this.InputBindings.Clone(),
				MouseSettings = this.MouseSettings.Clone(),
				AudioSettings = this.AudioSettings.Clone(),
				Language = this.Language,
				ShortcutSettings = this.ShortcutSettings.Clone(),
				BuilderToolsSettings = this.BuilderToolsSettings.Clone(),
				DebugSettings = this.DebugSettings.Clone(),
				MachinimaEditorSettings = this.MachinimaEditorSettings.Clone(),
				AutoJumpObstacle = this.AutoJumpObstacle,
				AutoJumpGap = this.AutoJumpGap,
				JumpForceSpeedMultiplierStep = this.JumpForceSpeedMultiplierStep,
				MaxJumpForceSpeedMultiplier = this.MaxJumpForceSpeedMultiplier,
				CreativeBlockInteractionDistance = this.CreativeBlockInteractionDistance,
				BuilderMode = this.BuilderMode,
				adventureInteractionDistance = this.adventureInteractionDistance,
				creativeInteractionDistance = this.creativeInteractionDistance,
				CurrentAdventureInteractionDistance = this.CurrentAdventureInteractionDistance,
				CurrentCreativeInteractionDistance = this.CurrentCreativeInteractionDistance,
				Mantling = this.Mantling,
				MinVelocityMantling = this.MinVelocityMantling,
				MaxVelocityMantling = this.MaxVelocityMantling,
				MantlingCameraOffsetY = this.MantlingCameraOffsetY,
				MantleBlockHeight = this.MantleBlockHeight,
				SprintForce = this.SprintForce,
				SprintAccelerationEasingType = this.SprintAccelerationEasingType,
				SprintAccelerationDuration = this.SprintAccelerationDuration,
				SprintDecelerationEasingType = this.SprintDecelerationEasingType,
				SprintDecelerationDuration = this.SprintDecelerationDuration,
				StaminaLowAlertPercent = this.StaminaLowAlertPercent,
				StaminaDebugInfo = this.StaminaDebugInfo,
				WeaponPullback = this.WeaponPullback,
				ItemAnimationsClipGeometry = this.ItemAnimationsClipGeometry,
				ItemsClipGeometry = this.ItemsClipGeometry,
				UseOverrideFirstPersonAnimations = this.UseOverrideFirstPersonAnimations,
				UseNewFlyCamera = this.UseNewFlyCamera,
				EntityUIMaxEntities = this.EntityUIMaxEntities,
				EntityUIMaxDistance = this.EntityUIMaxDistance,
				EntityUIHideDelay = this.EntityUIHideDelay,
				EntityUIFadeInDuration = this.EntityUIFadeInDuration,
				EntityUIFadeOutDuration = this.EntityUIFadeOutDuration,
				SlideDecelerationDuration = this.SlideDecelerationDuration,
				SlideDecelerationEasingType = this.SlideDecelerationEasingType,
				UseBlockSubfaces = this.UseBlockSubfaces,
				DisplayBlockSubfaces = this.DisplayBlockSubfaces,
				DisplayBlockBoundaries = this.DisplayBlockBoundaries,
				_placeBlocksAtRange = this._placeBlocksAtRange,
				PlaceBlocksAtRangeInAdventureMode = this.PlaceBlocksAtRangeInAdventureMode,
				BlockPlacementSupportValidation = this.BlockPlacementSupportValidation,
				ResetMouseSensitivityDuration = this.ResetMouseSensitivityDuration,
				PercentageOfPlaySelectionLengthGizmoShouldRender = this.PercentageOfPlaySelectionLengthGizmoShouldRender,
				MinPlaySelectGizmoSize = this.MinPlaySelectGizmoSize,
				MaxPlaySelectGizmoSize = this.MaxPlaySelectGizmoSize,
				EnableBrushSpacing = this.EnableBrushSpacing,
				BrushSpacingBlocks = this.BrushSpacingBlocks,
				PaintOperationsIgnoreHistoryLength = this.PaintOperationsIgnoreHistoryLength,
				MountMinTurnRate = this.MountMinTurnRate,
				MountMaxTurnRate = this.MountMaxTurnRate,
				MountSpeedMinTurnRate = this.MountSpeedMinTurnRate,
				MountSpeedMaxTurnRate = this.MountSpeedMaxTurnRate,
				MountForwardsAccelerationEasingType = this.MountForwardsAccelerationEasingType,
				MountForwardsDecelerationEasingType = this.MountForwardsDecelerationEasingType,
				MountBackwardsAccelerationEasingType = this.MountBackwardsAccelerationEasingType,
				MountBackwardsDecelerationEasingType = this.MountBackwardsDecelerationEasingType,
				MountForwardsAccelerationDuration = this.MountForwardsAccelerationDuration,
				MountForwardsDecelerationDuration = this.MountForwardsDecelerationDuration,
				MountBackwardsAccelerationDuration = this.MountBackwardsAccelerationDuration,
				MountBackwardsDecelerationDuration = this.MountBackwardsDecelerationDuration,
				MountForcedAccelerationMultiplier = this.MountForcedAccelerationMultiplier,
				MountForcedDecelerationMultiplier = this.MountForcedDecelerationMultiplier,
				MountRequireNewInput = this.MountRequireNewInput
			};
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x001A9594 File Offset: 0x001A7794
		[CompilerGenerated]
		internal static JObject <MigrateParsedSettings>g__RecursivelyCapitalize|113_0(JObject obj)
		{
			JObject jobject = new JObject();
			foreach (KeyValuePair<string, JToken> keyValuePair in obj)
			{
				string text = keyValuePair.Key[0].ToString().ToUpper() + keyValuePair.Key.Substring(1);
				JToken jtoken = keyValuePair.Value;
				bool flag = jtoken is JObject;
				if (flag)
				{
					jtoken = Settings.<MigrateParsedSettings>g__RecursivelyCapitalize|113_0((JObject)jtoken);
				}
				jobject[text] = jtoken;
			}
			return jobject;
		}

		// Token: 0x04003575 RID: 13685
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003576 RID: 13686
		public const int CurrentFormatVersion = 3;

		// Token: 0x04003577 RID: 13687
		public int FormatVersion = 3;

		// Token: 0x04003578 RID: 13688
		[JsonIgnore]
		public bool DidUndergoFormatMigration;

		// Token: 0x04003579 RID: 13689
		public bool Fullscreen = false;

		// Token: 0x0400357A RID: 13690
		public bool UseBorderlessForFullscreen = false;

		// Token: 0x0400357B RID: 13691
		public bool DynamicUIScaling = true;

		// Token: 0x0400357C RID: 13692
		public int StaticUIScale = 100;

		// Token: 0x0400357D RID: 13693
		public bool Maximized = false;

		// Token: 0x0400357E RID: 13694
		public bool VSync = true;

		// Token: 0x0400357F RID: 13695
		public int FpsLimit = 240;

		// Token: 0x04003580 RID: 13696
		public bool UnlimitedFps = false;

		// Token: 0x04003581 RID: 13697
		public int ViewDistance = 192;

		// Token: 0x04003582 RID: 13698
		public int FieldOfView = 70;

		// Token: 0x04003583 RID: 13699
		public bool AutomaticRenderScale = true;

		// Token: 0x04003584 RID: 13700
		public int RenderScale = 100;

		// Token: 0x04003585 RID: 13701
		public int MaxChatMessages = 64;

		// Token: 0x04003586 RID: 13702
		public BlockPlacementPreview.DisplayMode PlacementPreviewMode = BlockPlacementPreview.DisplayMode.Multipart;

		// Token: 0x04003587 RID: 13703
		public ScreenResolution ScreenResolution = ScreenResolutions.DefaultScreenResolution;

		// Token: 0x04003588 RID: 13704
		public bool DiagnosticMode = true;

		// Token: 0x04003589 RID: 13705
		public InputBindings InputBindings;

		// Token: 0x0400358A RID: 13706
		public MouseSettings MouseSettings = new MouseSettings();

		// Token: 0x0400358B RID: 13707
		public AudioSettings AudioSettings = new AudioSettings();

		// Token: 0x0400358C RID: 13708
		public MachinimaEditorSettings MachinimaEditorSettings = new MachinimaEditorSettings();

		// Token: 0x0400358D RID: 13709
		public BuilderToolsSettings BuilderToolsSettings = new BuilderToolsSettings();

		// Token: 0x0400358E RID: 13710
		public DebugSettings DebugSettings = new DebugSettings();

		// Token: 0x0400358F RID: 13711
		public string Language;

		// Token: 0x04003590 RID: 13712
		public ShortcutSettings ShortcutSettings = new ShortcutSettings();

		// Token: 0x04003591 RID: 13713
		public int SavedCameraIndex = 0;

		// Token: 0x04003592 RID: 13714
		public bool AutoJumpObstacle = true;

		// Token: 0x04003593 RID: 13715
		public bool AutoJumpGap;

		// Token: 0x04003594 RID: 13716
		public float JumpForceSpeedMultiplierStep = 4f;

		// Token: 0x04003595 RID: 13717
		public float MaxJumpForceSpeedMultiplier = 30f;

		// Token: 0x04003596 RID: 13718
		public int CreativeBlockInteractionDistance = 5;

		// Token: 0x04003597 RID: 13719
		public bool BlockHealth;

		// Token: 0x04003598 RID: 13720
		public bool BuilderMode;

		// Token: 0x04003599 RID: 13721
		public bool UseBlockSubfaces = true;

		// Token: 0x0400359A RID: 13722
		public bool DisplayBlockSubfaces = true;

		// Token: 0x0400359B RID: 13723
		public bool DisplayBlockBoundaries = false;

		// Token: 0x0400359C RID: 13724
		private int minimumInteractionDistance = 0;

		// Token: 0x0400359D RID: 13725
		public int adventureInteractionDistance = 5;

		// Token: 0x0400359E RID: 13726
		private int currentAdventureInteractionDistance = 5;

		// Token: 0x0400359F RID: 13727
		public int creativeInteractionDistance = 30;

		// Token: 0x040035A0 RID: 13728
		private int currentCreativeInteractionDistance = 25;

		// Token: 0x040035A1 RID: 13729
		public bool BlockPlacementSupportValidation = true;

		// Token: 0x040035A2 RID: 13730
		public float ResetMouseSensitivityDuration = 1f;

		// Token: 0x040035A3 RID: 13731
		public bool _placeBlocksAtRange = true;

		// Token: 0x040035A4 RID: 13732
		public bool PlaceBlocksAtRangeInAdventureMode = false;

		// Token: 0x040035A5 RID: 13733
		public float PercentageOfPlaySelectionLengthGizmoShouldRender = 0.5f;

		// Token: 0x040035A6 RID: 13734
		public int MinPlaySelectGizmoSize = 1;

		// Token: 0x040035A7 RID: 13735
		public int MaxPlaySelectGizmoSize = 25;

		// Token: 0x040035A8 RID: 13736
		public bool Mantling = true;

		// Token: 0x040035A9 RID: 13737
		public float MinVelocityMantling = -16f;

		// Token: 0x040035AA RID: 13738
		public float MaxVelocityMantling = 6f;

		// Token: 0x040035AB RID: 13739
		public float MantlingCameraOffsetY = -1.6f;

		// Token: 0x040035AC RID: 13740
		public float MantleBlockHeight = 0.8f;

		// Token: 0x040035AD RID: 13741
		public bool SprintFovEffect = true;

		// Token: 0x040035AE RID: 13742
		public float SprintFovIntensity = 1.175f;

		// Token: 0x040035AF RID: 13743
		public bool ViewBobbingEffect = true;

		// Token: 0x040035B0 RID: 13744
		public float ViewBobbingIntensity = 1f;

		// Token: 0x040035B1 RID: 13745
		public bool CameraShakeEffect = true;

		// Token: 0x040035B2 RID: 13746
		public float FirstPersonCameraShakeIntensity = 1f;

		// Token: 0x040035B3 RID: 13747
		public float ThirdPersonCameraShakeIntensity = 1f;

		// Token: 0x040035B4 RID: 13748
		public bool SprintForce = true;

		// Token: 0x040035B5 RID: 13749
		public Easing.EasingType SprintAccelerationEasingType = Easing.EasingType.QuadInOut;

		// Token: 0x040035B6 RID: 13750
		public float SprintAccelerationDuration = 1.5f;

		// Token: 0x040035B7 RID: 13751
		public Easing.EasingType SprintDecelerationEasingType = Easing.EasingType.CubicOut;

		// Token: 0x040035B8 RID: 13752
		public float SprintDecelerationDuration = 1.5f;

		// Token: 0x040035B9 RID: 13753
		public bool UseNewFlyCamera = true;

		// Token: 0x040035BA RID: 13754
		public Easing.EasingType SlideDecelerationEasingType = Easing.EasingType.QuintIn;

		// Token: 0x040035BB RID: 13755
		public float SlideDecelerationDuration = 0.9f;

		// Token: 0x040035BC RID: 13756
		public int StaminaLowAlertPercent = 20;

		// Token: 0x040035BD RID: 13757
		public bool StaminaDebugInfo = false;

		// Token: 0x040035BE RID: 13758
		public bool WeaponPullback = true;

		// Token: 0x040035BF RID: 13759
		public bool ItemAnimationsClipGeometry = false;

		// Token: 0x040035C0 RID: 13760
		public bool ItemsClipGeometry = false;

		// Token: 0x040035C1 RID: 13761
		public bool UseOverrideFirstPersonAnimations = false;

		// Token: 0x040035C2 RID: 13762
		public int EntityUIMaxEntities = 8;

		// Token: 0x040035C3 RID: 13763
		public float EntityUIMaxDistance = 50f;

		// Token: 0x040035C4 RID: 13764
		public float EntityUIHideDelay = 1f;

		// Token: 0x040035C5 RID: 13765
		public float EntityUIFadeInDuration = 0.15f;

		// Token: 0x040035C6 RID: 13766
		public float EntityUIFadeOutDuration = 1f;

		// Token: 0x040035C7 RID: 13767
		public int PaintOperationsIgnoreHistoryLength = 5;

		// Token: 0x040035C8 RID: 13768
		public int BrushSpacingBlocks = 100;

		// Token: 0x040035C9 RID: 13769
		public bool EnableBrushSpacing = false;

		// Token: 0x040035CA RID: 13770
		public float MountMinTurnRate = 180f;

		// Token: 0x040035CB RID: 13771
		public float MountMaxTurnRate = 60f;

		// Token: 0x040035CC RID: 13772
		public float MountSpeedMinTurnRate;

		// Token: 0x040035CD RID: 13773
		public float MountSpeedMaxTurnRate = 12f;

		// Token: 0x040035CE RID: 13774
		public Easing.EasingType MountForwardsAccelerationEasingType = Easing.EasingType.CircOut;

		// Token: 0x040035CF RID: 13775
		public Easing.EasingType MountForwardsDecelerationEasingType = Easing.EasingType.QuadInOut;

		// Token: 0x040035D0 RID: 13776
		public Easing.EasingType MountBackwardsAccelerationEasingType = Easing.EasingType.CircOut;

		// Token: 0x040035D1 RID: 13777
		public Easing.EasingType MountBackwardsDecelerationEasingType = Easing.EasingType.CircOut;

		// Token: 0x040035D2 RID: 13778
		public float MountForwardsAccelerationDuration = 3f;

		// Token: 0x040035D3 RID: 13779
		public float MountForwardsDecelerationDuration = 3f;

		// Token: 0x040035D4 RID: 13780
		public float MountBackwardsAccelerationDuration = 0.2f;

		// Token: 0x040035D5 RID: 13781
		public float MountBackwardsDecelerationDuration = 2f;

		// Token: 0x040035D6 RID: 13782
		public float MountForcedAccelerationMultiplier = 2f;

		// Token: 0x040035D7 RID: 13783
		public float MountForcedDecelerationMultiplier = 2f;

		// Token: 0x040035D8 RID: 13784
		public bool MountRequireNewInput;

		// Token: 0x040035D9 RID: 13785
		private static readonly string SettingsPath = Path.Combine(Paths.UserData, "Settings.json");

		// Token: 0x040035DA RID: 13786
		private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			Converters = 
			{
				new InputBindingJsonConverter(),
				new ShortcutSettingsJsonConverter()
			}
		};
	}
}
