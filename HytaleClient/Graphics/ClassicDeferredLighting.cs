using System;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x0200099D RID: 2461
	internal class ClassicDeferredLighting
	{
		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06004EDE RID: 20190 RVA: 0x001604E2 File Offset: 0x0015E6E2
		public int LightCount
		{
			get
			{
				return this._globalLightDataCount;
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001604EC File Offset: 0x0015E6EC
		public ClassicDeferredLighting(GraphicsDevice graphics, RenderTargetStore renderTargetStore)
		{
			this._graphics = graphics;
			this._gpuProgramStore = graphics.GPUProgramStore;
			this._renderTargetStore = renderTargetStore;
			this._gl = this._graphics.GL;
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x001605A1 File Offset: 0x0015E7A1
		public void Init()
		{
			MeshProcessor.CreateSphere(ref this._sphereLightMesh, 5, 8, 1f, 0, -1, -1);
			MeshProcessor.CreateSimpleBox(ref this._boxLightMesh, 1f);
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x001605CB File Offset: 0x0015E7CB
		public void Dispose()
		{
			this._sphereLightMesh.Dispose();
			this._boxLightMesh.Dispose();
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x001605E8 File Offset: 0x0015E7E8
		public void PrepareLightsForDraw(LightData[] lightData, int lightCount, Vector3 cameraPosition, ref Matrix viewRotationMatrix, ref Matrix invViewRotationMatrix, bool completeFullSetup)
		{
			this._outerLightDrawTasksCount = 0;
			this._innerLightDrawTasksCount = 0;
			for (int i = 0; i < lightCount; i++)
			{
				float radius = lightData[i].Sphere.Radius;
				Vector3 vector = lightData[i].Sphere.Center;
				Vector3 color = lightData[i].Color;
				BoundingSphere boundingSphere;
				boundingSphere.Center = vector;
				boundingSphere.Radius = radius;
				vector -= cameraPosition;
				Matrix modelMatrix;
				Matrix.CreateScale(radius, out modelMatrix);
				Matrix.AddTranslation(ref modelMatrix, vector.X, vector.Y, vector.Z);
				vector = Vector3.Transform(vector, viewRotationMatrix);
				bool flag = boundingSphere.Contains(cameraPosition) > ContainmentType.Disjoint;
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = (int)this._innerLightDrawTasksCount < this._innerLightDrawTasks.Length;
					if (flag3)
					{
						this._innerLightDrawTasks[(int)this._innerLightDrawTasksCount].ModelMatrix = modelMatrix;
						this._innerLightDrawTasks[(int)this._innerLightDrawTasksCount].Sphere = new BoundingSphere(vector, radius);
						this._innerLightDrawTasks[(int)this._innerLightDrawTasksCount].Color = color;
						this._innerLightDrawTasksCount += 1;
					}
				}
				else
				{
					bool flag4 = (int)this._outerLightDrawTasksCount < this._outerLightDrawTasks.Length;
					if (flag4)
					{
						this._outerLightDrawTasks[(int)this._outerLightDrawTasksCount].ModelMatrix = modelMatrix;
						this._outerLightDrawTasks[(int)this._outerLightDrawTasksCount].Sphere = new BoundingSphere(vector, radius);
						this._outerLightDrawTasks[(int)this._outerLightDrawTasksCount].Color = color;
						this._outerLightDrawTasksCount += 1;
					}
				}
			}
			this._globalLightDataCount = (int)(this._innerLightDrawTasksCount + this._outerLightDrawTasksCount);
			bool flag5 = !completeFullSetup;
			if (!flag5)
			{
				bool flag6 = this.UseStencilForOuterLights && this._outerLightDrawTasksCount > 0;
				if (flag6)
				{
					BoundingSphere sphere;
					sphere.Center = Vector3.Transform(this._outerLightDrawTasks[0].Sphere.Center, invViewRotationMatrix);
					sphere.Radius = this._outerLightDrawTasks[0].Sphere.Radius;
					this._globalLightBoundingBox = BoundingBox.CreateFromSphere(sphere);
					for (int j = 1; j < (int)this._outerLightDrawTasksCount; j++)
					{
						sphere.Center = Vector3.Transform(this._outerLightDrawTasks[j].Sphere.Center, invViewRotationMatrix);
						sphere.Radius = this._outerLightDrawTasks[j].Sphere.Radius;
						BoundingBox additional = BoundingBox.CreateFromSphere(sphere);
						this._globalLightBoundingBox = BoundingBox.CreateMerged(this._globalLightBoundingBox, additional);
					}
					Vector3 vector2 = this._globalLightBoundingBox.Max - this._globalLightBoundingBox.Min;
					Vector3 center = this._globalLightBoundingBox.GetCenter();
					Matrix.CreateScale(ref vector2, out this._boxLightModelMatrix);
					Matrix.AddTranslation(ref this._boxLightModelMatrix, center.X, center.Y, center.Z);
				}
				bool useLightGroups = this.UseLightGroups;
				if (useLightGroups)
				{
					this.PrepareLightGroupDrawTasks(12f, ref invViewRotationMatrix, ref this._innerLightDrawTasks, this._innerLightDrawTasksCount, ref this._globalLightPositionSizes, ref this._globalLightColors, 0, ref this._innerLightGroupDrawTasks, out this._innerLightGroupDrawTasksCount);
					this.PrepareLightGroupDrawTasks(7.5f, ref invViewRotationMatrix, ref this._outerLightDrawTasks, this._outerLightDrawTasksCount, ref this._globalLightPositionSizes, ref this._globalLightColors, (int)this._innerLightDrawTasksCount, ref this._outerLightGroupDrawTasks, out this._outerLightGroupDrawTasksCount);
					for (int k = 0; k < (int)this._outerLightGroupDrawTasksCount; k++)
					{
						bool flag7 = this._outerLightGroupDrawTasks[k].Sphere.Contains(new Vector3(0f)) > ContainmentType.Disjoint;
						bool flag8 = flag7;
						if (flag8)
						{
							this._innerLightGroupDrawTasks[(int)this._innerLightGroupDrawTasksCount] = this._outerLightGroupDrawTasks[k];
							this._innerLightGroupDrawTasksCount += 1;
							this._outerLightGroupDrawTasks[k] = this._outerLightGroupDrawTasks[(int)(this._outerLightGroupDrawTasksCount - 1)];
							this._outerLightGroupDrawTasksCount -= 1;
							k--;
						}
					}
				}
			}
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00160A48 File Offset: 0x0015EC48
		private void PrepareLightGroupDrawTasks(float maxGroupSphereRadius, ref Matrix invViewRotationMatrix, ref ClassicDeferredLighting.LightDrawTask[] inputLightDrawTasks, ushort inputLightDrawTaskCount, ref Vector4[] outputLightPositionSizes, ref Vector3[] outputLightColors, int outputLightStart, ref ClassicDeferredLighting.LightGroupDrawTask[] outputLightGroupDrawTasks, out ushort outputLightGroupDrawTasksCount)
		{
			float num = maxGroupSphereRadius * maxGroupSphereRadius;
			outputLightGroupDrawTasksCount = 0;
			bool flag = inputLightDrawTaskCount > 0;
			if (flag)
			{
				int num2 = outputLightStart;
				ushort[] array = new ushort[(int)inputLightDrawTaskCount];
				for (int i = 0; i < (int)inputLightDrawTaskCount; i++)
				{
					array[i] = (ushort)i;
				}
				int j = array.Length;
				while (j > 0)
				{
					int num3 = 0;
					ushort num4 = array[num3];
					ushort num5 = outputLightGroupDrawTasksCount;
					BoundingSphere boundingSphere = inputLightDrawTasks[(int)num4].Sphere;
					ushort num6 = 1;
					outputLightGroupDrawTasks[(int)num5].LightIndexStart = (ushort)num2;
					outputLightGroupDrawTasksCount += 1;
					outputLightColors[num2] = inputLightDrawTasks[(int)num4].Color;
					outputLightPositionSizes[num2] = new Vector4(inputLightDrawTasks[(int)num4].Sphere.Center.X, inputLightDrawTasks[(int)num4].Sphere.Center.Y, inputLightDrawTasks[(int)num4].Sphere.Center.Z, inputLightDrawTasks[(int)num4].Sphere.Radius);
					num2++;
					array[num3] = array[j - 1];
					j--;
					for (int k = 0; k < j; k++)
					{
						num4 = array[k];
						float num7;
						Vector3.DistanceSquared(ref boundingSphere.Center, ref inputLightDrawTasks[(int)num4].Sphere.Center, out num7);
						bool flag2 = num7 < num;
						if (flag2)
						{
							BoundingSphere boundingSphere2;
							BoundingSphere.CreateMerged(ref boundingSphere, ref inputLightDrawTasks[(int)num4].Sphere, out boundingSphere2);
							bool flag3 = boundingSphere2.Radius < maxGroupSphereRadius;
							if (flag3)
							{
								boundingSphere = boundingSphere2;
								num6 += 1;
								outputLightColors[num2] = inputLightDrawTasks[(int)num4].Color;
								outputLightPositionSizes[num2] = new Vector4(inputLightDrawTasks[(int)num4].Sphere.Center.X, inputLightDrawTasks[(int)num4].Sphere.Center.Y, inputLightDrawTasks[(int)num4].Sphere.Center.Z, inputLightDrawTasks[(int)num4].Sphere.Radius);
								num2++;
								array[k] = array[j - 1];
								j--;
								k--;
							}
						}
					}
					Vector3 vector = Vector3.Transform(boundingSphere.Center, invViewRotationMatrix);
					Matrix modelMatrix;
					Matrix.CreateScale(boundingSphere.Radius, out modelMatrix);
					Matrix.AddTranslation(ref modelMatrix, vector.X, vector.Y, vector.Z);
					outputLightGroupDrawTasks[(int)num5].ModelMatrix = modelMatrix;
					outputLightGroupDrawTasks[(int)num5].Sphere = boundingSphere;
					outputLightGroupDrawTasks[(int)num5].LightCount = num6;
				}
			}
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x00160D34 File Offset: 0x0015EF34
		public void TagStencil(uint stencilMask, ref Matrix viewRotationProjectionMatrix)
		{
			this._gl.StencilMask(stencilMask);
			this._gl.Enable(GL.DEPTH_TEST);
			this._gl.Disable(GL.CULL_FACE);
			this._gl.ColorMask(false, false, false, false);
			this._gl.StencilFunc(GL.ALWAYS, 0, stencilMask);
			this._gl.StencilOp(GL.KEEP, GL.INVERT, GL.KEEP);
			ZOnlyProgram zonlyProgram = this._gpuProgramStore.ZOnlyProgram;
			this._gl.UseProgram(zonlyProgram);
			zonlyProgram.ViewProjectionMatrix.SetValue(ref viewRotationProjectionMatrix);
			zonlyProgram.ModelMatrix.SetValue(ref this._boxLightModelMatrix);
			this._gl.BindVertexArray(this._boxLightMesh.VertexArray);
			this._gl.DrawElements(GL.TRIANGLES, this._boxLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
			this._gl.ColorMask(true, true, true, true);
			this._gl.StencilMask(0U);
			this._gl.Enable(GL.CULL_FACE);
			this._gl.StencilOp(GL.KEEP, GL.KEEP, GL.REPLACE);
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x00160E80 File Offset: 0x0015F080
		public void DrawDeferredLights(bool fullResolution, bool useStencilForOuterLights, ref Matrix viewRotationMatrix, ref Matrix projectionMatrix, float farClip)
		{
			this._gl.AssertDepthMask(false);
			this._gl.AssertBlendFunc(GL.SRC_ALPHA, GL.ONE);
			this._gl.AssertEnabled(GL.BLEND);
			this._gl.AssertActiveTexture(GL.TEXTURE0);
			this._gl.Enable(GL.DEPTH_TEST);
			this._gl.Enable(GL.CULL_FACE);
			LightProgram lightProgram = fullResolution ? this._gpuProgramStore.LightProgram : this._gpuProgramStore.LightLowResProgram;
			this._gl.UseProgram(lightProgram);
			lightProgram.Debug.SetValue(0);
			lightProgram.ProjectionMatrix.SetValue(ref projectionMatrix);
			lightProgram.ViewMatrix.SetValue(ref viewRotationMatrix);
			if (fullResolution)
			{
				lightProgram.InvScreenSize.SetValue(this._renderTargetStore.LightBufferFullRes.InvWidth, this._renderTargetStore.LightBufferFullRes.InvHeight);
			}
			else
			{
				lightProgram.InvScreenSize.SetValue(this._renderTargetStore.LightBufferHalfRes.InvWidth, this._renderTargetStore.LightBufferHalfRes.InvHeight);
			}
			bool useLightGroups = this.UseLightGroups;
			if (useLightGroups)
			{
				bool flag = this.LightDataTransferMethod == 1;
				if (flag)
				{
					lightProgram.GlobalLightColors.SetValue(this._globalLightColors, this._globalLightDataCount);
					lightProgram.GlobalLightPositionSizes.SetValue(this._globalLightPositionSizes, this._globalLightDataCount);
				}
			}
			lightProgram.UseLightGroup.SetValue(this.UseLightGroups ? 1 : 0);
			lightProgram.TransferMethod.SetValue(this.LightDataTransferMethod);
			bool useLinearZForLight = this._graphics.UseLinearZForLight;
			if (useLinearZForLight)
			{
				this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.LinearZ.GetTexture(RenderTarget.Target.Color0));
				lightProgram.FarClip.SetValue(farClip);
			}
			else
			{
				this._gl.BindTexture(GL.TEXTURE_2D, this._renderTargetStore.GBuffer.GetTexture(RenderTarget.Target.Depth));
			}
			this._gl.BindVertexArray(this._sphereLightMesh.VertexArray);
			this._gl.CullFace(GL.FRONT);
			this._gl.DepthFunc(GL.GREATER);
			bool useLightGroups2 = this.UseLightGroups;
			if (useLightGroups2)
			{
				bool flag2 = this.LightDataTransferMethod == 0;
				if (flag2)
				{
					for (int i = 0; i < (int)this._innerLightGroupDrawTasksCount; i++)
					{
						lightProgram.ModelMatrix.SetValue(ref this._innerLightGroupDrawTasks[i].ModelMatrix);
						lightProgram.GlobalLightColors.SetValue(this._globalLightColors, (int)this._innerLightGroupDrawTasks[i].LightIndexStart, (int)this._innerLightGroupDrawTasks[i].LightCount);
						lightProgram.GlobalLightPositionSizes.SetValue(this._globalLightPositionSizes, (int)this._innerLightGroupDrawTasks[i].LightIndexStart, (int)this._innerLightGroupDrawTasks[i].LightCount);
						lightProgram.LightGroup.SetValue(0, (int)this._innerLightGroupDrawTasks[i].LightCount);
						this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
				else
				{
					for (int j = 0; j < (int)this._innerLightGroupDrawTasksCount; j++)
					{
						lightProgram.ModelMatrix.SetValue(ref this._innerLightGroupDrawTasks[j].ModelMatrix);
						lightProgram.LightGroup.SetValue((int)this._innerLightGroupDrawTasks[j].LightIndexStart, (int)this._innerLightGroupDrawTasks[j].LightCount);
						this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
			}
			else
			{
				for (int k = 0; k < (int)this._innerLightDrawTasksCount; k++)
				{
					lightProgram.ModelMatrix.SetValue(ref this._innerLightDrawTasks[k].ModelMatrix);
					lightProgram.Color.SetValue(this._innerLightDrawTasks[k].Color);
					lightProgram.PositionSize.SetValue(this._innerLightDrawTasks[k].Sphere.Center.X, this._innerLightDrawTasks[k].Sphere.Center.Y, this._innerLightDrawTasks[k].Sphere.Center.Z, this._innerLightDrawTasks[k].Sphere.Radius);
					this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				}
			}
			this._gl.CullFace(GL.BACK);
			this._gl.DepthFunc(GL.LEQUAL);
			if (useStencilForOuterLights)
			{
				this._gl.StencilFunc(GL.NOTEQUAL, 0, 32U);
			}
			bool useLightGroups3 = this.UseLightGroups;
			if (useLightGroups3)
			{
				bool flag3 = this.LightDataTransferMethod == 0;
				if (flag3)
				{
					for (int l = 0; l < (int)this._outerLightGroupDrawTasksCount; l++)
					{
						lightProgram.ModelMatrix.SetValue(ref this._outerLightGroupDrawTasks[l].ModelMatrix);
						lightProgram.GlobalLightColors.SetValue(this._globalLightColors, (int)this._outerLightGroupDrawTasks[l].LightIndexStart, (int)this._outerLightGroupDrawTasks[l].LightCount);
						lightProgram.GlobalLightPositionSizes.SetValue(this._globalLightPositionSizes, (int)this._outerLightGroupDrawTasks[l].LightIndexStart, (int)this._outerLightGroupDrawTasks[l].LightCount);
						lightProgram.LightGroup.SetValue(0, (int)this._outerLightGroupDrawTasks[l].LightCount);
						this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
				else
				{
					for (int m = 0; m < (int)this._outerLightGroupDrawTasksCount; m++)
					{
						lightProgram.ModelMatrix.SetValue(ref this._outerLightGroupDrawTasks[m].ModelMatrix);
						lightProgram.LightGroup.SetValue((int)this._outerLightGroupDrawTasks[m].LightIndexStart, (int)this._outerLightGroupDrawTasks[m].LightCount);
						this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
					}
				}
			}
			else
			{
				for (int n = 0; n < (int)this._outerLightDrawTasksCount; n++)
				{
					lightProgram.ModelMatrix.SetValue(ref this._outerLightDrawTasks[n].ModelMatrix);
					lightProgram.Color.SetValue(this._outerLightDrawTasks[n].Color);
					lightProgram.PositionSize.SetValue(this._outerLightDrawTasks[n].Sphere.Center.X, this._outerLightDrawTasks[n].Sphere.Center.Y, this._outerLightDrawTasks[n].Sphere.Center.Z, this._outerLightDrawTasks[n].Sphere.Radius);
					this._gl.DrawElements(GL.TRIANGLES, this._sphereLightMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				}
			}
			this._gl.Disable(GL.CULL_FACE);
			this._gl.AssertCullFace(GL.BACK);
			this._gl.AssertDepthFunc(GL.LEQUAL);
			this._gl.AssertDepthMask(false);
			this._gl.AssertBlendFunc(GL.SRC_ALPHA, GL.ONE);
			this._gl.AssertEnabled(GL.BLEND);
		}

		// Token: 0x04002A13 RID: 10771
		public int LightDataTransferMethod = 0;

		// Token: 0x04002A14 RID: 10772
		public bool UseLightGroups = true;

		// Token: 0x04002A15 RID: 10773
		public bool UseStencilForOuterLights = true;

		// Token: 0x04002A16 RID: 10774
		private ClassicDeferredLighting.LightDrawTask[] _outerLightDrawTasks = new ClassicDeferredLighting.LightDrawTask[1024];

		// Token: 0x04002A17 RID: 10775
		private ClassicDeferredLighting.LightDrawTask[] _innerLightDrawTasks = new ClassicDeferredLighting.LightDrawTask[1024];

		// Token: 0x04002A18 RID: 10776
		private ushort _outerLightDrawTasksCount;

		// Token: 0x04002A19 RID: 10777
		private ushort _innerLightDrawTasksCount;

		// Token: 0x04002A1A RID: 10778
		private Vector4[] _globalLightPositionSizes = new Vector4[1024];

		// Token: 0x04002A1B RID: 10779
		private Vector3[] _globalLightColors = new Vector3[1024];

		// Token: 0x04002A1C RID: 10780
		private int _globalLightDataCount;

		// Token: 0x04002A1D RID: 10781
		private ClassicDeferredLighting.LightGroupDrawTask[] _outerLightGroupDrawTasks = new ClassicDeferredLighting.LightGroupDrawTask[1024];

		// Token: 0x04002A1E RID: 10782
		private ClassicDeferredLighting.LightGroupDrawTask[] _innerLightGroupDrawTasks = new ClassicDeferredLighting.LightGroupDrawTask[1024];

		// Token: 0x04002A1F RID: 10783
		private ushort _outerLightGroupDrawTasksCount;

		// Token: 0x04002A20 RID: 10784
		private ushort _innerLightGroupDrawTasksCount;

		// Token: 0x04002A21 RID: 10785
		private Mesh _sphereLightMesh;

		// Token: 0x04002A22 RID: 10786
		private Mesh _boxLightMesh;

		// Token: 0x04002A23 RID: 10787
		private Matrix _boxLightModelMatrix;

		// Token: 0x04002A24 RID: 10788
		private BoundingBox _globalLightBoundingBox;

		// Token: 0x04002A25 RID: 10789
		private readonly GraphicsDevice _graphics;

		// Token: 0x04002A26 RID: 10790
		private readonly GPUProgramStore _gpuProgramStore;

		// Token: 0x04002A27 RID: 10791
		private readonly RenderTargetStore _renderTargetStore;

		// Token: 0x04002A28 RID: 10792
		private readonly GLFunctions _gl;

		// Token: 0x02000E95 RID: 3733
		private struct LightDrawTask
		{
			// Token: 0x04004735 RID: 18229
			public Matrix ModelMatrix;

			// Token: 0x04004736 RID: 18230
			public BoundingSphere Sphere;

			// Token: 0x04004737 RID: 18231
			public Vector3 Color;
		}

		// Token: 0x02000E96 RID: 3734
		private struct LightGroupDrawTask
		{
			// Token: 0x04004738 RID: 18232
			public Matrix ModelMatrix;

			// Token: 0x04004739 RID: 18233
			public BoundingSphere Sphere;

			// Token: 0x0400473A RID: 18234
			public ushort LightIndexStart;

			// Token: 0x0400473B RID: 18235
			public ushort LightCount;
		}
	}
}
