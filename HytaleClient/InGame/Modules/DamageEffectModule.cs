using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.EntityStats;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008EE RID: 2286
	internal class DamageEffectModule : Module
	{
		// Token: 0x06004391 RID: 17297 RVA: 0x000D76B0 File Offset: 0x000D58B0
		public DamageEffectModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._damageScreenEffectRenderer = new ScreenEffectRenderer(this._gameInstance.Engine);
			this._healthAlertScreenEffectRenderer = new ScreenEffectRenderer(this._gameInstance.Engine);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x000D77B0 File Offset: 0x000D59B0
		public override void Initialize()
		{
			this.InitDamageImages();
			this.InitAnimationDatas();
			this.InitSoundDatas();
			BasicProgram basicProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
			this._quadRenderer = new QuadRenderer(this._gameInstance.Engine.Graphics, basicProgram.AttribPosition, basicProgram.AttribTexCoords);
			this.Resize(this._gameInstance.Engine.Window.Viewport.Width, this._gameInstance.Engine.Window.Viewport.Height);
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x000D7850 File Offset: 0x000D5A50
		protected override void DoDispose()
		{
			QuadRenderer quadRenderer = this._quadRenderer;
			if (quadRenderer != null)
			{
				quadRenderer.Dispose();
			}
			this._damageScreenEffectRenderer.Dispose();
			this._healthAlertScreenEffectRenderer.Dispose();
			this._gameInstance.Engine.Audio.SetRTPC(this._playerHealthRTPCId, 100f);
			this._gameInstance.Engine.Audio.SetRTPC(this._playerStaminaRTPCId, 100f);
			this._gameInstance.Engine.Audio.SetRTPC(this._playerSignatureRTPCId, 100f);
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x000D78EC File Offset: 0x000D5AEC
		private void InitDamageImages()
		{
			string hash;
			bool flag = this._gameInstance.HashesByServerAssetPath.TryGetValue("UI/DamageIndicators/HitIndicatorBasic.png", ref hash);
			if (flag)
			{
				try
				{
					this._damageIndicatorImages.Add("UI/DamageIndicators/HitIndicatorBasic.png", this.CreateDamageSpriteInfo(hash));
				}
				catch (Exception exception)
				{
					DamageEffectModule.Logger.Error(exception, "Failed to load damage texture: UI/DamageIndicators/HitIndicatorBasic.png");
				}
			}
			else
			{
				this._gameInstance.App.DevTools.Error("Missing damage indicator asset: UI/DamageIndicators/HitIndicatorBasic.png");
			}
			string hash2;
			bool flag2 = this._gameInstance.HashesByServerAssetPath.TryGetValue("UI/DamageIndicators/HitIndicatorCritic.png", ref hash2);
			if (flag2)
			{
				try
				{
					this._damageIndicatorImages.Add("UI/DamageIndicators/HitIndicatorCritic.png", this.CreateDamageSpriteInfo(hash2));
				}
				catch (Exception exception2)
				{
					DamageEffectModule.Logger.Error(exception2, "Failed to load damage texture: UI/DamageIndicators/HitIndicatorCritic.png");
				}
			}
			else
			{
				this._gameInstance.App.DevTools.Error("Missing damage indicator asset: UI/DamageIndicators/HitIndicatorCritic.png");
			}
			string hash3;
			bool flag3 = this._gameInstance.HashesByServerAssetPath.TryGetValue("UI/DamageIndicators/HitIndicatorMelee.png", ref hash3);
			if (flag3)
			{
				try
				{
					this._damageIndicatorImages.Add("UI/DamageIndicators/HitIndicatorMelee.png", this.CreateDamageSpriteInfo(hash3));
				}
				catch (Exception exception3)
				{
					DamageEffectModule.Logger.Error(exception3, "Failed to load damage texture: UI/DamageIndicators/HitIndicatorMelee.png");
				}
			}
			else
			{
				this._gameInstance.App.DevTools.Error("Missing damage indicator asset: UI/DamageIndicators/HitIndicatorMelee.png");
			}
			this._damageScreenEffectRenderer.Initialize();
			string targetScreenEffectTextureChecksum;
			bool flag4 = this._gameInstance.HashesByServerAssetPath.TryGetValue("UI/DamageScreenEffect.png", ref targetScreenEffectTextureChecksum);
			if (flag4)
			{
				this._damageScreenEffectRenderer.RequestTextureUpdate(targetScreenEffectTextureChecksum, false);
				this._damageScreenEffectRenderer.Color = new Vector4(1f, 1f, 1f, 0f);
			}
			else
			{
				this._gameInstance.App.DevTools.Error("Missing damage asset: UI/DamageScreenEffect.png");
			}
			this._healthAlertScreenEffectRenderer.Initialize();
			string targetScreenEffectTextureChecksum2;
			bool flag5 = this._gameInstance.HashesByServerAssetPath.TryGetValue("UI/HealthAlertScreenEffect.png", ref targetScreenEffectTextureChecksum2);
			if (flag5)
			{
				this._healthAlertScreenEffectRenderer.RequestTextureUpdate(targetScreenEffectTextureChecksum2, false);
				this._healthAlertScreenEffectRenderer.Color = new Vector4(1f, 1f, 1f, 0f);
			}
			else
			{
				this._gameInstance.App.DevTools.Error("Missing damage asset: UI/HealthAlertScreenEffect.png");
			}
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x000D7B74 File Offset: 0x000D5D74
		private DamageEffectModule.DamageSpriteInfo CreateDamageSpriteInfo(string hash)
		{
			Image image = new Image(AssetManager.GetAssetUsingHash(hash, false));
			DamageEffectModule.DamageSpriteInfo result;
			result.Texture = this.CreateTexture(image);
			result.Size = image.Width;
			return result;
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x000D7BB0 File Offset: 0x000D5DB0
		private unsafe GLTexture CreateTexture(Image sprite)
		{
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			GLTexture gltexture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, gltexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 10497);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 10497);
			byte[] array;
			byte* value;
			if ((array = sprite.Pixels) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, sprite.Width, sprite.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			array = null;
			return gltexture;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x000D7CB4 File Offset: 0x000D5EB4
		private void InitAnimationDatas()
		{
			this._animationDatas.Add("Physical", new DamageEffectModule.AnimationData
			{
				IndicatorDuration = 0.6f,
				MinAlpha = 0f,
				MaxAlpha = 1f,
				MinScaleFactor = 1f,
				MaxScaleFactor = 1.75f,
				ScaleDuration = 0.3f,
				MinRadius = 40f,
				MaxRadius = 90f,
				RadiusDuration = 0.3f
			});
			this._animationDatas.Add("Projectile", new DamageEffectModule.AnimationData
			{
				IndicatorDuration = 0.6f,
				MinAlpha = 0f,
				MaxAlpha = 1f,
				MinScaleFactor = 1f,
				MaxScaleFactor = 1.75f,
				ScaleDuration = 0.3f,
				MinRadius = 70f,
				MaxRadius = 120f,
				RadiusDuration = 0.3f
			});
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x000D7DD0 File Offset: 0x000D5FD0
		private void InitSoundDatas()
		{
			bool flag = !this._gameInstance.Engine.Audio.ResourceManager.WwiseGameParameterIds.TryGetValue("HEALTH", out this._playerHealthRTPCId);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Missing health RTPC: HEALTH");
			}
			bool flag2 = !this._gameInstance.Engine.Audio.ResourceManager.WwiseGameParameterIds.TryGetValue("STAMINA", out this._playerStaminaRTPCId);
			if (flag2)
			{
				this._gameInstance.App.DevTools.Error("Missing stamina RTPC: STAMINA");
			}
			bool flag3 = !this._gameInstance.Engine.Audio.ResourceManager.WwiseGameParameterIds.TryGetValue("SIGNATURE", out this._playerSignatureRTPCId);
			if (flag3)
			{
				this._gameInstance.App.DevTools.Error("Missing signature energy RTPC: SIGNATURE");
			}
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x000D7EC8 File Offset: 0x000D60C8
		public void Resize(int width, int height)
		{
			this._projectionMatrix = Matrix.CreateTranslation(0f, 0f, -1f) * Matrix.CreateOrthographicOffCenter((float)(-(float)width) / 2f, (float)width / 2f, (float)(-(float)height) / 2f, (float)height / 2f, 0.1f, 1000f);
			this._windowScale = (float)this._gameInstance.Engine.Window.Viewport.Height / 1080f;
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x000D7F50 File Offset: 0x000D6150
		public void Update(float deltaTime)
		{
			this.UpdateDamageIndicators(deltaTime);
			float num = 1f;
			ClientEntityStatValue entityStat = this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.Health);
			bool flag = entityStat != null;
			if (flag)
			{
				num = entityStat.Value / entityStat.Max;
			}
			bool flag2 = num != this._previousHealthRatio;
			if (flag2)
			{
				this.HandleHeartbeatSound(num);
			}
			this.UpdateFullScreenDamageEffects(num, deltaTime);
			this._previousHealthRatio = num;
			float num2 = 1f;
			ClientEntityStatValue entityStat2 = this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.Stamina);
			bool flag3 = entityStat2 != null;
			if (flag3)
			{
				num2 = entityStat2.Value / entityStat2.Max;
			}
			bool flag4 = num2 != this._previousStaminaRatio;
			if (flag4)
			{
				this.HandleStaminaSound(num2);
			}
			this._previousStaminaRatio = num2;
			float num3 = 1f;
			ClientEntityStatValue entityStat3 = this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.SignatureEnergy);
			bool flag5 = entityStat3 != null;
			if (flag5)
			{
				num3 = entityStat3.Value / entityStat3.Max;
			}
			bool flag6 = num3 != this._previousSignatureRatio;
			if (flag6)
			{
				this.HandleSignatureSound(num3);
			}
			this._previousSignatureRatio = num3;
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x000D8078 File Offset: 0x000D6278
		private void UpdateDamageIndicators(float deltaTime)
		{
			for (int i = this._damageIndicators.Count - 1; i >= 0; i--)
			{
				DamageEffectModule.DamageIndicator damageIndicator = this._damageIndicators[i];
				damageIndicator.Update(deltaTime, this._gameInstance.LocalPlayer.Position, this._gameInstance.CameraModule.Controller.Rotation.Yaw);
				bool flag = damageIndicator.IsFinished();
				if (flag)
				{
					this._damageIndicators.Remove(damageIndicator);
				}
			}
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x000D8104 File Offset: 0x000D6304
		private void HandleHeartbeatSound(float currentHealthRatio)
		{
			int num = (int)(currentHealthRatio * 100f);
			bool flag = num != this._previousHealthRTPCValue;
			if (flag)
			{
				this._gameInstance.Engine.Audio.SetRTPC(this._playerHealthRTPCId, (float)num);
				this._previousHealthRTPCValue = num;
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x000D8154 File Offset: 0x000D6354
		private void HandleStaminaSound(float currentStaminaRatio)
		{
			int num = (int)(currentStaminaRatio * 100f);
			bool flag = num != this._previousStaminaRTPCValue;
			if (flag)
			{
				this._gameInstance.Engine.Audio.SetRTPC(this._playerStaminaRTPCId, (float)num);
				this._previousStaminaRTPCValue = num;
			}
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x000D81A4 File Offset: 0x000D63A4
		private void HandleSignatureSound(float currentSignatureRatio)
		{
			int num = (int)(currentSignatureRatio * 100f);
			bool flag = num != this._previousSignatureRTPCValue;
			if (flag)
			{
				this._gameInstance.Engine.Audio.SetRTPC(this._playerSignatureRTPCId, (float)num);
				this._previousSignatureRTPCValue = num;
			}
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x000D81F4 File Offset: 0x000D63F4
		private float ConvertToNewRange(float value, float oldMinRange, float oldMaxRange, float newMinRange, float newMaxRange)
		{
			return (value - oldMinRange) * (newMaxRange - newMinRange) / (oldMaxRange - oldMinRange) + newMinRange;
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x000D8218 File Offset: 0x000D6418
		private void UpdateFullScreenDamageEffects(float currentHealthRatio, float deltaTime)
		{
			bool flag = this._damageScreenEffectRenderer.Color.W > 0f;
			if (flag)
			{
				this._gameInstance.ScreenEffectStoreModule.RequestDraw(this._damageScreenEffectRenderer);
				this._damageScreenEffectRenderer.Color.W = MathHelper.Max(this._damageScreenEffectRenderer.Color.W - deltaTime * 0.4f, 0f);
			}
			bool flag2 = currentHealthRatio <= this.HealthAlertThreshold;
			if (flag2)
			{
				float num = this.ConvertToNewRange(currentHealthRatio, this.HealthAlertThreshold, 0f, this.MinAlphaHealthBorder, this.MaxAlphaHealthBorder);
				this._healthBorderTimeElapsed += deltaTime;
				float num2 = this._healthBorderTimeElapsed * 3.1415927f;
				float newMaxRange = this.ConvertToNewRange(currentHealthRatio, this.HealthAlertThreshold, 0f, this.MinVarianceHealthBorder, this.MaxVarianceHealthBorder);
				float num3 = this.ConvertToNewRange((float)Math.Sin((double)num2), 0f, 1f, 0f, newMaxRange);
				this._healthAlertScreenEffectRenderer.Color.W = MathHelper.Lerp(this._healthAlertScreenEffectRenderer.Color.W, num + num3, this.LerpSpeedHealthBorder);
				this._gameInstance.ScreenEffectStoreModule.RequestDraw(this._healthAlertScreenEffectRenderer);
			}
			else
			{
				bool flag3 = this._healthAlertScreenEffectRenderer.Color.W > 0f;
				if (flag3)
				{
					bool flag4 = this._healthAlertScreenEffectRenderer.Color.W <= 0.01f;
					if (flag4)
					{
						this._healthAlertScreenEffectRenderer.Color.W = 0f;
					}
					else
					{
						this._healthAlertScreenEffectRenderer.Color.W = MathHelper.Lerp(this._healthAlertScreenEffectRenderer.Color.W, 0f, this.ResetSpeedHealthBorder);
					}
					this._gameInstance.ScreenEffectStoreModule.RequestDraw(this._healthAlertScreenEffectRenderer);
				}
			}
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x000D8408 File Offset: 0x000D6608
		public void PrepareForDraw()
		{
			ArrayUtils.GrowArrayIfNecessary<DamageEffectModule.DamageDrawTask>(ref this._damageDrawTasks, this._damageIndicators.Count, 5);
			int num = 0;
			for (int i = 0; i < this._damageIndicators.Count; i++)
			{
				DamageEffectModule.DamageIndicator damageIndicator = this._damageIndicators[i];
				bool flag = this.AngleHideDamage != 0 && Math.Abs(MathHelper.ToDegrees(damageIndicator.Angle)) < (float)this.AngleHideDamage / 2f;
				if (!flag)
				{
					DamageEffectModule.DamageSpriteInfo damageSpriteInfo;
					bool flag2 = !this._damageIndicatorImages.TryGetValue(damageIndicator.SpriteToDisplay, out damageSpriteInfo);
					if (!flag2)
					{
						int num2 = num;
						this._damageDrawTasks[num2].Texture = damageSpriteInfo.Texture;
						this._damageDrawTasks[num2].Alpha = damageIndicator.Alpha;
						this.CalculateMatrix(num2, (float)damageSpriteInfo.Size, damageIndicator);
						num++;
					}
				}
			}
			this._damageDrawTasksCount = num;
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x000D8500 File Offset: 0x000D6700
		private void CalculateMatrix(int taskId, float imageSize, DamageEffectModule.DamageIndicator damageEffect)
		{
			float num = imageSize * damageEffect.ScaleFactor * this._windowScale;
			Matrix matrix;
			Matrix.CreateScale(num, num, 1f, out matrix);
			float num2 = num / 2f;
			Matrix.CreateTranslation(-num2, -num2, 0f, out this._damageDrawTasks[taskId].MVPMatrix);
			Matrix.Multiply(ref matrix, ref this._damageDrawTasks[taskId].MVPMatrix, out this._damageDrawTasks[taskId].MVPMatrix);
			Matrix.CreateRotationZ(damageEffect.Angle, out matrix);
			Matrix.Multiply(ref this._damageDrawTasks[taskId].MVPMatrix, ref matrix, out this._damageDrawTasks[taskId].MVPMatrix);
			Matrix.CreateTranslation(num2, num2, 0f, out matrix);
			Matrix.Multiply(ref this._damageDrawTasks[taskId].MVPMatrix, ref matrix, out this._damageDrawTasks[taskId].MVPMatrix);
			Matrix.CreateTranslation(-num2 - damageEffect.OffsetSpritePosition.X * this._windowScale, -num2 - damageEffect.OffsetSpritePosition.Y * this._windowScale, 0f, out matrix);
			Matrix.Multiply(ref this._damageDrawTasks[taskId].MVPMatrix, ref matrix, out this._damageDrawTasks[taskId].MVPMatrix);
			Matrix.Multiply(ref this._damageDrawTasks[taskId].MVPMatrix, ref this._projectionMatrix, out this._damageDrawTasks[taskId].MVPMatrix);
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x000D867F File Offset: 0x000D687F
		public bool NeedsDrawing()
		{
			return this._damageDrawTasksCount > 0;
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x000D868C File Offset: 0x000D688C
		public void Draw()
		{
			BasicProgram basicProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("DrawDamageEffects called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			basicProgram.AssertInUse();
			basicProgram.Color.AssertValue(this._gameInstance.Engine.Graphics.WhiteColor);
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			for (int i = 0; i < this._damageDrawTasksCount; i++)
			{
				gl.BindTexture(GL.TEXTURE_2D, this._damageDrawTasks[i].Texture);
				basicProgram.Opacity.SetValue(this._damageDrawTasks[i].Alpha);
				basicProgram.MVPMatrix.SetValue(ref this._damageDrawTasks[i].MVPMatrix);
				this._quadRenderer.Draw();
			}
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x000D8788 File Offset: 0x000D6988
		public void AddDamageEffect(Vector3d damageSourcePosition, float damageAmount, DamageCause damageCause)
		{
			DamageEffectModule.AnimationData animationData;
			bool flag = this._animationDatas.TryGetValue(damageCause.Id, out animationData);
			if (flag)
			{
				this._damageIndicators.Add(new DamageEffectModule.DamageIndicator(damageSourcePosition, this.GetSpritePathToDisplay(damageAmount, damageCause), animationData));
			}
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x000D87CC File Offset: 0x000D69CC
		private string GetSpritePathToDisplay(float damageAmount, DamageCause damageCause)
		{
			bool flag = damageCause.Id.Equals("Projectile");
			if (flag)
			{
				ClientEntityStatValue entityStat = this._gameInstance.LocalPlayer.GetEntityStat(DefaultEntityStats.Health);
				bool flag2 = entityStat != null;
				if (flag2)
				{
					return (damageAmount / entityStat.Max > 0.2f) ? "UI/DamageIndicators/HitIndicatorCritic.png" : "UI/DamageIndicators/HitIndicatorBasic.png";
				}
			}
			return "UI/DamageIndicators/HitIndicatorMelee.png";
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x000D8836 File Offset: 0x000D6A36
		public void IncreaseDamageEffect(float alpha)
		{
			this._damageScreenEffectRenderer.Color.W = MathHelper.Min(this._damageScreenEffectRenderer.Color.W + alpha, 1f);
		}

		// Token: 0x0400214E RID: 8526
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400214F RID: 8527
		private const int DamageDrawTasksDefaultSize = 10;

		// Token: 0x04002150 RID: 8528
		private const int DamageDrawTasksGrowth = 5;

		// Token: 0x04002151 RID: 8529
		private const float MinCriticalRatio = 0.2f;

		// Token: 0x04002152 RID: 8530
		private const float SoundsLerpSpeed = 0.04f;

		// Token: 0x04002153 RID: 8531
		private const string DamageBasicPath = "UI/DamageIndicators/HitIndicatorBasic.png";

		// Token: 0x04002154 RID: 8532
		private const string DamageCriticalPath = "UI/DamageIndicators/HitIndicatorCritic.png";

		// Token: 0x04002155 RID: 8533
		private const string DamageMeleePath = "UI/DamageIndicators/HitIndicatorMelee.png";

		// Token: 0x04002156 RID: 8534
		private const string DamageTexturePath = "UI/DamageScreenEffect.png";

		// Token: 0x04002157 RID: 8535
		private const string HealthAlertTexturePath = "UI/HealthAlertScreenEffect.png";

		// Token: 0x04002158 RID: 8536
		private const string PlayerHealthRTPCName = "HEALTH";

		// Token: 0x04002159 RID: 8537
		private const string PlayerStaminaRTPCName = "STAMINA";

		// Token: 0x0400215A RID: 8538
		private const string PlayerSignatureRTPCName = "SIGNATURE";

		// Token: 0x0400215B RID: 8539
		public int AngleHideDamage = 0;

		// Token: 0x0400215C RID: 8540
		public float HealthAlertThreshold = 0.3f;

		// Token: 0x0400215D RID: 8541
		public float MinAlphaHealthBorder = 0.5f;

		// Token: 0x0400215E RID: 8542
		public float MaxAlphaHealthBorder = 0.85f;

		// Token: 0x0400215F RID: 8543
		public float MinVarianceHealthBorder = 0.08f;

		// Token: 0x04002160 RID: 8544
		public float MaxVarianceHealthBorder = 0.15f;

		// Token: 0x04002161 RID: 8545
		public float LerpSpeedHealthBorder = 0.2f;

		// Token: 0x04002162 RID: 8546
		public float ResetSpeedHealthBorder = 0.05f;

		// Token: 0x04002163 RID: 8547
		private readonly ScreenEffectRenderer _damageScreenEffectRenderer;

		// Token: 0x04002164 RID: 8548
		private readonly ScreenEffectRenderer _healthAlertScreenEffectRenderer;

		// Token: 0x04002165 RID: 8549
		private readonly List<DamageEffectModule.DamageIndicator> _damageIndicators = new List<DamageEffectModule.DamageIndicator>();

		// Token: 0x04002166 RID: 8550
		private readonly Dictionary<string, DamageEffectModule.DamageSpriteInfo> _damageIndicatorImages = new Dictionary<string, DamageEffectModule.DamageSpriteInfo>();

		// Token: 0x04002167 RID: 8551
		private readonly Dictionary<string, DamageEffectModule.AnimationData> _animationDatas = new Dictionary<string, DamageEffectModule.AnimationData>();

		// Token: 0x04002168 RID: 8552
		private Matrix _projectionMatrix;

		// Token: 0x04002169 RID: 8553
		private float _windowScale;

		// Token: 0x0400216A RID: 8554
		private QuadRenderer _quadRenderer;

		// Token: 0x0400216B RID: 8555
		private DamageEffectModule.DamageDrawTask[] _damageDrawTasks = new DamageEffectModule.DamageDrawTask[10];

		// Token: 0x0400216C RID: 8556
		private int _damageDrawTasksCount;

		// Token: 0x0400216D RID: 8557
		private float _healthBorderTimeElapsed;

		// Token: 0x0400216E RID: 8558
		private float _previousHealthRatio = 1f;

		// Token: 0x0400216F RID: 8559
		private float _previousStaminaRatio = 1f;

		// Token: 0x04002170 RID: 8560
		private float _previousSignatureRatio = 1f;

		// Token: 0x04002171 RID: 8561
		private uint _playerHealthRTPCId;

		// Token: 0x04002172 RID: 8562
		private int _previousHealthRTPCValue = 100;

		// Token: 0x04002173 RID: 8563
		private uint _playerStaminaRTPCId;

		// Token: 0x04002174 RID: 8564
		private int _previousStaminaRTPCValue = 100;

		// Token: 0x04002175 RID: 8565
		private uint _playerSignatureRTPCId;

		// Token: 0x04002176 RID: 8566
		private int _previousSignatureRTPCValue = 100;

		// Token: 0x02000DB5 RID: 3509
		private struct DamageDrawTask
		{
			// Token: 0x0400438D RID: 17293
			public GLTexture Texture;

			// Token: 0x0400438E RID: 17294
			public Matrix MVPMatrix;

			// Token: 0x0400438F RID: 17295
			public float Alpha;
		}

		// Token: 0x02000DB6 RID: 3510
		private struct DamageSpriteInfo
		{
			// Token: 0x04004390 RID: 17296
			public GLTexture Texture;

			// Token: 0x04004391 RID: 17297
			public int Size;
		}

		// Token: 0x02000DB7 RID: 3511
		private struct AnimationData
		{
			// Token: 0x0600661B RID: 26139 RVA: 0x00212CC8 File Offset: 0x00210EC8
			public AnimationData(float indicatorDuration, float minAlpha, float maxAlpha, float minScaleFactor, float maxScaleFactor, float scaleDuration, float minRadius, float maxRadius, float radiusDuration)
			{
				this.IndicatorDuration = indicatorDuration;
				this.MinAlpha = minAlpha;
				this.MaxAlpha = maxAlpha;
				this.MinScaleFactor = minScaleFactor;
				this.MaxScaleFactor = maxScaleFactor;
				this.ScaleDuration = scaleDuration;
				this.MinRadius = minRadius;
				this.MaxRadius = maxRadius;
				this.RadiusDuration = radiusDuration;
			}

			// Token: 0x04004392 RID: 17298
			public float IndicatorDuration;

			// Token: 0x04004393 RID: 17299
			public float MinAlpha;

			// Token: 0x04004394 RID: 17300
			public float MaxAlpha;

			// Token: 0x04004395 RID: 17301
			public float MinScaleFactor;

			// Token: 0x04004396 RID: 17302
			public float MaxScaleFactor;

			// Token: 0x04004397 RID: 17303
			public float ScaleDuration;

			// Token: 0x04004398 RID: 17304
			public float MinRadius;

			// Token: 0x04004399 RID: 17305
			public float MaxRadius;

			// Token: 0x0400439A RID: 17306
			public float RadiusDuration;
		}

		// Token: 0x02000DB8 RID: 3512
		private class DamageIndicator
		{
			// Token: 0x17001453 RID: 5203
			// (get) Token: 0x0600661C RID: 26140 RVA: 0x00212D1B File Offset: 0x00210F1B
			// (set) Token: 0x0600661D RID: 26141 RVA: 0x00212D23 File Offset: 0x00210F23
			public string SpriteToDisplay { get; private set; }

			// Token: 0x17001454 RID: 5204
			// (get) Token: 0x0600661E RID: 26142 RVA: 0x00212D2C File Offset: 0x00210F2C
			// (set) Token: 0x0600661F RID: 26143 RVA: 0x00212D34 File Offset: 0x00210F34
			public float Angle { get; private set; }

			// Token: 0x17001455 RID: 5205
			// (get) Token: 0x06006620 RID: 26144 RVA: 0x00212D3D File Offset: 0x00210F3D
			// (set) Token: 0x06006621 RID: 26145 RVA: 0x00212D45 File Offset: 0x00210F45
			public Vector2 OffsetSpritePosition { get; private set; }

			// Token: 0x17001456 RID: 5206
			// (get) Token: 0x06006622 RID: 26146 RVA: 0x00212D4E File Offset: 0x00210F4E
			// (set) Token: 0x06006623 RID: 26147 RVA: 0x00212D56 File Offset: 0x00210F56
			public float Alpha { get; private set; }

			// Token: 0x17001457 RID: 5207
			// (get) Token: 0x06006624 RID: 26148 RVA: 0x00212D5F File Offset: 0x00210F5F
			// (set) Token: 0x06006625 RID: 26149 RVA: 0x00212D67 File Offset: 0x00210F67
			public float ScaleFactor { get; private set; }

			// Token: 0x06006626 RID: 26150 RVA: 0x00212D70 File Offset: 0x00210F70
			public DamageIndicator(Vector3d damageSourcePosition, string spriteToDisplay, DamageEffectModule.AnimationData animationData)
			{
				this._damageSourcePosition = new Vector3((float)damageSourcePosition.X, (float)damageSourcePosition.Y, (float)damageSourcePosition.Z);
				this.SpriteToDisplay = spriteToDisplay;
				this._animationData = animationData;
			}

			// Token: 0x06006627 RID: 26151 RVA: 0x00212DBF File Offset: 0x00210FBF
			public bool IsFinished()
			{
				return this._timeElapsed >= this._animationData.IndicatorDuration;
			}

			// Token: 0x06006628 RID: 26152 RVA: 0x00212DD8 File Offset: 0x00210FD8
			public void Update(float deltaTime, Vector3 playerPosition, float yaw)
			{
				this._timeElapsed += deltaTime;
				bool flag = this._timeElapsed >= this._animationData.IndicatorDuration;
				if (!flag)
				{
					Vector3 vector = new Vector3((float)Math.Sin((double)yaw), 0f, (float)Math.Cos((double)yaw));
					Vector3 vector2 = playerPosition - this._damageSourcePosition;
					vector2.Normalize();
					this.Angle = (float)(Math.Atan2((double)vector.Z, (double)vector.X) - Math.Atan2((double)vector2.Z, (double)vector2.X));
					float num = this.Angle - MathHelper.ToRadians(90f);
					float num2 = (this._timeElapsed > this._animationData.RadiusDuration) ? this._animationData.MinRadius : (this._animationData.MaxRadius - (float)Easing.QuadEaseOut((double)this._timeElapsed, 0.0, (double)(this._animationData.MaxRadius - this._animationData.MinRadius), (double)this._animationData.RadiusDuration));
					this.OffsetSpritePosition = new Vector2(num2 * (float)Math.Cos((double)num), num2 * (float)Math.Sin((double)num));
					this.Alpha = this._animationData.MaxAlpha - (float)Easing.QuadEaseIn((double)this._timeElapsed, (double)this._animationData.MinAlpha, (double)this._animationData.MaxAlpha, (double)this._animationData.IndicatorDuration);
					this.ScaleFactor = ((this._timeElapsed > this._animationData.ScaleDuration) ? this._animationData.MinScaleFactor : (this._animationData.MaxScaleFactor - (float)Easing.BackEaseOutExtended((double)this._timeElapsed, 0.0, (double)(this._animationData.MaxScaleFactor - this._animationData.MinScaleFactor), (double)this._animationData.ScaleDuration)));
				}
			}

			// Token: 0x0400439B RID: 17307
			private readonly Vector3 _damageSourcePosition;

			// Token: 0x0400439D RID: 17309
			private readonly DamageEffectModule.AnimationData _animationData;

			// Token: 0x0400439E RID: 17310
			private float _timeElapsed = 0f;
		}
	}
}
