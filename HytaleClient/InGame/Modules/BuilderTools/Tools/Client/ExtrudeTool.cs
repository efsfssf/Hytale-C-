using System;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Map;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x02000986 RID: 2438
	internal class ExtrudeTool : ClientTool
	{
		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x0014C664 File Offset: 0x0014A864
		public override string ToolId
		{
			get
			{
				return "Extrude";
			}
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x0014C66C File Offset: 0x0014A86C
		public ExtrudeTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._renderer = new BlockShapeRenderer(this._graphics, (int)this._graphics.GPUProgramStore.BasicProgram.AttribPosition.Index, (int)this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords.Index);
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x0014C6F2 File Offset: 0x0014A8F2
		protected override void DoDispose()
		{
			this._renderer.Dispose();
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x0014C704 File Offset: 0x0014A904
		public override void Update(float deltaTime)
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			HitDetection.RaycastHit raycastHit;
			bool flag = !this._gameInstance.HitDetection.RaycastBlock(lookRay.Position, lookRay.Direction, this._raycastOptions, out raycastHit);
			if (flag)
			{
				this._hasTarget = false;
			}
			else
			{
				bool flag2 = this._hasTarget && raycastHit.BlockPosition == this._target && raycastHit.Normal == this._normal;
				if (!flag2)
				{
					this._hasTarget = true;
					this._target = raycastHit.BlockPosition;
					this._normal = raycastHit.Normal;
					this.UpdateBlockShapeModelData(this._extrudeRadius);
				}
			}
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x0014C7C0 File Offset: 0x0014A9C0
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			bool flag = !base.BrushTarget.IsNaN();
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
				Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
				Vector3 value = this._gameInstance.SceneRenderer.Data.CameraDirection * 0.06f;
				Vector3 vector = -value;
				Vector3 vector2 = this._target - cameraPosition;
				Matrix matrix;
				Matrix.CreateTranslation(ref vector, out matrix);
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix, out matrix);
				Matrix matrix2;
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ProjectionMatrix, out matrix2);
				Matrix matrix3;
				Matrix.CreateTranslation(ref vector2, out matrix3);
				Matrix.Multiply(ref matrix3, ref viewProjectionMatrix, out matrix3);
				Matrix matrix4;
				Matrix.CreateTranslation(ref vector2, out matrix4);
				Matrix.Multiply(ref matrix4, ref matrix2, out matrix4);
				Vector3 zero = Vector3.Zero;
				Vector3 one = Vector3.One;
				float value2 = 0.3f;
				this._graphics.SaveColorMask();
				gl.DepthMask(true);
				gl.ColorMask(false, false, false, false);
				basicProgram.MVPMatrix.SetValue(ref matrix3);
				basicProgram.Color.SetValue(zero);
				basicProgram.Opacity.SetValue(value2);
				this._renderer.DrawBlockShape();
				gl.DepthMask(false);
				this._graphics.RestoreColorMask();
				this._renderer.DrawBlockShape();
				basicProgram.Color.SetValue(one);
				basicProgram.MVPMatrix.SetValue(ref matrix4);
				this._renderer.DrawBlockShapeOutline();
			}
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x0014C988 File Offset: 0x0014AB88
		public override bool NeedsDrawing()
		{
			return this._hasTarget;
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x0014C9A0 File Offset: 0x0014ABA0
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (!flag)
			{
				this._gameInstance.Connection.SendPacket(new BuilderToolExtrudeAction((int)this._target.X, (int)this._target.Y, (int)this._target.Z, (int)this._normal.X, (int)this._normal.Y, (int)this._normal.Z));
			}
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x0014CA18 File Offset: 0x0014AC18
		public override void OnToolItemChange(ClientItemStack itemStack)
		{
			BuilderTool toolFromItemStack = BuilderTool.GetToolFromItemStack(this._gameInstance, itemStack);
			bool flag;
			if (toolFromItemStack != null)
			{
				object obj;
				if (itemStack == null)
				{
					obj = null;
				}
				else
				{
					JObject metadata = itemStack.Metadata;
					obj = ((metadata != null) ? metadata["ToolData"] : null);
				}
				flag = (obj == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this._extrudeDepth = int.Parse(toolFromItemStack.GetItemArgValueOrDefault(ref itemStack, "ExtrudeDepth"));
				this._extrudeRadius = int.Parse(toolFromItemStack.GetItemArgValueOrDefault(ref itemStack, "ExtrudeRadius"));
				bool hasTarget = this._hasTarget;
				if (hasTarget)
				{
					this.UpdateBlockShapeModelData(this._extrudeRadius);
				}
			}
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x0014CAA8 File Offset: 0x0014ACA8
		private void UpdateBlockShapeModelData(int range)
		{
			Vector3 vector = new Vector3((float)(range * 2), (float)(range * 2), (float)(range * 2));
			Vector3 vector2 = new Vector3((float)(range * 2), (float)(range * 2), (float)(range * 2));
			Vector3 value = new Vector3((float)(-(float)range) - 0.5f, (float)(-(float)range) - 0.5f, (float)(-(float)range) - 0.5f);
			int num = Math.Abs(this._extrudeDepth);
			bool flag = this._normal.X != 0f;
			if (flag)
			{
				vector2.X = (float)num;
				vector.X = 1f;
				bool flag2 = this._normal.X > 0f;
				if (flag2)
				{
					value.X = 0.5f;
				}
				else
				{
					value.X = (float)this._extrudeDepth * this._normal.X - 0.5f;
				}
			}
			else
			{
				bool flag3 = this._normal.Y != 0f;
				if (flag3)
				{
					vector2.Y = (float)num;
					vector.Y = 1f;
					bool flag4 = this._normal.Y > 0f;
					if (flag4)
					{
						value.Y = 0.5f;
					}
					else
					{
						value.Y = (float)this._extrudeDepth * this._normal.Y - 0.5f;
					}
				}
				else
				{
					bool flag5 = this._normal.Z != 0f;
					if (flag5)
					{
						vector2.Z = (float)num;
						vector.Z = 1f;
						bool flag6 = this._normal.Z > 0f;
						if (flag6)
						{
							value.Z = 0.5f;
						}
						else
						{
							value.Z = (float)this._extrudeDepth * this._normal.Z - 0.5f;
						}
					}
				}
			}
			bool[,,] array = new bool[(int)vector2.X, (int)vector2.Y, (int)vector2.Z];
			bool[,,] blockTestedPositionData = new bool[(int)vector.X, (int)vector.Y, (int)vector.Z];
			Vector3 normal = this._normal;
			Vector3 vector3 = this._target + normal;
			Vector3 testDirection = Vector3.Negate(normal);
			this.FindAllBlocksForFace(array, blockTestedPositionData, vector3, vector3, testDirection);
			normal.X -= (float)array.GetLength(0) / 2f + 0.5f;
			normal.Y -= (float)array.GetLength(1) / 2f + 0.5f;
			normal.Z -= (float)array.GetLength(2) / 2f + 0.5f;
			IntVector3 intVector = new IntVector3(value + Vector3.One * 0.5f);
			this._renderer.UpdateModelData(array, intVector.X, intVector.Y, intVector.Z);
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0014CD84 File Offset: 0x0014AF84
		private void FindAllBlocksForFace(bool[,,] blockPositionData, bool[,,] blockTestedPositionData, Vector3 startWorldPosition, Vector3 currentWorldPosition, Vector3 testDirection)
		{
			Vector3 vector = currentWorldPosition - startWorldPosition;
			int length = blockTestedPositionData.GetLength(0);
			int length2 = blockTestedPositionData.GetLength(1);
			int length3 = blockTestedPositionData.GetLength(2);
			int num = length / 2 + (int)vector.X;
			int num2 = length2 / 2 + (int)vector.Y;
			int num3 = length3 / 2 + (int)vector.Z;
			bool flag = num < 0 || num >= length || num2 < 0 || num2 >= length2 || num3 < 0 || num3 >= length3;
			if (!flag)
			{
				bool flag2 = blockTestedPositionData[num, num2, num3];
				if (!flag2)
				{
					blockTestedPositionData[num, num2, num3] = true;
					int block = this._gameInstance.MapModule.GetBlock(currentWorldPosition, int.MaxValue);
					bool flag3 = block == int.MaxValue;
					if (!flag3)
					{
						ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
						bool flag4 = this._gameInstance.MapModule.ClientBlockTypes[block].CollisionMaterial == 1;
						if (!flag4)
						{
							int block2 = this._gameInstance.MapModule.GetBlock(currentWorldPosition + testDirection, int.MaxValue);
							bool flag5 = !this._gameInstance.MapModule.ClientBlockTypes[block2].IsOccluder && this._gameInstance.MapModule.ClientBlockTypes[block2].CollisionMaterial != 1;
							if (!flag5)
							{
								blockPositionData[num, num2, num3] = true;
								bool flag6 = testDirection.X != 0f && blockPositionData.GetLength(0) > 1;
								if (flag6)
								{
									for (int i = 1; i < blockPositionData.GetLength(0); i++)
									{
										blockPositionData[num + i, num2, num3] = true;
									}
								}
								else
								{
									bool flag7 = testDirection.Y != 0f && blockPositionData.GetLength(1) > 1;
									if (flag7)
									{
										for (int j = 1; j < blockPositionData.GetLength(1); j++)
										{
											blockPositionData[num, num2 + j, num3] = true;
										}
									}
									else
									{
										bool flag8 = testDirection.Z != 0f && blockPositionData.GetLength(2) > 1;
										if (flag8)
										{
											for (int k = 1; k < blockPositionData.GetLength(2); k++)
											{
												blockPositionData[num, num2, num3 + k] = true;
											}
										}
									}
								}
								for (int l = 0; l < 6; l++)
								{
									Vector3 normal = ChunkGeometryBuilder.AdjacentBlockOffsetsBySide[l].Normal;
									bool flag9 = (testDirection.X != 0f && normal.X != 0f) || (testDirection.Y != 0f && normal.Y != 0f) || (testDirection.Z != 0f && normal.Z != 0f);
									if (!flag9)
									{
										Vector3 currentWorldPosition2 = currentWorldPosition + normal;
										this.FindAllBlocksForFace(blockPositionData, blockTestedPositionData, startWorldPosition, currentWorldPosition2, testDirection);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040028A2 RID: 10402
		private const string DepthArgKey = "ExtrudeDepth";

		// Token: 0x040028A3 RID: 10403
		private const string RadiusArgKey = "ExtrudeRadius";

		// Token: 0x040028A4 RID: 10404
		private readonly HitDetection.RaycastOptions _raycastOptions = new HitDetection.RaycastOptions
		{
			IgnoreEmptyCollisionMaterial = true,
			IgnoreFluids = true,
			CheckOversizedBoxes = true,
			Distance = 150f
		};

		// Token: 0x040028A5 RID: 10405
		private readonly BlockShapeRenderer _renderer;

		// Token: 0x040028A6 RID: 10406
		private int _extrudeDepth;

		// Token: 0x040028A7 RID: 10407
		private int _extrudeRadius;

		// Token: 0x040028A8 RID: 10408
		private bool _hasTarget;

		// Token: 0x040028A9 RID: 10409
		private Vector3 _target;

		// Token: 0x040028AA RID: 10410
		private Vector3 _normal;
	}
}
