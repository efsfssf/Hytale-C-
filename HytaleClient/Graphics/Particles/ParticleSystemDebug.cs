using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000ABB RID: 2747
	internal class ParticleSystemDebug : Disposable
	{
		// Token: 0x060056A4 RID: 22180 RVA: 0x0019FB88 File Offset: 0x0019DD88
		public ParticleSystemDebug(GraphicsDevice graphics, ParticleSystem particleSystem)
		{
			this._graphics = graphics;
			this._particleSystem = particleSystem;
			this._systemPositionRenderer = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._systemPositionRenderer.UpdateModelData(SphereModel.BuildModelData(0.025f, 0.05f, 4, 4, 0f));
			this._attractorPositionRenderer = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._attractorPositionRenderer.UpdateModelData(BipyramidModel.BuildModelData(0.025f, 0.05f, 16));
			int num = 0;
			for (int i = 0; i < this._particleSystem.SpawnerGroups.Length; i++)
			{
				ref ParticleSystem.SystemSpawnerGroup ptr = ref this._particleSystem.SpawnerGroups[i];
				string particleSpawnerId = ptr.Settings.ParticleSpawnerId;
				this._spawnerColors[particleSpawnerId] = ParticleSystemDebug.Colors[num];
				num = ((num == ParticleSystemDebug.Colors.Length - 1) ? 0 : (num + 1));
				PrimitiveModelRenderer primitiveModelRenderer = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
				Vector3 vector = Vector3.Max(ptr.Settings.EmitOffsetMax, new Vector3(0.05f));
				primitiveModelRenderer.UpdateModelData(SphereModel.BuildModelData(vector.X, vector.Y * 2f, 12, 12, vector.Z));
				this._spawnerSpawnAreaRenderers[particleSpawnerId] = primitiveModelRenderer;
				PrimitiveModelRenderer primitiveModelRenderer2 = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
				vector = Vector3.Max(ptr.Settings.ParticleSpawnerSettings.EmitOffsetMax, new Vector3(0.05f));
				ParticleSpawnerSettings.Shape emitShape = ptr.Settings.ParticleSpawnerSettings.EmitShape;
				ParticleSpawnerSettings.Shape shape = emitShape;
				if (shape != ParticleSpawnerSettings.Shape.FullCube)
				{
					primitiveModelRenderer2.UpdateModelData(SphereModel.BuildModelData(vector.X, vector.Y * 2f, 24, 24, vector.Z));
				}
				else
				{
					primitiveModelRenderer2.UpdateModelData(CubeModel.BuildModelData(vector.X, vector.Y, vector.Z));
				}
				this._particleSpawnAreaRenderers[particleSpawnerId] = primitiveModelRenderer2;
				for (int j = 0; j < ptr.Settings.Attractors.Length; j++)
				{
					ref ParticleAttractor ptr2 = ref ptr.Settings.Attractors[j];
					PrimitiveModelRenderer primitiveModelRenderer3 = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
					bool flag = ptr2.RadialAxis == Vector3.Zero;
					if (flag)
					{
						primitiveModelRenderer3.UpdateModelData(SphereModel.BuildModelData(ptr2.Radius, ptr2.Radius * 2f, 12, 12, 0f));
					}
					else
					{
						primitiveModelRenderer3.UpdateModelData(CylinderModel.BuildModelData(ptr2.Radius, 300f, 12));
					}
					bool flag2 = j == 0;
					if (flag2)
					{
						this._groupAttractorRenderers[particleSpawnerId] = new List<PrimitiveModelRenderer>();
					}
					this._groupAttractorRenderers[ptr.Settings.ParticleSpawnerId].Add(primitiveModelRenderer3);
				}
				for (int k = 0; k < ptr.Settings.ParticleSpawnerSettings.Attractors.Length; k++)
				{
					ref ParticleAttractor ptr3 = ref ptr.Settings.ParticleSpawnerSettings.Attractors[k];
					PrimitiveModelRenderer primitiveModelRenderer4 = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
					bool flag3 = ptr3.RadialAxis == Vector3.Zero;
					if (flag3)
					{
						primitiveModelRenderer4.UpdateModelData(SphereModel.BuildModelData(ptr3.Radius, ptr3.Radius * 2f, 24, 24, 0f));
					}
					else
					{
						primitiveModelRenderer4.UpdateModelData(CylinderModel.BuildModelData(ptr3.Radius, 300f, 24));
					}
					bool flag4 = k == 0;
					if (flag4)
					{
						this._spawnerAttractorRenderers[ptr.Settings.ParticleSpawnerId] = new List<PrimitiveModelRenderer>();
					}
					this._spawnerAttractorRenderers[ptr.Settings.ParticleSpawnerId].Add(primitiveModelRenderer4);
				}
			}
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x001A0000 File Offset: 0x0019E200
		protected override void DoDispose()
		{
			this._systemPositionRenderer.Dispose();
			this._attractorPositionRenderer.Dispose();
			foreach (PrimitiveModelRenderer primitiveModelRenderer in this._spawnerSpawnAreaRenderers.Values)
			{
				primitiveModelRenderer.Dispose();
			}
			foreach (PrimitiveModelRenderer primitiveModelRenderer2 in this._particleSpawnAreaRenderers.Values)
			{
				primitiveModelRenderer2.Dispose();
			}
			foreach (List<PrimitiveModelRenderer> list in this._groupAttractorRenderers.Values)
			{
				foreach (PrimitiveModelRenderer primitiveModelRenderer3 in list)
				{
					primitiveModelRenderer3.Dispose();
				}
			}
			foreach (List<PrimitiveModelRenderer> list2 in this._spawnerAttractorRenderers.Values)
			{
				foreach (PrimitiveModelRenderer primitiveModelRenderer4 in list2)
				{
					primitiveModelRenderer4.Dispose();
				}
			}
			this._graphics = null;
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x001A01D4 File Offset: 0x0019E3D4
		public void Draw(Matrix viewProjectionMatrix)
		{
			Matrix transformMatrix = Matrix.CreateTranslation(this._particleSystem.Position);
			this._systemPositionRenderer.Draw(viewProjectionMatrix, transformMatrix, this._graphics.BlackColor, 1f, GL.ONE);
			for (int i = 0; i < this._particleSystem.SpawnerGroups.Length; i++)
			{
				ref ParticleSystem.SystemSpawnerGroup ptr = ref this._particleSystem.SpawnerGroups[i];
				for (int j = 0; j < ptr.Settings.Attractors.Length; j++)
				{
					ParticleAttractor particleAttractor = ptr.Settings.Attractors[j];
					transformMatrix = Matrix.CreateTranslation(this._particleSystem.Position + ptr.Settings.Attractors[j].Position);
					this._groupAttractorRenderers[ptr.Settings.ParticleSpawnerId][j].Draw(viewProjectionMatrix, transformMatrix, this._spawnerColors[ptr.Settings.ParticleSpawnerId], 1f, GL.ONE);
					this._attractorPositionRenderer.Draw(viewProjectionMatrix, transformMatrix, this._spawnerColors[ptr.Settings.ParticleSpawnerId], 1f, GL.ONE);
				}
				transformMatrix = Matrix.CreateTranslation(this._particleSystem.Position + ptr.Settings.PositionOffset);
				this._spawnerSpawnAreaRenderers[ptr.Settings.ParticleSpawnerId].Draw(viewProjectionMatrix, transformMatrix, this._graphics.BlackColor, 1f, GL.ONE);
			}
			for (int k = 0; k < this._particleSystem.AliveSpawnerCount; k++)
			{
				ref ParticleSystem.SystemSpawner ptr2 = ref this._particleSystem.SystemSpawners[k];
				ParticleSystemSettings.SystemSpawnerSettings settings = this._particleSystem.SpawnerGroups[ptr2.GroupId].Settings;
				string particleSpawnerId = settings.ParticleSpawnerId;
				Vector3 vector = this._particleSystem.Position + (ptr2.Position + settings.PositionOffset) * this._particleSystem.Scale;
				transformMatrix = Matrix.CreateTranslation(vector);
				this._particleSpawnAreaRenderers[particleSpawnerId].Draw(viewProjectionMatrix, transformMatrix, this._spawnerColors[particleSpawnerId], 1f, GL.ONE);
				for (int l = 0; l < settings.ParticleSpawnerSettings.Attractors.Length; l++)
				{
					ref ParticleAttractor ptr3 = ref settings.ParticleSpawnerSettings.Attractors[l];
					Vector3 vector2 = vector + ptr3.Position;
					transformMatrix = Matrix.CreateFromQuaternion(this._particleSystem.Rotation);
					Matrix.AddTranslation(ref transformMatrix, vector2.X, vector2.Y, vector2.Z);
					this._spawnerAttractorRenderers[particleSpawnerId][l].Draw(viewProjectionMatrix, transformMatrix, this._spawnerColors[particleSpawnerId], 1f, GL.ONE);
					this._attractorPositionRenderer.Draw(viewProjectionMatrix, transformMatrix, this._spawnerColors[particleSpawnerId], 1f, GL.ONE);
				}
			}
		}

		// Token: 0x040033FB RID: 13307
		private const float AttractorIndicatorSize = 0.05f;

		// Token: 0x040033FC RID: 13308
		private const float SpawnerIndicatorSize = 0.02f;

		// Token: 0x040033FD RID: 13309
		private const int GroupModelSegments = 12;

		// Token: 0x040033FE RID: 13310
		private const int SpawnerModelSegments = 24;

		// Token: 0x040033FF RID: 13311
		private static Vector3[] Colors = new Vector3[]
		{
			new Vector3(0.99215686f, 0.49803922f, 0.49803922f),
			new Vector3(0.6784314f, 1f, 0.54901963f),
			new Vector3(1f, 0.7647059f, 0.4745098f),
			new Vector3(0.44313726f, 0.75686276f, 0.99215686f),
			new Vector3(0.96862745f, 1f, 0.47058824f)
		};

		// Token: 0x04003400 RID: 13312
		private GraphicsDevice _graphics;

		// Token: 0x04003401 RID: 13313
		private readonly ParticleSystem _particleSystem;

		// Token: 0x04003402 RID: 13314
		private readonly Dictionary<string, Vector3> _spawnerColors = new Dictionary<string, Vector3>();

		// Token: 0x04003403 RID: 13315
		private readonly Dictionary<string, PrimitiveModelRenderer> _spawnerSpawnAreaRenderers = new Dictionary<string, PrimitiveModelRenderer>();

		// Token: 0x04003404 RID: 13316
		private readonly Dictionary<string, PrimitiveModelRenderer> _particleSpawnAreaRenderers = new Dictionary<string, PrimitiveModelRenderer>();

		// Token: 0x04003405 RID: 13317
		private readonly Dictionary<string, List<PrimitiveModelRenderer>> _groupAttractorRenderers = new Dictionary<string, List<PrimitiveModelRenderer>>();

		// Token: 0x04003406 RID: 13318
		private readonly Dictionary<string, List<PrimitiveModelRenderer>> _spawnerAttractorRenderers = new Dictionary<string, List<PrimitiveModelRenderer>>();

		// Token: 0x04003407 RID: 13319
		private readonly PrimitiveModelRenderer _systemPositionRenderer;

		// Token: 0x04003408 RID: 13320
		private readonly PrimitiveModelRenderer _attractorPositionRenderer;
	}
}
