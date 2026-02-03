using System;
using System.Collections.Generic;
using HytaleClient.Common.Collections;
using HytaleClient.Core;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x0200098C RID: 2444
	internal class PlaySelectionTool : ClientTool
	{
		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06004D96 RID: 19862 RVA: 0x0014F939 File Offset: 0x0014DB39
		public override string ToolId
		{
			get
			{
				return "PlaySelection";
			}
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x0014F940 File Offset: 0x0014DB40
		public override bool NeedsDrawing()
		{
			return this.SelectionArea.NeedsDrawing();
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0014F94D File Offset: 0x0014DB4D
		public override bool NeedsTextDrawing()
		{
			return this.SelectionArea.NeedsTextDrawing();
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x0014F95A File Offset: 0x0014DB5A
		// (set) Token: 0x06004D9A RID: 19866 RVA: 0x0014F962 File Offset: 0x0014DB62
		public bool IsCursorOverSelection { get; private set; } = false;

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0014F96B File Offset: 0x0014DB6B
		// (set) Token: 0x06004D9C RID: 19868 RVA: 0x0014F973 File Offset: 0x0014DB73
		public PlaySelectionTool.EditMode Mode { get; set; } = PlaySelectionTool.EditMode.None;

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x0014F97C File Offset: 0x0014DB7C
		// (set) Token: 0x06004D9E RID: 19870 RVA: 0x0014F984 File Offset: 0x0014DB84
		public PlaySelectionTool.EditMode HoverMode { get; set; } = PlaySelectionTool.EditMode.None;

		// Token: 0x06004D9F RID: 19871 RVA: 0x0014F990 File Offset: 0x0014DB90
		public PlaySelectionTool(GameInstance gameInstance) : base(gameInstance)
		{
			this.SelectionArea = gameInstance.BuilderToolsModule.SelectionArea;
			this._rotationGizmo = new RotationGizmo(this._graphics, this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont, new RotationGizmo.OnRotationChange(this.OnRotationChange), 1.5707964f);
			this._translationGizmo = new TranslationGizmo(this._graphics, new TranslationGizmo.OnPositionChange(this.OnPositionChange));
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x0014FBF6 File Offset: 0x0014DDF6
		protected override void DoDispose()
		{
			this._translationGizmo.Dispose();
			this._rotationGizmo.Dispose();
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0014FC14 File Offset: 0x0014DE14
		public override void Update(float deltaTime)
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			float targetBlockHitDistance = this._gameInstance.InteractionModule.HasFoundTargetBlock ? this._gameInstance.InteractionModule.TargetBlockHit.Distance : 0f;
			this._translationGizmo.Tick(lookRay);
			this._rotationGizmo.Tick(lookRay, targetBlockHitDistance);
			this._rotationGizmo.UpdateRotation(true);
			switch (this.Mode)
			{
			case PlaySelectionTool.EditMode.MoveBox:
				this.OnMove();
				break;
			case PlaySelectionTool.EditMode.MovePos1:
			case PlaySelectionTool.EditMode.MovePos2:
			case PlaySelectionTool.EditMode.ResizePos1:
			case PlaySelectionTool.EditMode.ResizePos2:
			{
				Ray lookRay2 = this._gameInstance.CameraModule.GetLookRay();
				this.target = lookRay2.Position + lookRay2.Direction * this._resizeDistance;
				this.target.X = (float)PlaySelectionTool.FloorInt(this.target.X);
				this.target.Y = (float)PlaySelectionTool.FloorInt(this.target.Y);
				this.target.Z = (float)PlaySelectionTool.FloorInt(this.target.Z);
				bool flag = this.Mode == PlaySelectionTool.EditMode.ResizePos1;
				if (flag)
				{
					this.SelectionArea.Position1 = this.target;
				}
				else
				{
					bool flag2 = this.Mode == PlaySelectionTool.EditMode.ResizePos2;
					if (flag2)
					{
						this.SelectionArea.Position2 = this.target;
					}
					else
					{
						bool flag3 = this.Mode == PlaySelectionTool.EditMode.MovePos1;
						if (flag3)
						{
							this.SelectionArea.Position1 = this.target;
							this.SelectionArea.Position2 = this.SelectionArea.Position1 + this._resizePosition2 - this._resizePosition1;
						}
						else
						{
							bool flag4 = this.Mode == PlaySelectionTool.EditMode.MovePos2;
							if (flag4)
							{
								this.SelectionArea.Position2 = this.target;
								this.SelectionArea.Position1 = this.SelectionArea.Position2 + this._resizePosition1 - this._resizePosition2;
							}
						}
					}
				}
				this.SelectionArea.IsSelectionDirty = true;
				break;
			}
			case PlaySelectionTool.EditMode.ResizeSide:
			case PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension:
			case PlaySelectionTool.EditMode.TranslateSide:
				this.OnResize();
				break;
			case PlaySelectionTool.EditMode.ExtrudeBlocksFromFace:
			{
				int extrusionDepth = this.GetExtrusionDepth();
				bool flag5 = extrusionDepth != this.currentExtrusionDepth;
				if (flag5)
				{
					this.currentExtrusionDepth = extrusionDepth;
					this.EnsureExtrudePreviewEntitiesMatchDepth(this.currentExtrusionDepth);
				}
				break;
			}
			case PlaySelectionTool.EditMode.CreateSelectionSet2DPlane:
			{
				Ray lookRay3 = this._gameInstance.CameraModule.GetLookRay();
				Vector3 planePoint = new Vector3(this.SelectionArea.Position1.X, this.SelectionArea.Position1.Y, this.SelectionArea.Position1.Z);
				bool flag6 = this.initialBlockNormal == Vector3.Right;
				if (flag6)
				{
					planePoint.X += 1f;
				}
				else
				{
					bool flag7 = this.initialBlockNormal == Vector3.Up;
					if (flag7)
					{
						planePoint.Y += 1f;
					}
					else
					{
						bool flag8 = this.initialBlockNormal == Vector3.Backward;
						if (flag8)
						{
							planePoint.Z += 1f;
						}
					}
				}
				Vector3 vector;
				bool flag9 = HitDetection.CheckRayPlaneIntersection(planePoint, new Vector3((float)(this.lockX ? 1 : 0), (float)(this.lockY ? 1 : 0), (float)(this.lockZ ? 1 : 0)), lookRay3.Position, lookRay3.Direction, out vector, false);
				bool flag10 = flag9;
				if (flag10)
				{
					this.target = vector;
				}
				this.target.X = (this.lockX ? this.SelectionArea.Position1.X : ((float)PlaySelectionTool.FloorInt(this.target.X)));
				this.target.Y = (this.lockY ? this.SelectionArea.Position1.Y : ((float)PlaySelectionTool.FloorInt(this.target.Y)));
				this.target.Z = (this.lockZ ? this.SelectionArea.Position1.Z : ((float)PlaySelectionTool.FloorInt(this.target.Z)));
				bool flag11 = this.SelectionArea.Position2 != this.target;
				if (flag11)
				{
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_SCALE");
				}
				this.SelectionArea.Position2 = this.target;
				this.SelectionArea.IsSelectionDirty = true;
				break;
			}
			case PlaySelectionTool.EditMode.TransformTranslationBrushPoint:
			{
				int num = PlaySelectionTool.FloorInt(this.SelectionArea.SelectionSize.Y / 2f + 0.5f);
				bool flag12 = !base.BrushTarget.IsNaN();
				if (flag12)
				{
					this.OnPositionChange(base.BrushTarget + new Vector3(0f, (float)num, 0f));
				}
				break;
			}
			}
			this.UpdateSelectionHighlight(false);
			this.UpdateGizmoSelection();
			bool flag13 = this._gameInstance.Input.IsAnyKeyHeld(false);
			if (flag13)
			{
				this.OnKeyDown();
			}
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x00150148 File Offset: 0x0014E348
		protected override void OnActiveStateChange(bool newState)
		{
			if (newState)
			{
				this.SelectionArea.RenderMode = SelectionArea.SelectionRenderMode.PlaySelection;
			}
			else
			{
				this.SelectionArea.RenderMode = SelectionArea.SelectionRenderMode.LegacySelection;
			}
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x00150178 File Offset: 0x0014E378
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = firstRun && interactionType == 1;
			if (flag)
			{
				this.CancelAllActions(context);
			}
			else
			{
				bool flag2 = this.PerformActions(interactionType, clickType, context, firstRun);
				if (!flag2)
				{
					this.wasLastInteractionFirstRun = firstRun;
				}
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001501BC File Offset: 0x0014E3BC
		public void CancelAllActions(InteractionContext context = null)
		{
			this.ExitTransformMode(true);
			this.Mode = PlaySelectionTool.EditMode.None;
			this.lockX = (this.lockY = (this.lockZ = false));
			this.renderSideGizmos = true;
			bool flag = context != null;
			if (flag)
			{
				context.State.State = 0;
			}
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x00150210 File Offset: 0x0014E410
		private bool PerformActions(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.SetPointsInteraction(interactionType, clickType, context, firstRun);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.SwapToOtherSelectionInteraction(interactionType, clickType, context, firstRun);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = this.SingleClickSetBasicCubeInteraction(interactionType, clickType, context, firstRun);
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = this.TranslationGizmoInteraction(interactionType, clickType, context, firstRun);
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = this.RotationGizmoInteraction(interactionType, clickType, context, firstRun);
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = this.TranslateBoxPositionWithPanelGizmo(interactionType, clickType, context, firstRun);
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = this.ExtrudeBlocksTouchingSelectionFaceWithPanelGizmo(interactionType, clickType, context, firstRun);
									if (flag7)
									{
										result = true;
									}
									else
									{
										bool flag8 = this.ExtendBoxSideWithPanelGizmo(interactionType, clickType, context, firstRun);
										if (flag8)
										{
											result = true;
										}
										else
										{
											bool flag9 = this.CreateSelectionDragOnInteract(interactionType, clickType, context, firstRun);
											result = flag9;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x001502E4 File Offset: 0x0014E4E4
		private bool TranslationGizmoInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.Mode == PlaySelectionTool.EditMode.TransformTranslationGizmos && (firstRun || clickType == InteractionModule.ClickType.None);
			bool result;
			if (flag)
			{
				this._translationGizmo.OnInteract(this._gameInstance.CameraModule.GetLookRay(), interactionType);
				bool flag2 = clickType == InteractionModule.ClickType.None;
				if (flag2)
				{
					this.lastBlockActivatedOnTranslation = this.ToIntVector(this.SelectionArea.CenterPos);
					this._translationGizmo.Show(this.SelectionArea.CenterPos, Vector3.Forward, null);
				}
				if (firstRun)
				{
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_WIDGET");
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x00150390 File Offset: 0x0014E590
		private bool RotationGizmoInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.Mode == PlaySelectionTool.EditMode.TransformRotation && (firstRun || clickType == InteractionModule.ClickType.None);
			bool result;
			if (flag)
			{
				this._rotationGizmo.OnInteract(interactionType);
				bool flag2 = clickType == InteractionModule.ClickType.None;
				if (flag2)
				{
					this.FinishRotationGizmoInteraction();
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x001503E4 File Offset: 0x0014E5E4
		private void FinishRotationGizmoInteraction()
		{
			this._rotationGizmo.Show(this.rotationOrigin, new Vector3?(Vector3.Zero), null, null);
			this._cumulativeRotationMatrix = Matrix.Multiply(this._cumulativeRotationMatrix, this._rotationMatrixSinceStartOfInteraction);
			Vector3 value = new Vector3(0f, 0.5f, 0f);
			for (int i = 0; i < this._entityOffsetFromRotationOrigin.Count; i++)
			{
				this._entityOffsetFromRotationOrigin[i] = this._previewEntities[i].Position + value - this.rotationOrigin;
			}
			this.positionOneOffsetFromRotationPoint = this.SelectionArea.Position1 - this.rotationOrigin + new Vector3(0.5f, 0.5f, 0.5f);
			this.positionTwoOffsetFromRotationPoint = this.SelectionArea.Position2 - this.rotationOrigin + new Vector3(0.5f, 0.5f, 0.5f);
			this.lastRotation = Vector3.NaN;
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x00150508 File Offset: 0x0014E708
		private bool SingleClickSetBasicCubeInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.IsEditModeATransformationMode(this.Mode);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = base.BrushTarget.IsNaN();
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = firstRun && this.Mode == PlaySelectionTool.EditMode.None && !this.IsCursorOverSelection;
					if (flag3)
					{
						this.wasLastInteractionFirstRun = true;
					}
					else
					{
						bool flag4 = this.wasLastInteractionFirstRun && clickType == InteractionModule.ClickType.None;
						if (flag4)
						{
							this.wasLastInteractionFirstRun = false;
							this.Mode = PlaySelectionTool.EditMode.None;
							this.SelectionArea.Position1 = base.BrushTarget;
							this.SelectionArea.Position2 = base.BrushTarget;
							this.SelectionArea.IsSelectionDirty = true;
							this.SelectionArea.Update();
							this.SelectionArea.OnSelectionChange();
							return true;
						}
						this.wasLastInteractionFirstRun = false;
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x001505EC File Offset: 0x0014E7EC
		private bool SetPointsInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.IsEditModeATransformationMode(this.Mode);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !firstRun || !this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_E, false) || base.BrushTarget.IsNaN();
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = interactionType == 0;
					if (flag3)
					{
						this.SelectionArea.Position1 = base.BrushTarget;
					}
					else
					{
						this.SelectionArea.Position2 = base.BrushTarget;
					}
					this.SelectionArea.IsSelectionDirty = true;
					this.SelectionArea.OnSelectionChange();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0015068C File Offset: 0x0014E88C
		private bool SwapToOtherSelectionInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.IsEditModeATransformationMode(this.Mode);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !firstRun || !this._gameInstance.Input.IsShiftHeld();
				if (flag2)
				{
					result = false;
				}
				else
				{
					float num = -1f;
					int num2 = -1;
					Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
					for (int i = 0; i < 8; i++)
					{
						bool flag3 = this.SelectionArea.SelectionData[i] != null;
						if (flag3)
						{
							HitDetection.RayBoxCollision rayBoxCollision;
							bool flag4 = HitDetection.CheckRayBoxCollision(this.SelectionArea.SelectionData[i].Item3, lookRay.Position, lookRay.Direction, out rayBoxCollision, true);
							if (flag4)
							{
								float num3 = Vector3.Distance(lookRay.Position, rayBoxCollision.Position);
								bool flag5 = num2 == -1 || num3 < num;
								if (flag5)
								{
									num2 = i;
									num = num3;
								}
							}
						}
					}
					bool flag6 = num2 > -1;
					if (flag6)
					{
						this.SelectionArea.SetSelectionIndex(num2);
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x001507A8 File Offset: 0x0014E9A8
		private bool ExtrudeBlocksTouchingSelectionFaceWithPanelGizmo(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = firstRun && this._gameInstance.Input.IsAltHeld() && this.Mode == PlaySelectionTool.EditMode.None && interactionType == null && !this.gizmoNormal.IsNaN();
			bool result;
			if (flag)
			{
				this.Mode = PlaySelectionTool.EditMode.ExtrudeBlocksFromFace;
				this.InitializeFaceGizmoOperation();
				this.SetExtrusionCoordinateAndPreviewOffsets();
				result = true;
			}
			else
			{
				bool flag2 = clickType == InteractionModule.ClickType.None && this.Mode == PlaySelectionTool.EditMode.ExtrudeBlocksFromFace;
				if (flag2)
				{
					context.State.State = 0;
					this.Mode = PlaySelectionTool.EditMode.None;
					this._gameInstance.Connection.SendPacket(new BuilderToolStackArea(this.minExtrusionCoordinate.ToBlockPosition(), this.maxExtrusionCoordinate.ToBlockPosition(), PlaySelectionTool.FloorInt(this.gizmoNormal.X), PlaySelectionTool.FloorInt(this.gizmoNormal.Y), PlaySelectionTool.FloorInt(this.gizmoNormal.Z), this.currentExtrusionDepth));
					this.EnsureExtrudePreviewEntitiesMatchDepth(0);
					result = true;
				}
				else
				{
					bool flag3 = this.Mode == PlaySelectionTool.EditMode.ExtrudeBlocksFromFace;
					if (flag3)
					{
						context.State.State = 4;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x001508C8 File Offset: 0x0014EAC8
		private void EnsureExtrudePreviewEntitiesMatchDepth(int depthOfExtrusion)
		{
			bool flag = this.extrudeLayerPreviewEntities.Count == depthOfExtrusion;
			if (!flag)
			{
				while (this.extrudeLayerPreviewEntities.Count > depthOfExtrusion)
				{
					List<Entity> list = this.extrudeLayerPreviewEntities[this.extrudeLayerPreviewEntities.Count - 1];
					foreach (Entity entity in list)
					{
						entity.IsVisible = false;
						this._gameInstance.EntityStoreModule.Despawn(entity.NetworkId);
					}
					this.extrudeLayerPreviewEntities.RemoveAt(this.extrudeLayerPreviewEntities.Count - 1);
				}
				int networkEffectIndex;
				this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue("PrototypePastePreview", out networkEffectIndex);
				Vector3 vector = new Vector3(0.5f, 0f, 0.5f);
				while (this.extrudeLayerPreviewEntities.Count < depthOfExtrusion)
				{
					List<Entity> list2 = new List<Entity>();
					int num = this.extrudeLayerPreviewEntities.Count + 1;
					IntVector3 intVector = new IntVector3(PlaySelectionTool.FloorInt(this.gizmoNormal.X * (float)num), PlaySelectionTool.FloorInt(this.gizmoNormal.Y * (float)num), PlaySelectionTool.FloorInt(this.gizmoNormal.Z * (float)num));
					foreach (KeyValuePair<IntVector3, int> keyValuePair in this.extrusionPreviewEntityOffsets)
					{
						Vector3 positionTeleport = new Vector3((float)(this.minExtrusionCoordinate.X + keyValuePair.Key.X + intVector.X) + vector.X, (float)(this.minExtrusionCoordinate.Y + keyValuePair.Key.Y + intVector.Y) + vector.Y, (float)(this.minExtrusionCoordinate.Z + keyValuePair.Key.Z + intVector.Z) + vector.Z);
						Entity entity2;
						this._gameInstance.EntityStoreModule.Spawn(-1, out entity2);
						entity2.SetIsTangible(false);
						entity2.SetBlock(keyValuePair.Value);
						entity2.AddEffect(networkEffectIndex, null, null, null);
						entity2.SetPositionTeleport(positionTeleport);
						list2.Add(entity2);
					}
					this.extrudeLayerPreviewEntities.Add(list2);
				}
			}
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x00150B9C File Offset: 0x0014ED9C
		public int GetExtrusionDepth()
		{
			bool flag = !this.SelectionArea.IsSelectionDefined();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				Vector3 projectedCursorPosition = this.GetProjectedCursorPosition();
				int num = -1;
				bool flag2 = this._resizeDirection == PlaySelectionTool.Direction.Up || this._resizeDirection == PlaySelectionTool.Direction.Down;
				if (flag2)
				{
					num = PlaySelectionTool.FloorInt(Math.Abs(projectedCursorPosition.Y - (float)this.minExtrusionCoordinate.Y));
				}
				else
				{
					bool flag3 = this._resizeDirection == PlaySelectionTool.Direction.Left || this._resizeDirection == PlaySelectionTool.Direction.Right;
					if (flag3)
					{
						num = PlaySelectionTool.FloorInt(Math.Abs(projectedCursorPosition.X - (float)this.minExtrusionCoordinate.X));
					}
					else
					{
						bool flag4 = this._resizeDirection == PlaySelectionTool.Direction.Forward || this._resizeDirection == PlaySelectionTool.Direction.Backward;
						if (flag4)
						{
							num = PlaySelectionTool.FloorInt(Math.Abs(projectedCursorPosition.Z - (float)this.minExtrusionCoordinate.Z));
						}
					}
				}
				bool flag5 = num == -1;
				if (flag5)
				{
					result = 0;
				}
				else
				{
					result = MathHelper.Clamp(num, 0, 25);
				}
			}
			return result;
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x00150C9C File Offset: 0x0014EE9C
		private void SetExtrusionCoordinateAndPreviewOffsets()
		{
			this.currentExtrusionDepth = 0;
			Vector3 vector = Vector3.Min(this.SelectionArea.Position1, this.SelectionArea.Position2);
			Vector3 vector2 = Vector3.Max(this.SelectionArea.Position1, this.SelectionArea.Position2);
			this.minExtrusionCoordinate = new IntVector3(PlaySelectionTool.FloorInt(vector.X), PlaySelectionTool.FloorInt(vector.Y), PlaySelectionTool.FloorInt(vector.Z));
			this.maxExtrusionCoordinate = new IntVector3(PlaySelectionTool.FloorInt(vector2.X), PlaySelectionTool.FloorInt(vector2.Y), PlaySelectionTool.FloorInt(vector2.Z));
			bool flag = this.gizmoNormal == Vector3.Right;
			if (flag)
			{
				this.minExtrusionCoordinate.X = PlaySelectionTool.FloorInt(vector2.X);
			}
			else
			{
				bool flag2 = this.gizmoNormal == Vector3.Left;
				if (flag2)
				{
					this.maxExtrusionCoordinate.X = PlaySelectionTool.FloorInt(vector.X);
				}
				else
				{
					bool flag3 = this.gizmoNormal == Vector3.Up;
					if (flag3)
					{
						this.minExtrusionCoordinate.Y = PlaySelectionTool.FloorInt(vector2.Y);
					}
					else
					{
						bool flag4 = this.gizmoNormal == Vector3.Down;
						if (flag4)
						{
							this.maxExtrusionCoordinate.Y = PlaySelectionTool.FloorInt(vector.Y);
						}
						else
						{
							bool flag5 = this.gizmoNormal == Vector3.Backward;
							if (flag5)
							{
								this.minExtrusionCoordinate.Z = PlaySelectionTool.FloorInt(vector2.Z);
							}
							else
							{
								bool flag6 = this.gizmoNormal == Vector3.Forward;
								if (!flag6)
								{
									return;
								}
								this.maxExtrusionCoordinate.Z = PlaySelectionTool.FloorInt(vector.Z);
							}
						}
					}
				}
			}
			this.extrusionPreviewEntityOffsets.Clear();
			this.extrudeLayerPreviewEntities.Clear();
			for (int i = this.minExtrusionCoordinate.X; i <= this.maxExtrusionCoordinate.X; i++)
			{
				for (int j = this.minExtrusionCoordinate.Y; j <= this.maxExtrusionCoordinate.Y; j++)
				{
					for (int k = this.minExtrusionCoordinate.Z; k <= this.maxExtrusionCoordinate.Z; k++)
					{
						int block = this._gameInstance.MapModule.GetBlock(i, j, k, int.MaxValue);
						bool flag7 = this._gameInstance.MapModule.ClientBlockTypes[block].DrawType == null || block == int.MaxValue;
						if (!flag7)
						{
							IntVector3 key = new IntVector3(i - this.minExtrusionCoordinate.X, j - this.minExtrusionCoordinate.Y, k - this.minExtrusionCoordinate.Z);
							this.extrusionPreviewEntityOffsets[key] = block;
						}
					}
				}
			}
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x00150FA4 File Offset: 0x0014F1A4
		private bool TranslateBoxPositionWithPanelGizmo(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = firstRun && this._gameInstance.Input.IsShiftHeld() && this.Mode == PlaySelectionTool.EditMode.None && interactionType == null && !this.gizmoNormal.IsNaN();
			bool result;
			if (flag)
			{
				this.Mode = PlaySelectionTool.EditMode.MoveBox;
				this.InitializeFaceGizmoOperation();
				this.SelectionArea.IsSelectionDirty = true;
				result = true;
			}
			else
			{
				bool flag2 = clickType == InteractionModule.ClickType.None && this.Mode == PlaySelectionTool.EditMode.MoveBox;
				if (flag2)
				{
					context.State.State = 0;
					this.Mode = PlaySelectionTool.EditMode.None;
					this.SelectionArea.IsSelectionDirty = true;
					this.SelectionArea.Update();
					this.SelectionArea.OnSelectionChange();
					result = true;
				}
				else
				{
					bool flag3 = this.Mode == PlaySelectionTool.EditMode.MoveBox;
					if (flag3)
					{
						context.State.State = 4;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x0015107C File Offset: 0x0014F27C
		private bool ExtendBoxSideWithPanelGizmo(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			if (firstRun)
			{
				bool flag = this.Mode == PlaySelectionTool.EditMode.None && interactionType == null && !this.gizmoNormal.IsNaN();
				if (flag)
				{
					context.State.State = 0;
					this.Mode = PlaySelectionTool.EditMode.TranslateSide;
					this.InitializeFaceGizmoOperation();
					this.SelectionArea.IsSelectionDirty = true;
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_WIDGET");
					return true;
				}
			}
			bool flag2 = clickType == InteractionModule.ClickType.None && this.Mode == PlaySelectionTool.EditMode.TranslateSide;
			bool result;
			if (flag2)
			{
				context.State.State = 0;
				this.Mode = PlaySelectionTool.EditMode.None;
				this.SelectionArea.IsSelectionDirty = true;
				this.SelectionArea.OnSelectionChange();
				this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_PLACE");
				result = true;
			}
			else
			{
				bool flag3 = this.Mode == PlaySelectionTool.EditMode.TranslateSide;
				if (flag3)
				{
					context.State.State = 4;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0015117C File Offset: 0x0014F37C
		public void InitializeFaceGizmoOperation()
		{
			this._resizeOrigin = this.gizmoPosition;
			this._resizeNormal = this.gizmoNormal;
			this._resizeDirection = PlaySelectionTool.GetVectorDirection(this._resizeNormal);
			this._resizePosition1 = this.SelectionArea.Position1;
			this._resizePosition2 = this.SelectionArea.Position2;
			bool flag = this.gizmoNormal == Vector3.Right;
			if (flag)
			{
				this.resizePoint = ((this.SelectionArea.Position1.X > this.SelectionArea.Position2.X) ? PlaySelectionTool.SelectionPoint.PosOne : PlaySelectionTool.SelectionPoint.PosTwo);
			}
			else
			{
				bool flag2 = this.gizmoNormal == Vector3.Left;
				if (flag2)
				{
					this.resizePoint = ((this.SelectionArea.Position1.X < this.SelectionArea.Position2.X) ? PlaySelectionTool.SelectionPoint.PosOne : PlaySelectionTool.SelectionPoint.PosTwo);
				}
				else
				{
					bool flag3 = this.gizmoNormal == Vector3.Up;
					if (flag3)
					{
						this.resizePoint = ((this.SelectionArea.Position1.Y > this.SelectionArea.Position2.Y) ? PlaySelectionTool.SelectionPoint.PosOne : PlaySelectionTool.SelectionPoint.PosTwo);
					}
					else
					{
						bool flag4 = this.gizmoNormal == Vector3.Down;
						if (flag4)
						{
							this.resizePoint = ((this.SelectionArea.Position1.Y < this.SelectionArea.Position2.Y) ? PlaySelectionTool.SelectionPoint.PosOne : PlaySelectionTool.SelectionPoint.PosTwo);
						}
						else
						{
							bool flag5 = this.gizmoNormal == Vector3.Backward;
							if (flag5)
							{
								this.resizePoint = ((this.SelectionArea.Position1.Z > this.SelectionArea.Position2.Z) ? PlaySelectionTool.SelectionPoint.PosOne : PlaySelectionTool.SelectionPoint.PosTwo);
							}
							else
							{
								bool flag6 = this.gizmoNormal == Vector3.Forward;
								if (flag6)
								{
									this.resizePoint = ((this.SelectionArea.Position1.Z < this.SelectionArea.Position2.Z) ? PlaySelectionTool.SelectionPoint.PosOne : PlaySelectionTool.SelectionPoint.PosTwo);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x00151370 File Offset: 0x0014F570
		private bool CreateSelectionDragOnInteract(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = this.IsEditModeATransformationMode(this.Mode);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				if (firstRun)
				{
					bool flag2 = this.Mode == PlaySelectionTool.EditMode.None && interactionType == null && !this.IsCursorOverSelection;
					if (flag2)
					{
						bool flag3 = base.BrushTarget.IsNaN();
						if (flag3)
						{
							return false;
						}
						Vector3 brushTarget = base.BrushTarget;
						this.SelectionArea.Position1 = (this.SelectionArea.Position2 = brushTarget);
						this.Mode = PlaySelectionTool.EditMode.CreateSelectionSet2DPlane;
						this.lockX = (this.lockY = (this.lockZ = false));
						context.State.State = 4;
						this.initialBlockNormal = this._gameInstance.InteractionModule.TargetBlockHit.BlockNormal;
						bool flag4 = this.initialBlockNormal.X == 1f || this.initialBlockNormal.X == -1f;
						if (flag4)
						{
							this.lockX = true;
						}
						else
						{
							bool flag5 = this.initialBlockNormal.Y == 1f || this.initialBlockNormal.Y == -1f;
							if (flag5)
							{
								this.lockY = true;
							}
							else
							{
								bool flag6 = this.initialBlockNormal.Z == 1f || this.initialBlockNormal.Z == -1f;
								if (flag6)
								{
									this.lockZ = true;
								}
							}
						}
						this.SelectionArea.IsSelectionDirty = true;
						this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_WIDGET");
						return true;
					}
					else
					{
						bool flag7 = this.Mode == PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension;
						if (flag7)
						{
							this.Mode = PlaySelectionTool.EditMode.None;
							this.lockX = (this.lockY = (this.lockZ = false));
							context.State.State = 0;
							this.SelectionArea.IsSelectionDirty = true;
							this.SelectionArea.OnSelectionChange();
							this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_PLACE");
							return true;
						}
					}
				}
				bool flag8 = clickType == InteractionModule.ClickType.None && this.Mode == PlaySelectionTool.EditMode.CreateSelectionSet2DPlane;
				if (flag8)
				{
					context.State.State = 0;
					this.Mode = PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension;
					Vector3 position = this._gameInstance.CameraModule.Controller.Position;
					Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
					Quaternion rotation2 = Quaternion.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f);
					Vector3 direction = Vector3.Transform(Vector3.Forward, rotation2);
					HitDetection.CheckRayBoxCollision(this.SelectionArea.GetBoundsExclusiveMax(), position, direction, out this._rayBoxHit, true);
					this._resizeOrigin = this._rayBoxHit.Position;
					this._resizeNormal = (this.flipXZSelectionExpansionAxis ? this.initialBlockNormal.Sign(new Vector3(-1f, 1f, -1f)) : this.initialBlockNormal);
					this._resizeDirection = PlaySelectionTool.GetVectorDirection(this._resizeNormal);
					this._resizePosition1 = this.SelectionArea.Position1;
					this._resizePosition2 = this.SelectionArea.Position2;
					this.lockX = (this.lockY = (this.lockZ = false));
					this.SelectionArea.IsSelectionDirty = true;
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_PLACE");
					result = true;
				}
				else
				{
					bool flag9 = this.Mode == PlaySelectionTool.EditMode.CreateSelectionSet2DPlane;
					if (flag9)
					{
						context.State.State = 4;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x00151714 File Offset: 0x0014F914
		public IntVector3 ToIntVector(Vector3 vector3)
		{
			return new IntVector3(PlaySelectionTool.FloorInt(vector3.X + 0.1f), PlaySelectionTool.FloorInt(vector3.Y + 0.1f), PlaySelectionTool.FloorInt(vector3.Z + 0.1f));
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x00151760 File Offset: 0x0014F960
		public void EnterTransformMode(PlaySelectionTool.EditMode editModeToEnter, bool cutOriginal = true, EditorBlocksChange.BlockChange[] clipboard = null)
		{
			bool flag = this.Mode > PlaySelectionTool.EditMode.None;
			if (!flag)
			{
				bool flag2 = !this.IsEditModeATransformationMode(editModeToEnter);
				if (!flag2)
				{
					this._gameInstance.Connection.SendPacket(new BuilderToolSetTransformationModeState(true));
					this._translationMatrix = Matrix.Identity;
					this._rotationMatrixSinceStartOfInteraction = Matrix.Identity;
					this._cumulativeRotationMatrix = Matrix.Identity;
					this.shouldCutOriginal = cutOriginal;
					this.lastRotation = Vector3.Zero;
					bool flag3 = clipboard != null && clipboard.Length != 0;
					if (flag3)
					{
						this.initialPasteLocationForPasteMode = this.ToIntVector(this._gameInstance.BuilderToolsModule.BrushTargetPosition) + new IntVector3(0, 1, 0);
						int num = 0;
						foreach (EditorBlocksChange.BlockChange blockChange in clipboard)
						{
							bool flag4 = blockChange.Y < 0 && Math.Abs(blockChange.Y) > num;
							if (flag4)
							{
								num = Math.Abs(blockChange.Y);
							}
						}
						Vector3 vector = new Vector3((float)clipboard[0].X, (float)clipboard[0].Y, (float)clipboard[0].Z);
						Vector3 vector2 = new Vector3((float)clipboard[0].X, (float)clipboard[0].Y, (float)clipboard[0].Z);
						for (int j = 0; j < clipboard.Length; j++)
						{
							clipboard[j].Y += num;
							EditorBlocksChange.BlockChange blockChange2 = clipboard[j];
							Vector3 value = new Vector3((float)blockChange2.X, (float)blockChange2.Y, (float)blockChange2.Z);
							vector = Vector3.Min(vector, value);
							vector2 = Vector3.Max(vector2, value);
						}
						this.SelectionArea.Position1 = this.initialPasteLocationForPasteMode.ToVector3() + vector2;
						this.SelectionArea.Position2 = this.initialPasteLocationForPasteMode.ToVector3() + vector;
						this.SelectionArea.IsSelectionDirty = true;
						this.SelectionArea.Update();
					}
					this.rotationOrigin = this.SelectionArea.CenterPos + this.GetRotationGizmoOffsetFromDimensions(this.SelectionArea.SelectionSize);
					this.initialRotationOrigin = new Vector3(this.rotationOrigin.X, this.rotationOrigin.Y, this.rotationOrigin.Z);
					this.positionOneOffsetFromRotationPoint = this.SelectionArea.Position1 - this.rotationOrigin + new Vector3(0.5f, 0.5f, 0.5f);
					this.positionTwoOffsetFromRotationPoint = this.SelectionArea.Position2 - this.rotationOrigin + new Vector3(0.5f, 0.5f, 0.5f);
					this.lastBlockActivatedOnTranslation = this.ToIntVector(this.SelectionArea.CenterPos);
					BoundingBox bounds = this.SelectionArea.GetBounds();
					this._minPositionAtBeginningOfTransform = this.ToIntVector(bounds.Min);
					this._maxPositionAtBeginningOfTransform = this.ToIntVector(bounds.Max);
					this.renderSideGizmos = false;
					bool flag5 = clipboard != null;
					if (flag5)
					{
						this.CreateBlockEntityPreview(clipboard);
					}
					else
					{
						this.CreateBlockEntityPreview(null);
					}
					this.SwapTransformMode(editModeToEnter);
				}
			}
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x00151A9C File Offset: 0x0014FC9C
		public void SwapTransformMode(PlaySelectionTool.EditMode editModeToSwitchTo)
		{
			bool flag = !this.IsEditModeATransformationMode(editModeToSwitchTo);
			if (!flag)
			{
				bool flag2 = editModeToSwitchTo == PlaySelectionTool.EditMode.TransformRotation;
				if (flag2)
				{
					this._translationGizmo.Hide();
					this._rotationGizmo.Show(this.rotationOrigin, new Vector3?(Vector3.Zero), null, null);
					this.Mode = PlaySelectionTool.EditMode.TransformRotation;
				}
				else
				{
					bool flag3 = editModeToSwitchTo == PlaySelectionTool.EditMode.TransformTranslationGizmos;
					if (flag3)
					{
						this._rotationGizmo.Hide();
						this._translationGizmo.Show(this.SelectionArea.CenterPos, Vector3.Forward, null);
						this.Mode = PlaySelectionTool.EditMode.TransformTranslationGizmos;
						this.lastBlockActivatedOnTranslation = this.ToIntVector(this.SelectionArea.CenterPos);
					}
					else
					{
						bool flag4 = editModeToSwitchTo == PlaySelectionTool.EditMode.TransformTranslationBrushPoint;
						if (flag4)
						{
							this._rotationGizmo.Hide();
							this._translationGizmo.Hide();
							this.Mode = PlaySelectionTool.EditMode.TransformTranslationBrushPoint;
							this.lastBlockActivatedOnTranslation = this.ToIntVector(this.SelectionArea.CenterPos);
						}
					}
				}
			}
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x00151BA4 File Offset: 0x0014FDA4
		public bool IsInTransformationMode()
		{
			return this.IsEditModeATransformationMode(this.Mode);
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x00151BC4 File Offset: 0x0014FDC4
		private bool IsEditModeATransformationMode(PlaySelectionTool.EditMode editMode)
		{
			return editMode == PlaySelectionTool.EditMode.TransformRotation || editMode == PlaySelectionTool.EditMode.TransformTranslationGizmos || editMode == PlaySelectionTool.EditMode.TransformTranslationBrushPoint;
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x00151BE8 File Offset: 0x0014FDE8
		public void ExitTransformMode(bool cancelTransform)
		{
			bool flag = !this.IsInTransformationMode();
			if (!flag)
			{
				if (cancelTransform)
				{
					this.SelectionArea.Position1 = this._maxPositionAtBeginningOfTransform.ToVector3();
					this.SelectionArea.Position2 = this._minPositionAtBeginningOfTransform.ToVector3();
					this.SelectionArea.IsSelectionDirty = true;
					foreach (KeyValuePair<IntVector3, int> keyValuePair in this.blockIdsAtLocationOfGizmoEdit)
					{
						this._gameInstance.MapModule.SetClientBlock(keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z, keyValuePair.Value);
					}
					this._gameInstance.Connection.SendPacket(new BuilderToolSetTransformationModeState(false));
				}
				else
				{
					this.ConfirmTransformationModePlacement(true, true);
				}
				this._translationGizmo.Hide();
				this._rotationGizmo.Hide();
				for (int i = 0; i < this._previewEntities.Count; i++)
				{
					Entity entity = this._previewEntities[i];
					entity.IsVisible = false;
					this._gameInstance.EntityStoreModule.Despawn(entity.NetworkId);
				}
				this._previewEntities.Clear();
				this.initialPasteLocationForPasteMode = IntVector3.Min;
				this.renderSideGizmos = true;
				this.Mode = PlaySelectionTool.EditMode.None;
			}
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x00151D7C File Offset: 0x0014FF7C
		public void ConfirmTransformationModePlacement(bool applyTransformationToSelectionMinMax = false, bool isExitingTransformMode = false)
		{
			bool flag = !this.IsInTransformationMode();
			if (!flag)
			{
				BlockPosition blockPosition = null;
				bool flag2 = !this.initialPasteLocationForPasteMode.Equals(IntVector3.Min);
				if (flag2)
				{
					blockPosition = new BlockPosition(this.initialPasteLocationForPasteMode.X, this.initialPasteLocationForPasteMode.Y, this.initialPasteLocationForPasteMode.Z);
				}
				Matrix matrix = Matrix.Multiply(this._cumulativeRotationMatrix, this._translationMatrix);
				this._gameInstance.Connection.SendPacket(new BuilderToolSelectionTransform(Matrix.ToFlatFloatArray(matrix), this._minPositionAtBeginningOfTransform.ToBlockPosition(), this._maxPositionAtBeginningOfTransform.ToBlockPosition(), this.initialRotationOrigin.ToProtocolVector3f(), this.shouldCutOriginal, applyTransformationToSelectionMinMax, isExitingTransformMode, blockPosition));
			}
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x00151E38 File Offset: 0x00150038
		public float SnapRadianTo90Degrees(float radian)
		{
			return (float)(1.5707963705062866 * Math.Round((double)(radian / 1.5707964f)));
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x00151E64 File Offset: 0x00150064
		private void OnRotationChange(Vector3 rotation)
		{
			Vector3 vector = new Vector3(this.SnapRadianTo90Degrees(rotation.Pitch), this.SnapRadianTo90Degrees(rotation.Yaw), this.SnapRadianTo90Degrees(rotation.Roll));
			bool flag = vector.Equals(this.lastRotation);
			if (!flag)
			{
				this.lastRotation = vector;
				this._rotationMatrixSinceStartOfInteraction = Matrix.CreateFromYawPitchRoll(this.lastRotation.Yaw, this.lastRotation.Pitch, this.lastRotation.Roll);
				for (int i = 0; i < this._previewEntities.Count; i++)
				{
					Vector3 vector2 = this.rotationOrigin + Vector3.Transform(this._entityOffsetFromRotationOrigin[i], this._rotationMatrixSinceStartOfInteraction);
					Vector3 positionTeleport = new Vector3(vector2.X, (float)Math.Floor((double)vector2.Y), vector2.Z);
					this._previewEntities[i].SetPositionTeleport(positionTeleport);
				}
				this.SelectionArea.Position1 = Vector3.Transform(this.positionOneOffsetFromRotationPoint, this._rotationMatrixSinceStartOfInteraction) + this.rotationOrigin - new Vector3(0.5f, 0.5f, 0.5f);
				this.SelectionArea.Position2 = Vector3.Transform(this.positionTwoOffsetFromRotationPoint, this._rotationMatrixSinceStartOfInteraction) + this.rotationOrigin - new Vector3(0.5f, 0.5f, 0.5f);
				this.SelectionArea.IsSelectionDirty = true;
				this.SelectionArea.Update();
			}
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x00151FF8 File Offset: 0x001501F8
		private void OnPositionChange(Vector3 translatedTo)
		{
			bool flag = this.lastBlockActivatedOnTranslation.Equals(IntVector3.Zero);
			if (flag)
			{
				this.lastBlockActivatedOnTranslation = this.ToIntVector(this.SelectionArea.CenterPos);
			}
			IntVector3 value = this.ToIntVector(translatedTo);
			IntVector3 intVector = value - this.lastBlockActivatedOnTranslation;
			bool flag2 = intVector.X >= 1 || intVector.Y >= 1 || intVector.Z >= 1 || intVector.X <= -1 || intVector.Y <= -1 || intVector.Z <= -1;
			if (flag2)
			{
				this.lastBlockActivatedOnTranslation = value;
				this.SelectionArea.Position1 = (this.ToIntVector(this.SelectionArea.Position1) + intVector).ToVector3();
				this.SelectionArea.Position2 = (this.ToIntVector(this.SelectionArea.Position2) + intVector).ToVector3();
				Matrix.AddTranslation(ref this._translationMatrix, (float)intVector.X, (float)intVector.Y, (float)intVector.Z);
				Vector3 value2 = intVector.ToVector3();
				foreach (Entity entity in this._previewEntities)
				{
					entity.SetPositionTeleport(entity.NextPosition + value2);
				}
				this.rotationOrigin += value2;
				this.SelectionArea.IsSelectionDirty = true;
				this.SelectionArea.Update();
			}
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x00152198 File Offset: 0x00150398
		public bool TryEnterRotationMode(bool cutOriginal = true)
		{
			bool flag = this.Mode == PlaySelectionTool.EditMode.None;
			if (flag)
			{
				this.EnterTransformMode(PlaySelectionTool.EditMode.TransformRotation, cutOriginal, null);
			}
			else
			{
				bool flag2 = this.Mode == PlaySelectionTool.EditMode.TransformTranslationGizmos || this.Mode == PlaySelectionTool.EditMode.TransformTranslationBrushPoint;
				if (!flag2)
				{
					return false;
				}
				this.SwapTransformMode(PlaySelectionTool.EditMode.TransformRotation);
			}
			return true;
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x001521F0 File Offset: 0x001503F0
		public bool TryEnterTranslationGizmoMode(bool cutOriginal = true)
		{
			bool flag = this.Mode == PlaySelectionTool.EditMode.None;
			if (flag)
			{
				this.EnterTransformMode(PlaySelectionTool.EditMode.TransformTranslationGizmos, cutOriginal, null);
			}
			else
			{
				bool flag2 = this.Mode == PlaySelectionTool.EditMode.TransformRotation || this.Mode == PlaySelectionTool.EditMode.TransformTranslationBrushPoint;
				if (!flag2)
				{
					return false;
				}
				this.SwapTransformMode(PlaySelectionTool.EditMode.TransformTranslationGizmos);
			}
			return true;
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x00152248 File Offset: 0x00150448
		public bool TryEnterTranslationBrushPointMode(bool cutOriginal = true)
		{
			bool flag = this.Mode == PlaySelectionTool.EditMode.None;
			if (flag)
			{
				this.EnterTransformMode(PlaySelectionTool.EditMode.TransformTranslationBrushPoint, cutOriginal, null);
			}
			else
			{
				bool flag2 = this.Mode == PlaySelectionTool.EditMode.TransformRotation || this.Mode == PlaySelectionTool.EditMode.TransformTranslationGizmos;
				if (!flag2)
				{
					return false;
				}
				this.SwapTransformMode(PlaySelectionTool.EditMode.TransformTranslationBrushPoint);
			}
			return true;
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001522A0 File Offset: 0x001504A0
		public bool TryEnterTranslationModeWithClipboard(EditorBlocksChange.BlockChange[] clipboard)
		{
			bool flag = this.Mode > PlaySelectionTool.EditMode.None;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = clipboard == null || clipboard.Length == 0;
				if (flag2)
				{
					this._gameInstance.Chat.Log("You do not currently have anything copied in your clipboard.");
					result = false;
				}
				else
				{
					this.EnterTransformMode(PlaySelectionTool.EditMode.TransformTranslationGizmos, true, clipboard);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001522FC File Offset: 0x001504FC
		public void OnScrollWheelEvent(int directionOfScroll)
		{
			bool flag = !this.IsInTransformationMode();
			if (!flag)
			{
				this.OnRotationChange(new Vector3(0f, (float)directionOfScroll * 1.5707964f, 0f));
				this.FinishRotationGizmoInteraction();
			}
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x00152340 File Offset: 0x00150540
		private void OnKeyDown()
		{
			Input input = this._gameInstance.Input;
			bool flag = this.Mode == PlaySelectionTool.EditMode.None && input.IsShiftHeld() && input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_O, false);
			if (flag)
			{
				this._gameInstance.Connection.SendPacket(new BuilderToolSelectionToolAskForClipboard());
			}
			else
			{
				bool flag2 = input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_I, false);
				if (flag2)
				{
					this.TryEnterTranslationBrushPointMode(!input.IsAltHeld());
				}
				else
				{
					bool flag3 = input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_O, false);
					if (flag3)
					{
						this.TryEnterTranslationGizmoMode(!input.IsAltHeld());
					}
					else
					{
						bool flag4 = input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_R, false);
						if (flag4)
						{
							this.TryEnterRotationMode(!input.IsAltHeld());
						}
						else
						{
							bool flag5 = input.ConsumeKey(SDL.SDL_Scancode.SDL_SCANCODE_K, false);
							if (flag5)
							{
								bool flag6 = input.IsAltHeld();
								if (flag6)
								{
									this.ConfirmTransformationModePlacement(false, false);
								}
								else
								{
									this.ExitTransformMode(input.IsShiftHeld());
								}
							}
						}
					}
				}
			}
			bool flag7 = input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_E, false);
			if (flag7)
			{
				bool flag8 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionNextSet], false);
				if (flag8)
				{
					this.SelectionArea.CycleSelectionIndex(true);
				}
				bool flag9 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionPreviousSet], false);
				if (flag9)
				{
					this.SelectionArea.CycleSelectionIndex(false);
				}
			}
			bool flag10 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionShiftUp], false);
			if (flag10)
			{
				this.SelectionArea.Shift(Vector3.Up);
			}
			bool flag11 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionShiftDown], false);
			if (flag11)
			{
				this.SelectionArea.Shift(Vector3.Down);
			}
			bool flag12 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionNextDrawMode], false);
			if (flag12)
			{
				this.NextDrawMode();
			}
			bool flag13 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionClear], false);
			if (flag13)
			{
				this.CancelAllActions(null);
				this.SelectionArea.ClearSelection();
			}
			bool flag14 = input.IsShiftHeld() && input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionCopy], false);
			if (flag14)
			{
				this.OnGeneralAction(2);
			}
			bool flag15 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionPosOne], false);
			if (flag15)
			{
				this.SelectionArea.Position1 = Vector3.Floor(this._gameInstance.LocalPlayer.Position);
				bool flag16 = this.SelectionArea.Position2.IsNaN();
				if (flag16)
				{
					this.SelectionArea.Position2 = Vector3.Floor(this._gameInstance.LocalPlayer.Position);
				}
				this.SelectionArea.IsSelectionDirty = true;
				this.SelectionArea.Update();
				this.SelectionArea.OnSelectionChange();
			}
			bool flag17 = input.ConsumeKey(this._keybinds[PlaySelectionTool.Keybind.SelectionPosTwo], false);
			if (flag17)
			{
				this.SelectionArea.Position2 = Vector3.Floor(this._gameInstance.LocalPlayer.Position);
				bool flag18 = this.SelectionArea.Position1.IsNaN();
				if (flag18)
				{
					this.SelectionArea.Position1 = Vector3.Floor(this._gameInstance.LocalPlayer.Position);
				}
				this.SelectionArea.IsSelectionDirty = true;
				this.SelectionArea.Update();
				this.SelectionArea.OnSelectionChange();
			}
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x00152680 File Offset: 0x00150880
		public void CreateBlockEntityPreview(EditorBlocksChange.BlockChange[] clipboard = null)
		{
			int networkEffectIndex;
			this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue("PrototypePastePreview", out networkEffectIndex);
			this._previewEntities.Clear();
			this._entityOffsetFromRotationOrigin.Clear();
			this.blockIdsAtLocationOfGizmoEdit.Clear();
			Vector3 value = new Vector3(0.5f, 0f, 0.5f);
			Vector3[] positionOffsets;
			int[] adjacentLookup;
			NativeArray<int> blockIds = (clipboard != null) ? PasteTool.GenerateChunkArray(clipboard, out positionOffsets, out adjacentLookup) : PasteTool.GenerateChunkArray(this.SelectionArea, this._gameInstance, out positionOffsets, out adjacentLookup);
			List<Vector3> list = new List<Vector3>(16);
			List<int> list2 = PasteTool.FilterVisibleBlocks(blockIds, positionOffsets, adjacentLookup, this._gameInstance, list);
			blockIds.Dispose();
			bool flag = list2.Count > 32768;
			if (!flag)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					int block = list2[i];
					Vector3 value2 = list[i];
					Vector3 vector = value2 + value;
					bool flag2 = clipboard != null;
					if (flag2)
					{
						vector += this.initialPasteLocationForPasteMode.ToVector3();
					}
					Entity entity;
					this._gameInstance.EntityStoreModule.Spawn(-1, out entity);
					entity.SetIsTangible(false);
					entity.SetBlock(block);
					entity.AddEffect(networkEffectIndex, null, null, null);
					entity.SetPositionTeleport(vector);
					this._previewEntities.Add(entity);
					this._entityOffsetFromRotationOrigin.Add(vector + new Vector3(0f, 0.5f, 0f) - this.rotationOrigin);
				}
				bool flag3 = clipboard == null && this.shouldCutOriginal;
				if (flag3)
				{
					foreach (Vector3 vector2 in this.SelectionArea)
					{
						int block2 = this._gameInstance.MapModule.GetBlock(vector2, int.MaxValue);
						this._gameInstance.MapModule.SetClientBlock((int)vector2.X, (int)vector2.Y, (int)vector2.Z, 0);
						this.blockIdsAtLocationOfGizmoEdit[this.ToIntVector(vector2)] = block2;
					}
				}
			}
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x001528F8 File Offset: 0x00150AF8
		private Vector3 GetRotationGizmoOffsetFromDimensions(Vector3 dimensions)
		{
			int num = 0;
			bool flag = dimensions.X % 2f == 0f;
			if (flag)
			{
				num |= 4;
			}
			bool flag2 = dimensions.Y % 2f == 0f;
			if (flag2)
			{
				num |= 2;
			}
			bool flag3 = dimensions.Z % 2f == 0f;
			if (flag3)
			{
				num |= 1;
			}
			Vector3 result;
			switch (num)
			{
			case 0:
				result = new Vector3(0f, 0f, 0f);
				break;
			case 1:
				result = new Vector3(0f, 0f, 0.5f);
				break;
			case 2:
				result = new Vector3(0f, 0.5f, 0f);
				break;
			case 3:
				result = new Vector3(0.5f, 0f, 0f);
				break;
			case 4:
				result = new Vector3(0.5f, 0f, 0f);
				break;
			case 5:
				result = new Vector3(0f, 0.5f, 0f);
				break;
			case 6:
				result = new Vector3(0f, 0f, 0.5f);
				break;
			case 7:
				result = new Vector3(0f, 0f, 0f);
				break;
			default:
				result = new Vector3(0f, 0f, 0f);
				break;
			}
			return result;
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x00152A74 File Offset: 0x00150C74
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			bool flag = this.SelectionArea.IsSelectionDefined();
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				SceneRenderer.SceneData data = this._gameInstance.SceneRenderer.Data;
				bool visible = this._rotationGizmo.Visible;
				if (visible)
				{
					this._rotationGizmo.Draw(ref viewProjectionMatrix, this._gameInstance.CameraModule.Controller, -data.CameraPosition);
				}
				bool visible2 = this._translationGizmo.Visible;
				if (visible2)
				{
					this._translationGizmo.Draw(ref viewProjectionMatrix, -data.CameraPosition);
				}
				Vector3 value = this._gameInstance.SceneRenderer.Data.CameraDirection * 0.1f;
				Vector3 vector = -value;
				Matrix matrix;
				Matrix.CreateTranslation(ref vector, out matrix);
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ViewRotationMatrix, out matrix);
				Matrix matrix2;
				Matrix.Multiply(ref matrix, ref this._gameInstance.SceneRenderer.Data.ProjectionMatrix, out matrix2);
				this._graphics.SaveColorMask();
				gl.DepthMask(true);
				gl.ColorMask(false, false, false, false);
				gl.DepthFunc(GL.ALWAYS);
				this.SelectionArea.Renderer.DrawOutlineBox(ref viewProjectionMatrix, ref data.ViewRotationMatrix, -data.CameraPosition, data.ViewportSize, this._graphics.BlackColor, this._graphics.BlackColor, 0f, 1f, true);
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
				this.SelectionArea.Renderer.DrawOutlineBox(ref viewProjectionMatrix, ref data.ViewRotationMatrix, -data.CameraPosition, data.ViewportSize, this._graphics.BlackColor, this._graphics.BlackColor, 0f, 1f, true);
				gl.DepthMask(false);
				this._graphics.RestoreColorMask();
				float num = (float)this._builderTools.builderToolsSettings.SelectionOpacity * 0.01f;
				this.SelectionArea.Renderer.DrawGrid(ref matrix2, -data.CameraPosition, this.Color, num, this._selectionDrawMode);
				gl.DepthFunc(GL.ALWAYS);
				this.SelectionArea.Renderer.DrawOutlineBox(ref matrix2, ref data.ViewRotationMatrix, -data.CameraPosition, data.ViewportSize, this._graphics.WhiteColor, this._graphics.BlackColor, num, num * 0.25f, this._builderTools.DrawHighlightAndUndergroundColor);
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
				bool flag2 = this.Mode == PlaySelectionTool.EditMode.ResizePos1 || this.Mode == PlaySelectionTool.EditMode.MovePos1;
				if (flag2)
				{
					this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.4f, 0.05f);
				}
				else
				{
					bool flag3 = this.Mode == PlaySelectionTool.EditMode.ResizePos2 || this.Mode == PlaySelectionTool.EditMode.MovePos2 || this.Mode == PlaySelectionTool.EditMode.CreateSelectionSet2DPlane || this.Mode == PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension;
					if (flag3)
					{
						this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.05f, 0.4f);
					}
					else
					{
						bool flag4 = this.HoverMode == PlaySelectionTool.EditMode.ResizePos1 || this.HoverMode == PlaySelectionTool.EditMode.MovePos1;
						if (flag4)
						{
							this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.2f, 0.05f);
						}
						else
						{
							bool flag5 = this.HoverMode == PlaySelectionTool.EditMode.ResizePos2 || this.HoverMode == PlaySelectionTool.EditMode.MovePos2;
							if (flag5)
							{
								this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.05f, 0.2f);
							}
							else
							{
								this.SelectionArea.Renderer.DrawCornerBoxes(ref viewProjectionMatrix, -data.CameraPosition, this._graphics.GreenColor, this._graphics.RedColor, 0.05f, 0.05f);
							}
						}
					}
				}
				bool flag6 = this.renderSideGizmos;
				if (flag6)
				{
					Vector3 vector2 = this.gizmoNormal;
					bool flag7 = this.Mode == PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension || this.Mode == PlaySelectionTool.EditMode.TranslateSide || this.Mode == PlaySelectionTool.EditMode.ResizeSide;
					if (flag7)
					{
						Vector3 other = new Vector3(-1f, -1f, -1f);
						Vector3 vector3 = (this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne) ? this.SelectionArea.Position1 : this.SelectionArea.Position2;
						Vector3 vector4 = (this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne) ? this.SelectionArea.Position2 : this.SelectionArea.Position1;
						bool flag8 = this.gizmoNormal == Vector3.Right;
						if (flag8)
						{
							vector2 = ((vector3.X > vector4.X) ? vector2 : vector2.Sign(other));
						}
						else
						{
							bool flag9 = this.gizmoNormal == Vector3.Left;
							if (flag9)
							{
								vector2 = ((vector3.X < vector4.X) ? vector2 : vector2.Sign(other));
							}
							else
							{
								bool flag10 = this.gizmoNormal == Vector3.Up;
								if (flag10)
								{
									vector2 = ((vector3.Y > vector4.Y) ? vector2 : vector2.Sign(other));
								}
								else
								{
									bool flag11 = this.gizmoNormal == Vector3.Down;
									if (flag11)
									{
										vector2 = ((vector3.Y < vector4.Y) ? vector2 : vector2.Sign(other));
									}
									else
									{
										bool flag12 = this.gizmoNormal == Vector3.Backward;
										if (flag12)
										{
											vector2 = ((vector3.Z > vector4.Z) ? vector2 : vector2.Sign(other));
										}
										else
										{
											bool flag13 = this.gizmoNormal == Vector3.Forward;
											if (flag13)
											{
												vector2 = ((vector3.Z < vector4.Z) ? vector2 : vector2.Sign(other));
											}
										}
									}
								}
							}
						}
					}
					foreach (Vector3 selectionNormal in this._blockFaceNormals)
					{
						bool flag14 = this._gameInstance.Input.IsShiftHeld();
						Vector3 color;
						if (flag14)
						{
							color = (selectionNormal.Equals(vector2) ? this._graphics.MagentaColor : this._graphics.RedColor);
						}
						else
						{
							bool flag15 = this._gameInstance.Input.IsAltHeld();
							if (flag15)
							{
								color = (selectionNormal.Equals(vector2) ? this._graphics.YellowColor : this._graphics.WhiteColor);
							}
							else
							{
								color = (selectionNormal.Equals(vector2) ? this._graphics.CyanColor : this._graphics.BlueColor);
							}
						}
						Settings settings = this._gameInstance.App.Settings;
						this.SelectionArea.Renderer.DrawResizeGizmoForFace(this._gameInstance.SceneRenderer.Data.CameraPosition, ref viewProjectionMatrix, selectionNormal, color, (float)settings.MinPlaySelectGizmoSize, (float)settings.MaxPlaySelectGizmoSize, settings.PercentageOfPlaySelectionLengthGizmoShouldRender);
					}
				}
			}
			bool flag16 = this.FaceHighlightNeedsDrawing();
			if (flag16)
			{
				Vector3 vector5 = (this.Mode == PlaySelectionTool.EditMode.MoveBox || this.Mode == PlaySelectionTool.EditMode.ResizeSide || this.Mode == PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension || this.Mode == PlaySelectionTool.EditMode.TranslateSide) ? this._resizeNormal : this._rayBoxHit.Normal;
				Vector3 other2 = new Vector3(-1f, -1f, -1f);
				Vector3 vector6 = (this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne) ? this.SelectionArea.Position1 : this.SelectionArea.Position2;
				Vector3 vector7 = (this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne) ? this.SelectionArea.Position2 : this.SelectionArea.Position1;
				bool flag17 = this.gizmoNormal == Vector3.Right;
				if (flag17)
				{
					vector5 = ((vector6.X > vector7.X) ? vector5 : vector5.Sign(other2));
				}
				else
				{
					bool flag18 = this.gizmoNormal == Vector3.Left;
					if (flag18)
					{
						vector5 = ((vector6.X < vector7.X) ? vector5 : vector5.Sign(other2));
					}
					else
					{
						bool flag19 = this.gizmoNormal == Vector3.Up;
						if (flag19)
						{
							vector5 = ((vector6.Y > vector7.Y) ? vector5 : vector5.Sign(other2));
						}
						else
						{
							bool flag20 = this.gizmoNormal == Vector3.Down;
							if (flag20)
							{
								vector5 = ((vector6.Y < vector7.Y) ? vector5 : vector5.Sign(other2));
							}
							else
							{
								bool flag21 = this.gizmoNormal == Vector3.Backward;
								if (flag21)
								{
									vector5 = ((vector6.Z > vector7.Z) ? vector5 : vector5.Sign(other2));
								}
								else
								{
									bool flag22 = this.gizmoNormal == Vector3.Forward;
									if (flag22)
									{
										vector5 = ((vector6.Z < vector7.Z) ? vector5 : vector5.Sign(other2));
									}
								}
							}
						}
					}
				}
				Vector3 color2 = (this.Mode == PlaySelectionTool.EditMode.MoveBox) ? this._graphics.MagentaColor : ((this.Mode == PlaySelectionTool.EditMode.ResizeSide || this.Mode == PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension || this.Mode == PlaySelectionTool.EditMode.TranslateSide) ? this._graphics.CyanColor : this._graphics.BlueColor);
				this.SelectionArea.Renderer.DrawFaceHighlight(ref viewProjectionMatrix, vector5, color2, -this._gameInstance.SceneRenderer.Data.CameraPosition);
			}
			bool flag23 = this.SelectionArea.IsAnySelectionDefined();
			if (flag23)
			{
				GLFunctions gl2 = this._graphics.GL;
				gl2.DepthFunc(GL.ALWAYS);
				for (int j = 0; j < 8; j++)
				{
					bool flag24 = this.SelectionArea.SelectionData[j] != null && j != this.SelectionArea.SelectionIndex;
					if (flag24)
					{
						Vector3 vector8 = this.SelectionArea.SelectionColors[j];
						this.SelectionArea.BoxRenderer.Draw(Vector3.Zero, this.SelectionArea.SelectionData[j].Item3, viewProjectionMatrix, vector8, 0.4f, vector8, 0.03f);
					}
				}
				gl2.DepthFunc((!this._graphics.UseReverseZ) ? GL.GEQUAL : GL.LEQUAL);
			}
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x0015358C File Offset: 0x0015178C
		public override void DrawText(ref Matrix viewProjectionMatrix)
		{
			base.DrawText(ref viewProjectionMatrix);
			bool flag = this.Mode == PlaySelectionTool.EditMode.TransformRotation && this._rotationGizmo.Visible;
			if (flag)
			{
				this._rotationGizmo.DrawText();
			}
			this.SelectionArea.Renderer.DrawText(ref viewProjectionMatrix, this._gameInstance.CameraModule.Controller);
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x001535EC File Offset: 0x001517EC
		private void OnResize()
		{
			bool flag = !this.SelectionArea.IsSelectionDefined();
			if (!flag)
			{
				Vector3 projectedCursorPosition = this.GetProjectedCursorPosition();
				Vector3 size = this.SelectionArea.GetSize();
				float num = size.X * size.Y * size.Z;
				bool flag2 = this._resizeDirection == PlaySelectionTool.Direction.Up || this._resizeDirection == PlaySelectionTool.Direction.Down;
				if (flag2)
				{
					float num2 = Math.Abs(projectedCursorPosition.Y - this.SelectionArea.CenterPos.Y);
					bool flag3 = num2 > 150f;
					if (flag3)
					{
						projectedCursorPosition.Y = this.SelectionArea.CenterPos.Y + this._resizeNormal.Y * 150f;
					}
					float y = MathHelper.Min(MathHelper.Max((float)PlaySelectionTool.FloorInt(projectedCursorPosition.Y), 0f), (float)(ChunkHelper.Height - 1));
					bool flag4 = this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne;
					if (flag4)
					{
						this.SelectionArea.Position1.Y = y;
					}
					else
					{
						this.SelectionArea.Position2.Y = y;
					}
				}
				else
				{
					bool flag5 = this._resizeDirection == PlaySelectionTool.Direction.Left || this._resizeDirection == PlaySelectionTool.Direction.Right;
					if (flag5)
					{
						float num3 = Math.Abs(projectedCursorPosition.X - this.SelectionArea.CenterPos.X);
						bool flag6 = num3 > 150f;
						if (flag6)
						{
							projectedCursorPosition.X = this.SelectionArea.CenterPos.X + this._resizeNormal.X * 150f;
						}
						bool flag7 = this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne;
						if (flag7)
						{
							this.SelectionArea.Position1.X = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.X);
						}
						else
						{
							this.SelectionArea.Position2.X = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.X);
						}
					}
					else
					{
						bool flag8 = this._resizeDirection == PlaySelectionTool.Direction.Forward || this._resizeDirection == PlaySelectionTool.Direction.Backward;
						if (flag8)
						{
							float num4 = Math.Abs(projectedCursorPosition.Z - this.SelectionArea.CenterPos.Z);
							bool flag9 = num4 > 150f;
							if (flag9)
							{
								projectedCursorPosition.Z = this.SelectionArea.CenterPos.Z + this._resizeNormal.Z * 150f;
							}
							bool flag10 = this.resizePoint == PlaySelectionTool.SelectionPoint.PosOne;
							if (flag10)
							{
								this.SelectionArea.Position1.Z = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Z);
							}
							else
							{
								this.SelectionArea.Position2.Z = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Z);
							}
						}
					}
				}
				Vector3 size2 = this.SelectionArea.GetSize();
				float num5 = size2.X * size2.Y * size2.Z;
				bool flag11 = num5 > num;
				if (flag11)
				{
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_SCALE");
				}
				else
				{
					bool flag12 = num5 < num;
					if (flag12)
					{
						this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_SCALE");
					}
				}
				this.SelectionArea.IsSelectionDirty = true;
			}
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0015390C File Offset: 0x00151B0C
		private void OnMove()
		{
			bool flag = !this.SelectionArea.IsSelectionDefined();
			if (!flag)
			{
				Vector3 projectedCursorPosition = this.GetProjectedCursorPosition();
				Vector3 size = this.SelectionArea.GetSize();
				Vector3 position = this.SelectionArea.Position1;
				bool flag2 = this._resizeDirection == PlaySelectionTool.Direction.Up;
				if (flag2)
				{
					bool flag3 = this.SelectionArea.Position1.Y > this.SelectionArea.Position2.Y;
					if (flag3)
					{
						this.SelectionArea.Position1.Y = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Y);
						this.SelectionArea.Position2.Y = this.SelectionArea.Position1.Y - size.Y + 1f;
					}
					else
					{
						this.SelectionArea.Position2.Y = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Y);
						this.SelectionArea.Position1.Y = this.SelectionArea.Position2.Y - size.Y + 1f;
					}
				}
				else
				{
					bool flag4 = this._resizeDirection == PlaySelectionTool.Direction.Down;
					if (flag4)
					{
						bool flag5 = this.SelectionArea.Position1.Y < this.SelectionArea.Position2.Y;
						if (flag5)
						{
							this.SelectionArea.Position1.Y = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Y);
							this.SelectionArea.Position2.Y = this.SelectionArea.Position1.Y + size.Y - 1f;
						}
						else
						{
							this.SelectionArea.Position2.Y = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Y);
							this.SelectionArea.Position1.Y = this.SelectionArea.Position2.Y - size.Y + 1f;
						}
					}
					else
					{
						bool flag6 = this._resizeDirection == PlaySelectionTool.Direction.Left;
						if (flag6)
						{
							bool flag7 = this.SelectionArea.Position1.X < this.SelectionArea.Position2.X;
							if (flag7)
							{
								this.SelectionArea.Position1.X = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.X);
								this.SelectionArea.Position2.X = this.SelectionArea.Position1.X + size.X - 1f;
							}
							else
							{
								this.SelectionArea.Position2.X = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.X);
								this.SelectionArea.Position1.X = this.SelectionArea.Position2.X - size.X + 1f;
							}
						}
						else
						{
							bool flag8 = this._resizeDirection == PlaySelectionTool.Direction.Right;
							if (flag8)
							{
								bool flag9 = this.SelectionArea.Position1.X > this.SelectionArea.Position2.X;
								if (flag9)
								{
									this.SelectionArea.Position1.X = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.X);
									this.SelectionArea.Position2.X = this.SelectionArea.Position1.X - size.X + 1f;
								}
								else
								{
									this.SelectionArea.Position2.X = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.X);
									this.SelectionArea.Position1.X = this.SelectionArea.Position2.X - size.X + 1f;
								}
							}
							else
							{
								bool flag10 = this._resizeDirection == PlaySelectionTool.Direction.Forward;
								if (flag10)
								{
									bool flag11 = this.SelectionArea.Position1.Z > this.SelectionArea.Position2.Z;
									if (flag11)
									{
										this.SelectionArea.Position2.Z = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Z);
										this.SelectionArea.Position1.Z = this.SelectionArea.Position2.Z + size.Z - 1f;
									}
									else
									{
										this.SelectionArea.Position1.Z = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Z);
										this.SelectionArea.Position2.Z = this.SelectionArea.Position1.Z - size.Z + 1f;
									}
								}
								else
								{
									bool flag12 = this._resizeDirection == PlaySelectionTool.Direction.Backward;
									if (flag12)
									{
										bool flag13 = this.SelectionArea.Position1.Z < this.SelectionArea.Position2.Z;
										if (flag13)
										{
											this.SelectionArea.Position2.Z = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Z);
											this.SelectionArea.Position1.Z = this.SelectionArea.Position2.Z - size.Z + 1f;
										}
										else
										{
											this.SelectionArea.Position1.Z = (float)PlaySelectionTool.FloorInt(projectedCursorPosition.Z);
											this.SelectionArea.Position2.Z = this.SelectionArea.Position1.Z - size.Z + 1f;
										}
									}
								}
							}
						}
					}
				}
				bool flag14 = position != this.SelectionArea.Position1;
				if (flag14)
				{
					this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_SELECTION_DRAG");
				}
				this.SelectionArea.IsSelectionDirty = true;
			}
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x00153E9C File Offset: 0x0015209C
		private void UpdateGizmoSelection()
		{
			bool flag = this.Mode > PlaySelectionTool.EditMode.None;
			if (!flag)
			{
				Vector3 position = this._gameInstance.CameraModule.Controller.Position;
				Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
				Quaternion rotation2 = Quaternion.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f);
				Vector3 lineDirection = Vector3.Transform(Vector3.Forward, rotation2);
				lineDirection.Normalize();
				Settings settings = this._gameInstance.App.Settings;
				float num = MathHelper.Clamp(this.SelectionArea.SelectionSize.X * settings.PercentageOfPlaySelectionLengthGizmoShouldRender, (float)settings.MinPlaySelectGizmoSize, (float)settings.MaxPlaySelectGizmoSize) / 2f;
				float num2 = MathHelper.Clamp(this.SelectionArea.SelectionSize.Y * settings.PercentageOfPlaySelectionLengthGizmoShouldRender, (float)settings.MinPlaySelectGizmoSize, (float)settings.MaxPlaySelectGizmoSize) / 2f;
				float num3 = MathHelper.Clamp(this.SelectionArea.SelectionSize.Z * settings.PercentageOfPlaySelectionLengthGizmoShouldRender, (float)settings.MinPlaySelectGizmoSize, (float)settings.MaxPlaySelectGizmoSize) / 2f;
				float num4 = float.MaxValue;
				bool flag2 = false;
				foreach (Vector3 vector in this._blockFaceNormals)
				{
					Vector3 vector2 = new Vector3(this.SelectionArea.CenterPos.X + this.SelectionArea.SelectionSize.X / 2f * vector.X, this.SelectionArea.CenterPos.Y + this.SelectionArea.SelectionSize.Y / 2f * vector.Y, this.SelectionArea.CenterPos.Z + this.SelectionArea.SelectionSize.Z / 2f * vector.Z);
					Vector3 vector3;
					bool flag3 = HitDetection.CheckRayPlaneIntersection(vector2, vector, position, lineDirection, out vector3, true);
					bool flag4 = !flag3;
					if (!flag4)
					{
						Vector3 vector4 = Vector3.Subtract(vector2, vector3).Abs();
						bool flag5 = vector.X == 0f && vector4.X > num;
						if (!flag5)
						{
							bool flag6 = vector.Y == 0f && vector4.Y > num2;
							if (!flag6)
							{
								bool flag7 = vector.Z == 0f && vector4.Z > num3;
								if (!flag7)
								{
									float num5 = Vector3.DistanceSquared(vector3, position);
									bool flag8 = num5 >= num4;
									if (!flag8)
									{
										num4 = num5;
										this.gizmoPosition = vector3;
										this.gizmoNormal = vector;
										flag2 = true;
									}
								}
							}
						}
					}
				}
				bool flag9 = !flag2;
				if (flag9)
				{
					this.gizmoNormal = Vector3.NaN;
				}
			}
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x0015417C File Offset: 0x0015237C
		private bool UpdateSelectionHighlight(bool reverse = false)
		{
			Vector3 position = this._gameInstance.CameraModule.Controller.Position;
			Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
			Quaternion rotation2 = Quaternion.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f);
			Vector3 vector = Vector3.Transform(Vector3.Forward, rotation2);
			if (reverse)
			{
				vector = -vector;
			}
			bool flag = this._gameInstance.Input.IsShiftHeld();
			if (flag)
			{
				BoundingBox box = new BoundingBox(this.SelectionArea.Position1, this.SelectionArea.Position1 + Vector3.One);
				bool flag2 = HitDetection.CheckRayBoxCollision(box, position, vector, out this._rayBoxHit, true);
				if (flag2)
				{
					this.HoverMode = PlaySelectionTool.EditMode.ResizePos1;
				}
				else
				{
					box = new BoundingBox(this.SelectionArea.Position2, this.SelectionArea.Position2 + Vector3.One);
					bool flag3 = HitDetection.CheckRayBoxCollision(box, position, vector, out this._rayBoxHit, true);
					if (flag3)
					{
						this.HoverMode = PlaySelectionTool.EditMode.ResizePos2;
					}
					else
					{
						this.HoverMode = PlaySelectionTool.EditMode.None;
					}
				}
			}
			else
			{
				this.HoverMode = PlaySelectionTool.EditMode.None;
			}
			bool flag4 = this.SelectionArea.IsSelectionDefined();
			if (flag4)
			{
				bool flag5 = HitDetection.CheckRayBoxCollision(this.SelectionArea.GetBoundsExclusiveMax(), position, vector, out this._rayBoxHit, true);
				if (flag5)
				{
					return this.IsCursorOverSelection = true;
				}
			}
			return this.IsCursorOverSelection = false;
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x00154300 File Offset: 0x00152500
		public bool FaceHighlightNeedsDrawing()
		{
			bool flag = this.Mode == PlaySelectionTool.EditMode.MoveBox || this.Mode == PlaySelectionTool.EditMode.ResizeSide || this.Mode == PlaySelectionTool.EditMode.TranslateSide || this.Mode == PlaySelectionTool.EditMode.CreateSelectionSetThirdDimension;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2;
				if (this.Mode == PlaySelectionTool.EditMode.None && this.HoverMode == PlaySelectionTool.EditMode.None)
				{
					ToolInstance activeTool = this._builderTools.ActiveTool;
					string a;
					if (activeTool == null)
					{
						a = null;
					}
					else
					{
						BuilderTool builderTool = activeTool.BuilderTool;
						a = ((builderTool != null) ? builderTool.Id : null);
					}
					flag2 = (a != this.ToolId);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				result = (flag3 && false);
			}
			return result;
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x00154390 File Offset: 0x00152590
		private Vector3 GetProjectedCursorPosition()
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			Vector3 vector = this._resizeOrigin - lookRay.Position;
			bool flag = this._resizeDirection == PlaySelectionTool.Direction.Up || this._resizeDirection == PlaySelectionTool.Direction.Down;
			Vector2 ray1Position;
			Vector2 ray1Direction;
			Vector2 ray2Position;
			Vector2 ray2Direction;
			if (flag)
			{
				float num = vector.X;
				vector.X = vector.Z;
				vector.Z = -num;
				ray1Position = new Vector2(lookRay.Position.X, lookRay.Position.Z);
				ray1Direction = new Vector2(lookRay.Direction.X, lookRay.Direction.Z);
				ray2Position = new Vector2(this._resizeOrigin.X, this._resizeOrigin.Z);
				ray2Direction = new Vector2(vector.X, vector.Z);
			}
			else
			{
				bool flag2 = this._resizeDirection == PlaySelectionTool.Direction.Left || this._resizeDirection == PlaySelectionTool.Direction.Right;
				if (flag2)
				{
					float num = vector.Y;
					vector.Y = vector.Z;
					vector.Z = -num;
					ray1Position = new Vector2(lookRay.Position.Y, lookRay.Position.Z);
					ray1Direction = new Vector2(lookRay.Direction.Y, lookRay.Direction.Z);
					ray2Position = new Vector2(this._resizeOrigin.Y, this._resizeOrigin.Z);
					ray2Direction = new Vector2(vector.Y, vector.Z);
				}
				else
				{
					float num = vector.X;
					vector.X = vector.Y;
					vector.Y = -num;
					ray1Position = new Vector2(lookRay.Position.X, lookRay.Position.Y);
					ray1Direction = new Vector2(lookRay.Direction.X, lookRay.Direction.Y);
					ray2Position = new Vector2(this._resizeOrigin.X, this._resizeOrigin.Y);
					ray2Direction = new Vector2(vector.X, vector.Y);
				}
			}
			Vector2 vector2;
			bool flag3 = HitDetection.Get2DRayIntersection(ray1Position, ray1Direction, ray2Position, ray2Direction, out vector2);
			Vector3 result;
			if (flag3)
			{
				bool flag4 = this._resizeDirection == PlaySelectionTool.Direction.Up || this._resizeDirection == PlaySelectionTool.Direction.Down;
				if (flag4)
				{
					float num2 = (vector2.X - lookRay.Position.X) / lookRay.Direction.X;
					result = new Vector3(vector2.X, num2 * lookRay.Direction.Y + lookRay.Position.Y, vector2.Y);
				}
				else
				{
					bool flag5 = this._resizeDirection == PlaySelectionTool.Direction.Left || this._resizeDirection == PlaySelectionTool.Direction.Right;
					if (flag5)
					{
						float num2 = (vector2.Y - lookRay.Position.Z) / lookRay.Direction.Z;
						result = new Vector3(num2 * lookRay.Direction.X + lookRay.Position.X, vector2.X, vector2.Y);
					}
					else
					{
						float num2 = (vector2.X - lookRay.Position.X) / lookRay.Direction.X;
						result = new Vector3(vector2.X, vector2.Y, num2 * lookRay.Direction.Z + lookRay.Position.Z);
					}
				}
			}
			else
			{
				result = lookRay.Position;
			}
			return result;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0015470C File Offset: 0x0015290C
		public void NextDrawMode()
		{
			bool flag = this._selectionDrawMode == SelectionToolRenderer.SelectionDrawMode.Subtract;
			if (flag)
			{
				this._selectionDrawMode = SelectionToolRenderer.SelectionDrawMode.Normal;
			}
			else
			{
				this._selectionDrawMode++;
			}
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x0015473E File Offset: 0x0015293E
		private void OnGeneralAction(BuilderToolAction action)
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolGeneralAction(action));
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x00154758 File Offset: 0x00152958
		private static int FloorInt(float v)
		{
			return (int)Math.Floor((double)v);
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x00154764 File Offset: 0x00152964
		private static PlaySelectionTool.Direction GetVectorDirection(Vector3 vector)
		{
			bool flag = vector == Vector3.Up;
			PlaySelectionTool.Direction result;
			if (flag)
			{
				result = PlaySelectionTool.Direction.Up;
			}
			else
			{
				bool flag2 = vector == Vector3.Down;
				if (flag2)
				{
					result = PlaySelectionTool.Direction.Down;
				}
				else
				{
					bool flag3 = vector == Vector3.Left;
					if (flag3)
					{
						result = PlaySelectionTool.Direction.Left;
					}
					else
					{
						bool flag4 = vector == Vector3.Right;
						if (flag4)
						{
							result = PlaySelectionTool.Direction.Right;
						}
						else
						{
							bool flag5 = vector == Vector3.Forward;
							if (flag5)
							{
								result = PlaySelectionTool.Direction.Forward;
							}
							else
							{
								bool flag6 = vector == Vector3.Backward;
								if (flag6)
								{
									result = PlaySelectionTool.Direction.Backward;
								}
								else
								{
									result = PlaySelectionTool.Direction.None;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040028D4 RID: 10452
		public Vector3 Color = Vector3.One;

		// Token: 0x040028D5 RID: 10453
		public bool lockX;

		// Token: 0x040028D6 RID: 10454
		public bool lockY;

		// Token: 0x040028D7 RID: 10455
		public bool lockZ;

		// Token: 0x040028D8 RID: 10456
		public Vector3 initialBlockNormal;

		// Token: 0x040028D9 RID: 10457
		public bool flipXZSelectionExpansionAxis = true;

		// Token: 0x040028DA RID: 10458
		public Vector3 gizmoNormal = Vector3.NaN;

		// Token: 0x040028DB RID: 10459
		public Vector3 gizmoPosition = Vector3.NaN;

		// Token: 0x040028DC RID: 10460
		public bool wasLastInteractionFirstRun;

		// Token: 0x040028DD RID: 10461
		public PlaySelectionTool.SelectionPoint resizePoint = PlaySelectionTool.SelectionPoint.PosOne;

		// Token: 0x040028E1 RID: 10465
		private const int MaxResizeDistance = 150;

		// Token: 0x040028E2 RID: 10466
		private const int BlockPreviewSizeLimit = 32768;

		// Token: 0x040028E3 RID: 10467
		private const int MaxExtrudeDistance = 25;

		// Token: 0x040028E4 RID: 10468
		private readonly RotationGizmo _rotationGizmo;

		// Token: 0x040028E5 RID: 10469
		private readonly TranslationGizmo _translationGizmo;

		// Token: 0x040028E6 RID: 10470
		private SelectionToolRenderer.SelectionDrawMode _selectionDrawMode = SelectionToolRenderer.SelectionDrawMode.Normal;

		// Token: 0x040028E7 RID: 10471
		private bool renderSideGizmos = true;

		// Token: 0x040028E8 RID: 10472
		private List<Entity> _previewEntities = new List<Entity>(128);

		// Token: 0x040028E9 RID: 10473
		private List<Vector3> _entityOffsetFromRotationOrigin = new List<Vector3>(128);

		// Token: 0x040028EA RID: 10474
		private Matrix _translationMatrix = Matrix.Identity;

		// Token: 0x040028EB RID: 10475
		private Matrix _rotationMatrixSinceStartOfInteraction = Matrix.Identity;

		// Token: 0x040028EC RID: 10476
		private Matrix _cumulativeRotationMatrix = Matrix.Identity;

		// Token: 0x040028ED RID: 10477
		private IntVector3 _minPositionAtBeginningOfTransform;

		// Token: 0x040028EE RID: 10478
		private IntVector3 _maxPositionAtBeginningOfTransform;

		// Token: 0x040028EF RID: 10479
		private Vector3 initialRotationOrigin = default(Vector3);

		// Token: 0x040028F0 RID: 10480
		private Vector3 rotationOrigin = default(Vector3);

		// Token: 0x040028F1 RID: 10481
		private IntVector3 lastBlockActivatedOnTranslation = IntVector3.Zero;

		// Token: 0x040028F2 RID: 10482
		private Vector3 lastRotation = default(Vector3);

		// Token: 0x040028F3 RID: 10483
		private Vector3 positionOneOffsetFromRotationPoint = default(Vector3);

		// Token: 0x040028F4 RID: 10484
		private Vector3 positionTwoOffsetFromRotationPoint = default(Vector3);

		// Token: 0x040028F5 RID: 10485
		private EditorBlocksChange.BlockChange[] _cachedBlockChanges;

		// Token: 0x040028F6 RID: 10486
		private IntVector3 initialPasteLocationForPasteMode = IntVector3.Min;

		// Token: 0x040028F7 RID: 10487
		private bool shouldCutOriginal = false;

		// Token: 0x040028F8 RID: 10488
		private Dictionary<IntVector3, int> blockIdsAtLocationOfGizmoEdit = new Dictionary<IntVector3, int>();

		// Token: 0x040028F9 RID: 10489
		private List<List<Entity>> extrudeLayerPreviewEntities = new List<List<Entity>>();

		// Token: 0x040028FA RID: 10490
		private Dictionary<IntVector3, int> extrusionPreviewEntityOffsets = new Dictionary<IntVector3, int>();

		// Token: 0x040028FB RID: 10491
		private IntVector3 minExtrusionCoordinate;

		// Token: 0x040028FC RID: 10492
		private IntVector3 maxExtrusionCoordinate;

		// Token: 0x040028FD RID: 10493
		private int currentExtrusionDepth;

		// Token: 0x040028FE RID: 10494
		private HitDetection.RayBoxCollision _rayBoxHit;

		// Token: 0x040028FF RID: 10495
		private Vector3 _resizePosition1;

		// Token: 0x04002900 RID: 10496
		private Vector3 _resizePosition2;

		// Token: 0x04002901 RID: 10497
		private Vector3 _resizeNormal;

		// Token: 0x04002902 RID: 10498
		private Vector3 _resizeOrigin;

		// Token: 0x04002903 RID: 10499
		private float _resizeDistance;

		// Token: 0x04002904 RID: 10500
		private PlaySelectionTool.Direction _resizeDirection = PlaySelectionTool.Direction.None;

		// Token: 0x04002905 RID: 10501
		public Vector3[] _blockFaceNormals = new Vector3[]
		{
			Vector3.Up,
			Vector3.Down,
			Vector3.Backward,
			Vector3.Forward,
			Vector3.Left,
			Vector3.Right
		};

		// Token: 0x04002906 RID: 10502
		private readonly Dictionary<PlaySelectionTool.Keybind, SDL.SDL_Scancode> _keybinds = new Dictionary<PlaySelectionTool.Keybind, SDL.SDL_Scancode>
		{
			{
				PlaySelectionTool.Keybind.SelectionShiftUp,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP
			},
			{
				PlaySelectionTool.Keybind.SelectionShiftDown,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN
			},
			{
				PlaySelectionTool.Keybind.SelectionPosOne,
				SDL.SDL_Scancode.SDL_SCANCODE_LEFTBRACKET
			},
			{
				PlaySelectionTool.Keybind.SelectionPosTwo,
				SDL.SDL_Scancode.SDL_SCANCODE_RIGHTBRACKET
			},
			{
				PlaySelectionTool.Keybind.SelectionCopy,
				SDL.SDL_Scancode.SDL_SCANCODE_C
			},
			{
				PlaySelectionTool.Keybind.SelectionClear,
				SDL.SDL_Scancode.SDL_SCANCODE_DELETE
			},
			{
				PlaySelectionTool.Keybind.SelectionNextDrawMode,
				SDL.SDL_Scancode.SDL_SCANCODE_COMMA
			},
			{
				PlaySelectionTool.Keybind.SelectionNextSet,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP
			},
			{
				PlaySelectionTool.Keybind.SelectionPreviousSet,
				SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN
			}
		};

		// Token: 0x04002907 RID: 10503
		public SelectionArea SelectionArea;

		// Token: 0x04002908 RID: 10504
		private Vector3 target = new Vector3(0f, 0f, 0f);

		// Token: 0x02000E76 RID: 3702
		private enum Keybind
		{
			// Token: 0x0400467F RID: 18047
			SelectionShiftUp,
			// Token: 0x04004680 RID: 18048
			SelectionShiftDown,
			// Token: 0x04004681 RID: 18049
			SelectionPosOne,
			// Token: 0x04004682 RID: 18050
			SelectionPosTwo,
			// Token: 0x04004683 RID: 18051
			SelectionCopy,
			// Token: 0x04004684 RID: 18052
			SelectionClear,
			// Token: 0x04004685 RID: 18053
			SelectionNextDrawMode,
			// Token: 0x04004686 RID: 18054
			SelectionNextSet,
			// Token: 0x04004687 RID: 18055
			SelectionPreviousSet
		}

		// Token: 0x02000E77 RID: 3703
		private enum Direction
		{
			// Token: 0x04004689 RID: 18057
			None,
			// Token: 0x0400468A RID: 18058
			Up,
			// Token: 0x0400468B RID: 18059
			Down,
			// Token: 0x0400468C RID: 18060
			Left,
			// Token: 0x0400468D RID: 18061
			Right,
			// Token: 0x0400468E RID: 18062
			Forward,
			// Token: 0x0400468F RID: 18063
			Backward
		}

		// Token: 0x02000E78 RID: 3704
		public enum SelectionPoint
		{
			// Token: 0x04004691 RID: 18065
			PosOne,
			// Token: 0x04004692 RID: 18066
			PosTwo
		}

		// Token: 0x02000E79 RID: 3705
		public enum EditMode
		{
			// Token: 0x04004694 RID: 18068
			None,
			// Token: 0x04004695 RID: 18069
			MoveBox,
			// Token: 0x04004696 RID: 18070
			MovePos1,
			// Token: 0x04004697 RID: 18071
			MovePos2,
			// Token: 0x04004698 RID: 18072
			ResizeSide,
			// Token: 0x04004699 RID: 18073
			ResizePos1,
			// Token: 0x0400469A RID: 18074
			ResizePos2,
			// Token: 0x0400469B RID: 18075
			ExtrudeBlocksFromFace,
			// Token: 0x0400469C RID: 18076
			CreateSelectionSet2DPlane,
			// Token: 0x0400469D RID: 18077
			CreateSelectionSetThirdDimension,
			// Token: 0x0400469E RID: 18078
			TranslateSide,
			// Token: 0x0400469F RID: 18079
			TransformRotation,
			// Token: 0x040046A0 RID: 18080
			TransformTranslationGizmos,
			// Token: 0x040046A1 RID: 18081
			TransformTranslationBrushPoint
		}
	}
}
