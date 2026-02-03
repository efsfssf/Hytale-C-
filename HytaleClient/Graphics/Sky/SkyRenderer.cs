using System;
using System.Diagnostics;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Graphics.Sky
{
	// Token: 0x02000A51 RID: 2641
	internal class SkyRenderer : Disposable
	{
		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x0600540A RID: 21514 RVA: 0x0018110E File Offset: 0x0017F30E
		// (set) Token: 0x0600540B RID: 21515 RVA: 0x00181116 File Offset: 0x0017F316
		public bool IsCloudsTextureLoading { get; private set; }

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x0600540C RID: 21516 RVA: 0x0018111F File Offset: 0x0017F31F
		// (set) Token: 0x0600540D RID: 21517 RVA: 0x00181127 File Offset: 0x0017F327
		public int CloudsTexturesCount { get; private set; }

		// Token: 0x0600540E RID: 21518 RVA: 0x00181130 File Offset: 0x0017F330
		private void InitializeClouds()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			for (int i = 0; i < 4; i++)
			{
				this.CloudsTextures[i] = gl.GenTexture();
				gl.BindTexture(GL.TEXTURE_2D, this.CloudsTextures[i]);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9985);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9729);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 10497);
				gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 10497);
			}
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x00181200 File Offset: 0x0017F400
		private void DisposeClouds()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			for (int i = 0; i < 4; i++)
			{
				gl.DeleteTexture(this.CloudsTextures[i]);
			}
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x00181244 File Offset: 0x0017F444
		public unsafe void RequestCloudsTextureUpdate(string[] targetCloudsChecksums, bool forceUpdate = false)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			GLFunctions gl = this._engine.Graphics.GL;
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				this._currentCloudsTextureChecksums[i] = targetCloudsChecksums[i];
				bool flag = targetCloudsChecksums[i] != null;
				if (flag)
				{
					num++;
				}
			}
			this.CloudsTexturesCount = num;
			bool flag2 = num == 0 && !forceUpdate;
			if (flag2)
			{
				this.IsCloudsTextureLoading = false;
			}
			else
			{
				this.IsCloudsTextureLoading = true;
				ThreadPool.QueueUserWorkItem(delegate(object _)
				{
					Image[] cloudsImages = new Image[4];
					for (int j = 0; j < 4; j++)
					{
						bool flag3 = targetCloudsChecksums[j] != null;
						if (flag3)
						{
							try
							{
								cloudsImages[j] = new Image(AssetManager.GetAssetUsingHash(targetCloudsChecksums[j], false));
							}
							catch (Exception exception)
							{
								cloudsImages[j] = null;
								SkyRenderer.Logger.Error(exception, "Failed to load cloud texture: " + AssetManager.GetAssetLocalPathUsingHash(targetCloudsChecksums[j]));
							}
						}
					}
					this._engine.RunOnMainThread(this._engine, delegate
					{
						for (int k = 0; k < 4; k++)
						{
							bool flag4 = targetCloudsChecksums[k] != this._currentCloudsTextureChecksums[k];
							if (flag4)
							{
								return;
							}
						}
						for (int l = 0; l < 4; l++)
						{
							bool flag5 = cloudsImages[l] != null;
							if (flag5)
							{
								gl.BindTexture(GL.TEXTURE_2D, this.CloudsTextures[l]);
								byte[] array;
								byte* value;
								if ((array = cloudsImages[l].Pixels) == null || array.Length == 0)
								{
									value = null;
								}
								else
								{
									value = &array[0];
								}
								gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, cloudsImages[l].Width, cloudsImages[l].Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
								array = null;
								gl.GenerateMipmap(GL.TEXTURE_2D);
							}
						}
						this.IsCloudsTextureLoading = false;
					}, false, false);
				});
			}
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x00181300 File Offset: 0x0017F500
		public void PrepareCloudsForDraw(ref Matrix viewRotationProjectionMatrix, ref Quaternion rotation)
		{
			Matrix.CreateFromQuaternion(ref rotation, out this._tempMatrix);
			Matrix.CreateScale(1000f, out this._cloudsMVPMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref this._cloudsMVPMatrix, out this._cloudsMVPMatrix);
			Matrix.AddTranslation(ref this._cloudsMVPMatrix, this.CloudsSphereOffset.X, this.CloudsSphereOffset.Y, this.CloudsSphereOffset.Z);
			Matrix.Multiply(ref this._cloudsMVPMatrix, ref viewRotationProjectionMatrix, out this._cloudsMVPMatrix);
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x00181384 File Offset: 0x0017F584
		public void DrawClouds()
		{
			bool isCloudsTextureLoading = this.IsCloudsTextureLoading;
			if (!isCloudsTextureLoading)
			{
				CloudsProgram cloudsProgram = this._engine.Graphics.GPUProgramStore.CloudsProgram;
				cloudsProgram.AssertInUse();
				cloudsProgram.MVPMatrix.SetValue(ref this._cloudsMVPMatrix);
				GLFunctions gl = this._engine.Graphics.GL;
				gl.BindVertexArray(this._cloudsVertexArray);
				gl.DrawElements(GL.TRIANGLES, this._sphereIndicesCount, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06005413 RID: 21523 RVA: 0x00181404 File Offset: 0x0017F604
		// (set) Token: 0x06005414 RID: 21524 RVA: 0x0018140C File Offset: 0x0017F60C
		public bool IsStarsTextureLoading { get; private set; }

		// Token: 0x06005415 RID: 21525 RVA: 0x00181418 File Offset: 0x0017F618
		public SkyRenderer(Engine engine)
		{
			this._engine = engine;
			GLFunctions gl = this._engine.Graphics.GL;
			this._sphereVerticesBuffer = gl.GenBuffer();
			this._sphereIndicesBuffer = gl.GenBuffer();
			gl.BindVertexArray(GLVertexArray.None);
			gl.BindBuffer(GLVertexArray.None, GL.ARRAY_BUFFER, this._sphereVerticesBuffer);
			gl.BindBuffer(GLVertexArray.None, GL.ELEMENT_ARRAY_BUFFER, this._sphereIndicesBuffer);
			this.CreateSphereGeometry();
			this.StarsTexture = gl.GenTexture();
			this._skyVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._skyVertexArray);
			gl.BindBuffer(this._skyVertexArray, GL.ARRAY_BUFFER, this._sphereVerticesBuffer);
			gl.BindBuffer(this._skyVertexArray, GL.ELEMENT_ARRAY_BUFFER, this._sphereIndicesBuffer);
			SkyProgram skyProgram = this._engine.Graphics.GPUProgramStore.SkyProgram;
			gl.EnableVertexAttribArray(skyProgram.AttribPosition.Index);
			gl.VertexAttribPointer(skyProgram.AttribPosition.Index, 3, GL.FLOAT, false, SkyAndCloudsVertex.Size, IntPtr.Zero);
			gl.EnableVertexAttribArray(skyProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(skyProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, SkyAndCloudsVertex.Size, (IntPtr)12);
			this.SunTexture = gl.GenTexture();
			this.MoonTexture = gl.GenTexture();
			this._sunOrMoonRenderer = new QuadRenderer(this._engine.Graphics, this._engine.Graphics.GPUProgramStore.BasicProgram.AttribPosition, this._engine.Graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			this._cloudsVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._cloudsVertexArray);
			gl.BindBuffer(this._cloudsVertexArray, GL.ARRAY_BUFFER, this._sphereVerticesBuffer);
			gl.BindBuffer(this._cloudsVertexArray, GL.ELEMENT_ARRAY_BUFFER, this._sphereIndicesBuffer);
			CloudsProgram cloudsProgram = this._engine.Graphics.GPUProgramStore.CloudsProgram;
			gl.EnableVertexAttribArray(cloudsProgram.AttribPosition.Index);
			gl.VertexAttribPointer(cloudsProgram.AttribPosition.Index, 3, GL.FLOAT, false, SkyAndCloudsVertex.Size, IntPtr.Zero);
			gl.EnableVertexAttribArray(cloudsProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(cloudsProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, SkyAndCloudsVertex.Size, (IntPtr)12);
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x0018170C File Offset: 0x0017F90C
		protected override void DoDispose()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			gl.DeleteTexture(this.StarsTexture);
			gl.DeleteBuffer(this._sphereVerticesBuffer);
			gl.DeleteBuffer(this._sphereIndicesBuffer);
			gl.DeleteVertexArray(this._skyVertexArray);
			gl.DeleteVertexArray(this._cloudsVertexArray);
			this.DisposeSunAndMoon();
			this.DisposeClouds();
			this.DisposeHorizon();
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x00181784 File Offset: 0x0017F984
		public void Initialize()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			GLFunctions gl = this._engine.Graphics.GL;
			gl.BindTexture(GL.TEXTURE_2D, this.StarsTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9729);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9729);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 10497);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 10497);
			this.InitializeSunAndMoon();
			this.InitializeClouds();
			this.InitializeHorizon();
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x00181844 File Offset: 0x0017FA44
		public unsafe void RequestStarsTextureUpdate(string targetStarsTextureChecksum, bool forceUpdate = false)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = !forceUpdate && targetStarsTextureChecksum == this._currentStarsTextureChecksum;
			if (!flag)
			{
				this._currentStarsTextureChecksum = targetStarsTextureChecksum;
				GLFunctions gl = this._engine.Graphics.GL;
				bool flag2 = targetStarsTextureChecksum == null;
				if (flag2)
				{
					this.IsStarsTextureLoading = false;
					gl.BindTexture(GL.TEXTURE_2D, this.StarsTexture);
					byte[] array;
					byte* value;
					if ((array = this._engine.Graphics.TransparentPixel) == null || array.Length == 0)
					{
						value = null;
					}
					else
					{
						value = &array[0];
					}
					gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, 1, 1, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
					array = null;
				}
				else
				{
					this.IsStarsTextureLoading = true;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						Image starsImage = null;
						try
						{
							starsImage = new Image(AssetManager.GetAssetUsingHash(targetStarsTextureChecksum, false));
						}
						catch (Exception exception)
						{
							SkyRenderer.Logger.Error(exception, "Failed to load star texture: " + AssetManager.GetAssetLocalPathUsingHash(targetStarsTextureChecksum));
						}
						this._engine.RunOnMainThread(this._engine, delegate
						{
							bool flag3 = starsImage == null;
							if (flag3)
							{
								this.IsStarsTextureLoading = false;
							}
							else
							{
								bool flag4 = targetStarsTextureChecksum != this._currentStarsTextureChecksum;
								if (!flag4)
								{
									gl.BindTexture(GL.TEXTURE_2D, this.StarsTexture);
									byte[] array2;
									byte* value2;
									if ((array2 = starsImage.Pixels) == null || array2.Length == 0)
									{
										value2 = null;
									}
									else
									{
										value2 = &array2[0];
									}
									gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, starsImage.Width, starsImage.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value2));
									array2 = null;
									this.IsStarsTextureLoading = false;
								}
							}
						}, false, false);
					});
				}
			}
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x00181960 File Offset: 0x0017FB60
		private unsafe void CreateSphereGeometry()
		{
			SkyAndCloudsVertex[] array = new SkyAndCloudsVertex[400];
			for (int i = 0; i < 20; i++)
			{
				float num = (float)i / 19f * 3.1415927f;
				float y = (float)i / 19f;
				for (int j = 0; j < 20; j++)
				{
					int num2 = i * 20 + j;
					float num3 = (float)j / 19f * 6.2831855f;
					array[num2].Position = new Vector3((float)Math.Cos((double)num), (float)Math.Sin((double)num) * (float)Math.Sin((double)num3), (float)Math.Sin((double)num) * (float)Math.Cos((double)num3));
					float x = (float)j / 19f;
					array[num2].TextureCoordinates = new Vector2(x, y);
				}
			}
			int num4 = 19;
			this._sphereIndicesCount = num4 * num4 * 6;
			ushort[] array2 = new ushort[this._sphereIndicesCount];
			for (int k = 0; k < num4; k++)
			{
				for (int l = 0; l < num4; l++)
				{
					ushort num5 = (ushort)(k * 20 + l);
					ushort num6 = (ushort)(k * 20 + l + 1);
					ushort num7 = (ushort)((k + 1) * 20 + l + 1);
					ushort num8 = (ushort)((k + 1) * 20 + l);
					int num9 = (k * num4 + l) * 6;
					array2[num9] = num5;
					array2[num9 + 1] = num7;
					array2[num9 + 2] = num6;
					array2[num9 + 3] = num5;
					array2[num9 + 4] = num8;
					array2[num9 + 5] = num7;
				}
			}
			GLFunctions gl = this._engine.Graphics.GL;
			SkyAndCloudsVertex[] array3;
			SkyAndCloudsVertex* value;
			if ((array3 = array) == null || array3.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array3[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(array.Length * SkyAndCloudsVertex.Size), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array3 = null;
			ushort[] array4;
			ushort* value2;
			if ((array4 = array2) == null || array4.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array4[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._sphereIndicesCount * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array4 = null;
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x00181BB5 File Offset: 0x0017FDB5
		public void PrepareSkyForDraw(ref Matrix viewRotationProjectionMatrix)
		{
			Matrix.CreateScale(1000f, out this._skyMVPMatrix);
			Matrix.Multiply(ref this._skyMVPMatrix, ref viewRotationProjectionMatrix, out this._skyMVPMatrix);
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x00181BDC File Offset: 0x0017FDDC
		public void DrawSky()
		{
			SkyProgram skyProgram = this._engine.Graphics.GPUProgramStore.SkyProgram;
			skyProgram.AssertInUse();
			this._engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this.StarsTexture);
			skyProgram.MVPMatrix.SetValue(ref this._skyMVPMatrix);
			GLFunctions gl = this._engine.Graphics.GL;
			gl.BindVertexArray(this._skyVertexArray);
			gl.DrawElements(GL.TRIANGLES, this._sphereIndicesCount, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x00181C74 File Offset: 0x0017FE74
		private unsafe void InitializeHorizon()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			PrimitiveModelData primitiveModelData = CylinderModel.BuildHollowModelData(1f, 1f, 8);
			this._horizonVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._horizonVertexArray);
			this._horizonVerticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._horizonVertexArray, GL.ARRAY_BUFFER, this._horizonVerticesBuffer);
			float[] array;
			float* value;
			if ((array = primitiveModelData.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(primitiveModelData.Vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			this._horizonIndicesBuffer = gl.GenBuffer();
			this._horizonIndicesCount = primitiveModelData.Indices.Length;
			gl.BindBuffer(this._horizonVertexArray, GL.ELEMENT_ARRAY_BUFFER, this._horizonIndicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = primitiveModelData.Indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._horizonIndicesCount * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
			BasicProgram basicProgram = this._engine.Graphics.GPUProgramStore.BasicProgram;
			gl.EnableVertexAttribArray(basicProgram.AttribPosition.Index);
			gl.VertexAttribPointer(basicProgram.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(basicProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(basicProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x00181E3C File Offset: 0x0018003C
		private void DisposeHorizon()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			gl.DeleteBuffer(this._horizonVerticesBuffer);
			gl.DeleteBuffer(this._horizonIndicesBuffer);
			gl.DeleteVertexArray(this._horizonVertexArray);
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x00181E84 File Offset: 0x00180084
		public void PrepareHorizonForDraw(ref Matrix viewRotationProjectionMatrix, Vector3 horizonPosition, Vector3 horizonScale)
		{
			Matrix.CreateScale(horizonScale.X, horizonScale.Y, horizonScale.Z, out this._horizonMVPMatrix);
			Matrix.AddTranslation(ref this._horizonMVPMatrix, horizonPosition.X, horizonPosition.Y, horizonPosition.Z);
			Matrix.Multiply(ref this._horizonMVPMatrix, ref viewRotationProjectionMatrix, out this._horizonMVPMatrix);
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x00181EE4 File Offset: 0x001800E4
		public void DrawHorizon()
		{
			BasicProgram basicProgram = this._engine.Graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Opacity.AssertValue(1f);
			this._engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this._engine.Graphics.WhitePixelTexture.GLTexture);
			basicProgram.MVPMatrix.SetValue(ref this._horizonMVPMatrix);
			GLFunctions gl = this._engine.Graphics.GL;
			gl.BindVertexArray(this._horizonVertexArray);
			gl.DrawElements(GL.TRIANGLES, this._horizonIndicesCount, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06005420 RID: 21536 RVA: 0x00181F99 File Offset: 0x00180199
		// (set) Token: 0x06005421 RID: 21537 RVA: 0x00181FA1 File Offset: 0x001801A1
		public GLTexture SunTexture { get; private set; }

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06005422 RID: 21538 RVA: 0x00181FAA File Offset: 0x001801AA
		// (set) Token: 0x06005423 RID: 21539 RVA: 0x00181FB2 File Offset: 0x001801B2
		public GLTexture MoonTexture { get; private set; }

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06005424 RID: 21540 RVA: 0x00181FBB File Offset: 0x001801BB
		// (set) Token: 0x06005425 RID: 21541 RVA: 0x00181FC3 File Offset: 0x001801C3
		public bool IsMoonTextureLoading { get; private set; }

		// Token: 0x06005426 RID: 21542 RVA: 0x00181FCC File Offset: 0x001801CC
		private void InitializeSunAndMoon()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			gl.BindTexture(GL.TEXTURE_2D, this.SunTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
			gl.BindTexture(GL.TEXTURE_2D, this.MoonTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x001820E8 File Offset: 0x001802E8
		public unsafe void LoadSunTexture(string sunChecksum)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				try
				{
					Image sunImage = new Image(AssetManager.GetAssetUsingHash(sunChecksum, false));
					this._engine.RunOnMainThread(this._engine, delegate
					{
						GLFunctions gl = this._engine.Graphics.GL;
						gl.BindTexture(GL.TEXTURE_2D, this.SunTexture);
						byte[] array;
						byte* value;
						if ((array = sunImage.Pixels) == null || array.Length == 0)
						{
							value = null;
						}
						else
						{
							value = &array[0];
						}
						gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, sunImage.Width, sunImage.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
						array = null;
						this._isSunTextureLoaded = true;
					}, false, false);
				}
				catch (Exception exception)
				{
					SkyRenderer.Logger.Error(exception, "Failed to load sun texture: " + AssetManager.GetAssetLocalPathUsingHash(sunChecksum));
				}
			});
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x0018211C File Offset: 0x0018031C
		public unsafe void RequestMoonTextureUpdate(string targetMoonTextureChecksum, bool forceUpdate = false)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = !forceUpdate && targetMoonTextureChecksum == this._currentMoonTextureChecksum;
			if (!flag)
			{
				this._currentMoonTextureChecksum = targetMoonTextureChecksum;
				GLFunctions gl = this._engine.Graphics.GL;
				bool flag2 = targetMoonTextureChecksum == null;
				if (flag2)
				{
					this.IsMoonTextureLoading = false;
					gl.BindTexture(GL.TEXTURE_2D, this.MoonTexture);
					byte[] array;
					byte* value;
					if ((array = this._engine.Graphics.TransparentPixel) == null || array.Length == 0)
					{
						value = null;
					}
					else
					{
						value = &array[0];
					}
					gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, 1, 1, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
					array = null;
				}
				else
				{
					this.IsMoonTextureLoading = true;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						Image moonImage = null;
						try
						{
							moonImage = new Image(AssetManager.GetAssetUsingHash(targetMoonTextureChecksum, false));
						}
						catch (Exception exception)
						{
							SkyRenderer.Logger.Error(exception, "Failed to load moon texture: " + AssetManager.GetAssetLocalPathUsingHash(targetMoonTextureChecksum));
						}
						this._engine.RunOnMainThread(this._engine, delegate
						{
							bool flag3 = moonImage == null;
							if (flag3)
							{
								this.IsMoonTextureLoading = false;
							}
							else
							{
								bool flag4 = targetMoonTextureChecksum != this._currentMoonTextureChecksum;
								if (!flag4)
								{
									gl.BindTexture(GL.TEXTURE_2D, this.MoonTexture);
									byte[] array2;
									byte* value2;
									if ((array2 = moonImage.Pixels) == null || array2.Length == 0)
									{
										value2 = null;
									}
									else
									{
										value2 = &array2[0];
									}
									gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, moonImage.Width, moonImage.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value2));
									array2 = null;
									this.IsMoonTextureLoading = false;
								}
							}
						}, false, false);
					});
				}
			}
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x00182238 File Offset: 0x00180438
		private void DisposeSunAndMoon()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			gl.DeleteTexture(this.SunTexture);
			gl.DeleteTexture(this.MoonTexture);
			this._sunOrMoonRenderer.Dispose();
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x00182280 File Offset: 0x00180480
		public bool SunNeedsDrawing(Vector3 normalizedSunPosition, Vector3 cameraDirection, float sunScale)
		{
			return this._isSunTextureLoaded && normalizedSunPosition.Y > -0.3f && Vector3.Dot(cameraDirection, normalizedSunPosition) > -(sunScale * 0.4f);
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x001822BC File Offset: 0x001804BC
		public void PrepareSunForDraw(ref Matrix viewRotationMatrix, ref Matrix projectionMatrix, Vector3 normalizedSunPosition, float sunScale)
		{
			Matrix.CreateTranslation(-0.5f, -0.5f, 0f, out this.SunMVPMatrix);
			Matrix.CreateScale(102f * sunScale, out this._tempMatrix);
			Matrix.Multiply(ref this.SunMVPMatrix, ref this._tempMatrix, out this.SunMVPMatrix);
			Vector3 vector = Vector3.Transform(normalizedSunPosition * 1000f, viewRotationMatrix);
			this.SunMVPMatrix.M41 = this.SunMVPMatrix.M41 + vector.X;
			this.SunMVPMatrix.M42 = this.SunMVPMatrix.M42 + vector.Y;
			this.SunMVPMatrix.M43 = this.SunMVPMatrix.M43 + vector.Z;
			Matrix.Multiply(ref this.SunMVPMatrix, ref projectionMatrix, out this.SunMVPMatrix);
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x0018237C File Offset: 0x0018057C
		public void DrawSun()
		{
			BasicProgram basicProgram = this._engine.Graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Opacity.AssertValue(1f);
			this._engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this.SunTexture);
			basicProgram.MVPMatrix.SetValue(ref this.SunMVPMatrix);
			this._sunOrMoonRenderer.Draw();
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x001823F8 File Offset: 0x001805F8
		public bool BackgroundSkyNeedDrawing(Vector3 normalizedSunPosition)
		{
			return normalizedSunPosition.Y > -0.85f;
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x00182418 File Offset: 0x00180618
		public bool StarsNeedDrawing(Vector3 normalizedSunPosition)
		{
			return -normalizedSunPosition.Y > -0.3f;
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x00182438 File Offset: 0x00180638
		public bool MoonNeedsDrawing(Vector3 normalizedSunPosition, Vector3 cameraDirection, float moonScale)
		{
			return -normalizedSunPosition.Y > -0.3f && !this.IsMoonTextureLoading && Vector3.Dot(cameraDirection, -normalizedSunPosition) > -(moonScale * 0.4f);
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x0018247C File Offset: 0x0018067C
		public void PrepareMoonForDraw(ref Matrix viewRotationMatrix, ref Matrix projectionMatrix, Vector3 normalizedSunPosition, float moonScale)
		{
			Matrix.CreateTranslation(-0.5f, -0.5f, 0f, out this._moonMVPMatrix);
			Matrix.CreateScale(102f * moonScale, out this._tempMatrix);
			Matrix.Multiply(ref this._moonMVPMatrix, ref this._tempMatrix, out this._moonMVPMatrix);
			Vector3 vector = Vector3.Transform(-normalizedSunPosition * 1000f, viewRotationMatrix);
			this._moonMVPMatrix.M41 = this._moonMVPMatrix.M41 + vector.X;
			this._moonMVPMatrix.M42 = this._moonMVPMatrix.M42 + vector.Y;
			this._moonMVPMatrix.M43 = this._moonMVPMatrix.M43 + vector.Z;
			Matrix.Multiply(ref this._moonMVPMatrix, ref projectionMatrix, out this._moonMVPMatrix);
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x00182540 File Offset: 0x00180740
		public void DrawMoon()
		{
			BasicProgram basicProgram = this._engine.Graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			this._engine.Graphics.GL.AssertTextureBound(GL.TEXTURE0, this.MoonTexture);
			basicProgram.MVPMatrix.SetValue(ref this._moonMVPMatrix);
			this._sunOrMoonRenderer.Draw();
		}

		// Token: 0x04002EEE RID: 12014
		private readonly Vector3 CloudsSphereOffset = new Vector3(0f, -500f, 0f);

		// Token: 0x04002EF0 RID: 12016
		private string[] _currentCloudsTextureChecksums = new string[4];

		// Token: 0x04002EF1 RID: 12017
		public readonly GLTexture[] CloudsTextures = new GLTexture[4];

		// Token: 0x04002EF2 RID: 12018
		public readonly Vector4[] CloudColors = new Vector4[4];

		// Token: 0x04002EF3 RID: 12019
		public readonly float[] CloudOffsets = new float[4];

		// Token: 0x04002EF5 RID: 12021
		private Matrix _cloudsMVPMatrix;

		// Token: 0x04002EF6 RID: 12022
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002EF7 RID: 12023
		private const int SphereSegments = 20;

		// Token: 0x04002EF8 RID: 12024
		private const int SphereRadius = 1000;

		// Token: 0x04002EF9 RID: 12025
		private readonly Engine _engine;

		// Token: 0x04002EFA RID: 12026
		private readonly GLVertexArray _skyVertexArray;

		// Token: 0x04002EFB RID: 12027
		private readonly GLVertexArray _cloudsVertexArray;

		// Token: 0x04002EFC RID: 12028
		private readonly GLBuffer _sphereVerticesBuffer;

		// Token: 0x04002EFD RID: 12029
		private readonly GLBuffer _sphereIndicesBuffer;

		// Token: 0x04002EFE RID: 12030
		private int _sphereIndicesCount;

		// Token: 0x04002EFF RID: 12031
		public readonly GLTexture StarsTexture;

		// Token: 0x04002F01 RID: 12033
		private string _currentStarsTextureChecksum;

		// Token: 0x04002F02 RID: 12034
		private Matrix _tempMatrix;

		// Token: 0x04002F03 RID: 12035
		private Matrix _skyMVPMatrix;

		// Token: 0x04002F04 RID: 12036
		private GLVertexArray _horizonVertexArray;

		// Token: 0x04002F05 RID: 12037
		private GLBuffer _horizonVerticesBuffer;

		// Token: 0x04002F06 RID: 12038
		private GLBuffer _horizonIndicesBuffer;

		// Token: 0x04002F07 RID: 12039
		private int _horizonIndicesCount;

		// Token: 0x04002F08 RID: 12040
		private Matrix _horizonMVPMatrix;

		// Token: 0x04002F09 RID: 12041
		private const int SunSize = 102;

		// Token: 0x04002F0A RID: 12042
		private const int MoonSize = 102;

		// Token: 0x04002F0D RID: 12045
		public Matrix SunMVPMatrix;

		// Token: 0x04002F0E RID: 12046
		private Matrix _moonMVPMatrix;

		// Token: 0x04002F0F RID: 12047
		private readonly QuadRenderer _sunOrMoonRenderer;

		// Token: 0x04002F10 RID: 12048
		private bool _isSunTextureLoaded = false;

		// Token: 0x04002F12 RID: 12050
		private string _currentMoonTextureChecksum;
	}
}
