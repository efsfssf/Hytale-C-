using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Items;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x0200099A RID: 2458
	internal abstract class AnimatedRenderer : Disposable
	{
		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06004E77 RID: 20087 RVA: 0x0015C937 File Offset: 0x0015AB37
		public bool SelfManagedNodeBuffer
		{
			get
			{
				return this._selfManageNodeBuffer;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06004E78 RID: 20088 RVA: 0x0015C93F File Offset: 0x0015AB3F
		public BlockyModel Model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06004E79 RID: 20089 RVA: 0x0015C947 File Offset: 0x0015AB47
		// (set) Token: 0x06004E7A RID: 20090 RVA: 0x0015C94F File Offset: 0x0015AB4F
		public GLBuffer NodeBuffer { get; private set; }

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06004E7B RID: 20091 RVA: 0x0015C958 File Offset: 0x0015AB58
		public ushort NodeCount
		{
			get
			{
				return this._nodeCount;
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x0015C960 File Offset: 0x0015AB60
		public AnimatedRenderer(BlockyModel model, Point[] atlasSizes, bool selfManageNodeBuffer)
		{
			this._model = model;
			this._nodeCount = (ushort)this._model.NodeCount;
			this._atlasSizes = atlasSizes;
			this._selfManageNodeBuffer = selfManageNodeBuffer;
			this.NodeMatrices = new Matrix[(int)this._nodeCount];
			this._nodeMatricesHandle = GCHandle.Alloc(this.NodeMatrices, GCHandleType.Pinned);
			this._nodeMatricesAddr = this._nodeMatricesHandle.AddrOfPinnedObject();
			this.NodeTransforms = new AnimatedRenderer.NodeTransform[(int)this._nodeCount];
			this._nodeLocalParentTransforms = new AnimatedRenderer.NodeTransform[(int)this._nodeCount];
			this._targetNodeAnimsPerSlot = new BlockyAnimation.BlockyAnimNodeAnim[(int)(9 * this._nodeCount)];
			this._lastFramesBeforeBlending = new AnimatedRenderer.LastFrame[(int)(9 * this._nodeCount)];
			this._hasLastFramesBeforeBlending = new BitArray((int)(9 * this._nodeCount));
			this._highestActiveSlotByNodes = new int[(int)this._nodeCount];
			for (int i = 0; i < 9; i++)
			{
				this._animationSlots[i] = new AnimatedRenderer.AnimationSlot();
			}
			this.SetupAnimatedNodeSlots();
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x0015CA98 File Offset: 0x0015AC98
		public virtual void CreateGPUData(GraphicsDevice graphics)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._graphics = graphics;
			bool selfManageNodeBuffer = this._selfManageNodeBuffer;
			if (selfManageNodeBuffer)
			{
				GLFunctions gl = graphics.GL;
				this.NodeBuffer = gl.GenBuffer();
			}
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0015CAD8 File Offset: 0x0015ACD8
		protected override void DoDispose()
		{
			this._nodeMatricesHandle.Free();
			bool flag = this._graphics != null && this._selfManageNodeBuffer;
			if (flag)
			{
				GLFunctions gl = this._graphics.GL;
				gl.DeleteBuffer(this.NodeBuffer);
			}
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0015CB28 File Offset: 0x0015AD28
		public void AdvancePlayback(float elapsedTime)
		{
			for (int i = 0; i < 9; i++)
			{
				bool flag;
				this._animationSlots[i].AdvancePlayback(elapsedTime, out flag);
				bool flag2 = flag;
				if (flag2)
				{
					int num = i * (int)this._nodeCount;
					for (int j = 0; j < (int)this._nodeCount; j++)
					{
						this._hasLastFramesBeforeBlending[num + j] = false;
					}
					this._areAnimatedNodeSlotsDirty = true;
				}
			}
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x0015CBA0 File Offset: 0x0015ADA0
		public void CopyAllSlotAnimations(AnimatedRenderer targetModelRenderer)
		{
			for (int i = 0; i < 9; i++)
			{
				this._animationSlots[i].Copy(targetModelRenderer._animationSlots[i]);
			}
			for (int j = 0; j < (int)this._nodeCount; j++)
			{
				int nameId = this._model.AllNodes[j].NameId;
				int num;
				bool flag = targetModelRenderer._model.NodeIndicesByNameId.TryGetValue(nameId, out num);
				if (flag)
				{
					for (int k = 0; k < 9; k++)
					{
						int num2 = k * (int)this._nodeCount + j;
						int num3 = k * (int)targetModelRenderer._nodeCount + num;
						this._hasLastFramesBeforeBlending[j] = targetModelRenderer._hasLastFramesBeforeBlending[num3];
						this._lastFramesBeforeBlending[j] = targetModelRenderer._lastFramesBeforeBlending[num3];
					}
				}
			}
			this._cameraOrientation = targetModelRenderer._cameraOrientation;
			this._areAnimatedNodeSlotsDirty = true;
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0015CCA0 File Offset: 0x0015AEA0
		public void SetSlotAnimation(int slotIndex, BlockyAnimation animation, bool isLooping = true, float speedMultiplier = 1f, float startTime = 0f, float blendingDuration = 0f, ClientItemPullbackConfig pullbackConfig = null, bool force = false)
		{
			bool flag = this._animationSlots[slotIndex].Animation == animation && !force;
			if (flag)
			{
				this._animationSlots[slotIndex].SetPullback(pullbackConfig);
			}
			else
			{
				bool areAnimatedNodeSlotsDirty = this._areAnimatedNodeSlotsDirty;
				if (areAnimatedNodeSlotsDirty)
				{
					this.SetupAnimatedNodeSlots();
				}
				BlockyAnimation animation2 = this._animationSlots[slotIndex].Animation;
				for (int i = 0; i < (int)this._nodeCount; i++)
				{
					BlockyModelNode blockyModelNode = this._model.AllNodes[i];
					int num = slotIndex * (int)this._nodeCount + i;
					bool flag2 = (animation2 != null && animation2.NodeAnimationsByNameId.ContainsKey(blockyModelNode.NameId)) || this._hasLastFramesBeforeBlending[num];
					if (flag2)
					{
						int num2 = this._model.ParentNodes[i];
						ref AnimatedRenderer.NodeTransform ptr = ref this.NodeTransforms[i];
						this.ComputeNodeTransform(ref ptr, blockyModelNode, i, slotIndex);
						this._hasLastFramesBeforeBlending[num] = true;
						this._lastFramesBeforeBlending[num] = new AnimatedRenderer.LastFrame
						{
							Position = ptr.Position,
							Orientation = ptr.Orientation,
							ShapeStretch = this._animShapeStretch,
							Visible = this._animVisible,
							UvOffset = this._animUvOffset
						};
					}
					else
					{
						this._hasLastFramesBeforeBlending[num] = false;
					}
				}
				this._animationSlots[slotIndex].SetAnimation(animation, isLooping, speedMultiplier, startTime, blendingDuration, pullbackConfig);
				this._areAnimatedNodeSlotsDirty = true;
			}
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0015CE34 File Offset: 0x0015B034
		public void SetSlotAnimationNoBlending(int slotIndex, BlockyAnimation animation, bool isLooping = true, float speedMultiplier = 1f, float startTime = 0f)
		{
			bool flag = this._animationSlots[slotIndex].Animation == animation;
			if (!flag)
			{
				int num = slotIndex * (int)this._nodeCount;
				for (int i = 0; i < (int)this._nodeCount; i++)
				{
					this._hasLastFramesBeforeBlending[num + i] = false;
				}
				this._animationSlots[slotIndex].SetAnimationNoBlending(animation, isLooping, speedMultiplier, startTime);
				this._areAnimatedNodeSlotsDirty = true;
			}
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0015CEA1 File Offset: 0x0015B0A1
		public void SetSlotAnimationSpeedMultiplier(int slotIndex, float speedMultiplier)
		{
			this._animationSlots[slotIndex].SpeedMultiplier = speedMultiplier;
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x0015CEB2 File Offset: 0x0015B0B2
		public BlockyAnimation GetSlotAnimation(int slot)
		{
			return this._animationSlots[slot].Animation;
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x0015CEC1 File Offset: 0x0015B0C1
		public float GetSlotAnimationTime(int slot)
		{
			return this._animationSlots[slot].AnimationTime;
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0015CED0 File Offset: 0x0015B0D0
		public float GetSlotAnimationSpeedMultiplier(int slot)
		{
			return this._animationSlots[slot].SpeedMultiplier;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x0015CEDF File Offset: 0x0015B0DF
		public ClientItemPullbackConfig GetSlotPullbackConfig(int slot)
		{
			return this._animationSlots[slot].PullbackConfig;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0015CEF0 File Offset: 0x0015B0F0
		public bool IsSlotPlayingAnimation(int slotIndex)
		{
			AnimatedRenderer.AnimationSlot animationSlot = this._animationSlots[slotIndex];
			bool flag = animationSlot.Animation == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isLooping = animationSlot.IsLooping;
				result = (isLooping || animationSlot.AnimationTime < (float)animationSlot.Animation.Duration);
			}
			return result;
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0015CF40 File Offset: 0x0015B140
		private void SetupAnimatedNodeSlots()
		{
			int i = 0;
			while (i < (int)this._nodeCount)
			{
				ref BlockyModelNode ptr = ref this._model.AllNodes[i];
				bool flag = false;
				this._highestActiveSlotByNodes[i] = -1;
				bool flag2 = !ptr.IsPiece;
				if (flag2)
				{
					for (int j = 8; j >= 0; j--)
					{
						bool flag3 = false;
						AnimatedRenderer.AnimationSlot animationSlot = this._animationSlots[j];
						int num = j * (int)this._nodeCount + i;
						bool flag4 = animationSlot.IsBlending && this._hasLastFramesBeforeBlending[num];
						if (flag4)
						{
							flag = true;
							flag3 = true;
						}
						bool flag5 = animationSlot.Animation != null && animationSlot.Animation.NodeAnimationsByNameId.TryGetValue(ptr.NameId, out this._targetNodeAnimsPerSlot[num]);
						if (flag5)
						{
							flag = true;
							flag3 = true;
						}
						else
						{
							this._targetNodeAnimsPerSlot[num] = null;
						}
						bool flag6 = flag3 && this._highestActiveSlotByNodes[i] == -1;
						if (flag6)
						{
							this._highestActiveSlotByNodes[i] = j;
						}
					}
				}
				bool flag7 = !flag;
				if (flag7)
				{
					bool flag8 = ptr.CameraNode > 0;
					if (flag8)
					{
						this._highestActiveSlotByNodes[i] = 0;
					}
					else
					{
						this._nodeLocalParentTransforms[i].Position = ptr.Position + Vector3.Transform(ptr.Offset, ptr.Orientation) + Vector3.Transform(ptr.ProceduralOffset, Quaternion.Identity);
						Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(ptr.ProceduralRotation.Yaw, ptr.ProceduralRotation.Pitch, ptr.ProceduralRotation.Roll);
						this._nodeLocalParentTransforms[i].Orientation = quaternion * ptr.Orientation;
					}
				}
				IL_1BD:
				i++;
				continue;
				goto IL_1BD;
			}
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x0015D120 File Offset: 0x0015B320
		public void SetCameraOrientation(Quaternion orientation)
		{
			this._cameraOrientation = orientation;
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x0015D12C File Offset: 0x0015B32C
		public void SetCameraNodes(CameraSettings cameraSettings)
		{
			for (int i = 0; i < this._cameraNodes.Length; i++)
			{
				this._cameraNodes[i] = AnimatedRenderer.CameraControlNode.None;
			}
			int num = 0;
			for (;;)
			{
				int num2 = num;
				CameraAxis yaw = cameraSettings.Yaw;
				if (num2 >= ((yaw != null) ? yaw.TargetNodes.Length : 0))
				{
					break;
				}
				this._cameraNodes[cameraSettings.Yaw.TargetNodes[num]] = AnimatedRenderer.CameraControlNode.LookYaw;
				num++;
			}
			int num3 = 0;
			for (;;)
			{
				int num4 = num3;
				CameraAxis pitch = cameraSettings.Pitch;
				if (num4 >= ((pitch != null) ? pitch.TargetNodes.Length : 0))
				{
					break;
				}
				bool flag = this._cameraNodes[cameraSettings.Pitch.TargetNodes[num3]] == AnimatedRenderer.CameraControlNode.LookYaw;
				if (flag)
				{
					this._cameraNodes[cameraSettings.Pitch.TargetNodes[num3]] = AnimatedRenderer.CameraControlNode.Look;
				}
				else
				{
					this._cameraNodes[cameraSettings.Pitch.TargetNodes[num3]] = AnimatedRenderer.CameraControlNode.LookPitch;
				}
				num3++;
			}
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x0015D20C File Offset: 0x0015B40C
		public void UpdatePose()
		{
			bool areAnimatedNodeSlotsDirty = this._areAnimatedNodeSlotsDirty;
			if (areAnimatedNodeSlotsDirty)
			{
				this.SetupAnimatedNodeSlots();
				this._areAnimatedNodeSlotsDirty = false;
			}
			for (int i = 0; i < (int)this._nodeCount; i++)
			{
				this.UpdatePoseForNode(ref this._model.AllNodes[i], i);
			}
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x0015D264 File Offset: 0x0015B464
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SendDataToGPU()
		{
			Debug.Assert(this._selfManageNodeBuffer, "Error: trying to send Node data to a GPU buffer when _selfManageNodeBuffer is false.");
			GLFunctions gl = this._graphics.GL;
			gl.BindBuffer(GL.UNIFORM_BUFFER, this.NodeBuffer);
			gl.BufferData(GL.UNIFORM_BUFFER, (IntPtr)((int)(this._nodeCount * 64)), this._nodeMatricesAddr, GL.DYNAMIC_DRAW);
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x0015D2CC File Offset: 0x0015B4CC
		private void UpdatePoseForNode(ref BlockyModelNode node, int nodeIndex)
		{
			int num = this._model.ParentNodes[nodeIndex];
			ref AnimatedRenderer.NodeTransform ptr = ref this.NodeTransforms[nodeIndex];
			bool flag = this._highestActiveSlotByNodes[nodeIndex] == -1;
			if (flag)
			{
				bool flag2 = num == -1;
				if (flag2)
				{
					ptr.Position = this._nodeLocalParentTransforms[nodeIndex].Position;
					ptr.Orientation = this._nodeLocalParentTransforms[nodeIndex].Orientation;
				}
				else
				{
					ptr.Position = Vector3.Transform(this._nodeLocalParentTransforms[nodeIndex].Position, this.NodeTransforms[num].Orientation) + this.NodeTransforms[num].Position;
					ptr.Orientation = this.NodeTransforms[num].Orientation * this._nodeLocalParentTransforms[nodeIndex].Orientation;
				}
				bool visible = node.Visible;
				if (visible)
				{
					Matrix.Compose(ptr.Orientation, ptr.Position, out this.NodeMatrices[nodeIndex]);
				}
				else
				{
					Matrix.CreateScale(0f, out this.NodeMatrices[nodeIndex]);
				}
			}
			else
			{
				int slotIndex = this._highestActiveSlotByNodes[nodeIndex];
				this.ComputeNodeTransform(ref ptr, node, nodeIndex, slotIndex);
				bool flag3 = !node.IsPiece && node.CameraNode > 0;
				if (flag3)
				{
					int cameraNode = node.CameraNode;
					bool flag4 = this._cameraNodes[cameraNode] == AnimatedRenderer.CameraControlNode.Look;
					if (flag4)
					{
						Quaternion.Multiply(ref ptr.Orientation, ref this._cameraOrientation, out ptr.Orientation);
					}
					else
					{
						bool flag5 = this._cameraNodes[cameraNode] == AnimatedRenderer.CameraControlNode.LookYaw;
						if (flag5)
						{
							Vector3 planeNormal = Vector3.Transform(Vector3.Up, ptr.Orientation);
							Vector3 vector = Vector3.Transform(Vector3.Forward, this._cameraOrientation);
							Vector3 vector2 = Vector3.ProjectOnPlane(vector, planeNormal);
							Vector3 forward = Vector3.Forward;
							Quaternion quaternion;
							Quaternion.CreateFromNormalizedVectors(ref forward, ref vector2, out quaternion);
							ptr.Orientation = quaternion * ptr.Orientation;
						}
						else
						{
							bool flag6 = this._cameraNodes[cameraNode] == AnimatedRenderer.CameraControlNode.LookPitch;
							if (flag6)
							{
								Vector3 planeNormal2 = Vector3.Transform(Vector3.Right, ptr.Orientation);
								Vector3 vector3 = Vector3.Transform(Vector3.Up, this._cameraOrientation);
								Vector3 vector4 = Vector3.ProjectOnPlane(vector3, planeNormal2);
								Vector3 up = Vector3.Up;
								Quaternion quaternion2;
								Quaternion.CreateFromNormalizedVectors(ref up, ref vector4, out quaternion2);
								ptr.Orientation = quaternion2 * ptr.Orientation;
							}
						}
					}
				}
				ptr.Position += Vector3.Transform(node.Offset, ptr.Orientation);
				ptr.Position += Vector3.Transform(node.ProceduralOffset, Quaternion.Identity);
				bool flag7 = num != -1;
				if (flag7)
				{
					ptr.Position = Vector3.Transform(ptr.Position, this.NodeTransforms[num].Orientation) + this.NodeTransforms[num].Position;
					ptr.Orientation = this.NodeTransforms[num].Orientation * ptr.Orientation;
				}
				Matrix.Compose(ptr.Orientation, ptr.Position, out this.NodeMatrices[nodeIndex]);
				Matrix.ApplyScale(ref this.NodeMatrices[nodeIndex], this._animShapeStretch);
				this.NodeMatrices[nodeIndex].M14 = (float)this._animUvOffset.X / (float)this._atlasSizes[(int)node.AtlasIndex].X;
				this.NodeMatrices[nodeIndex].M24 = -(float)this._animUvOffset.Y / (float)this._atlasSizes[(int)node.AtlasIndex].Y;
			}
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x0015D69C File Offset: 0x0015B89C
		private void ComputeNodeTransform(ref AnimatedRenderer.NodeTransform refNodeTransform, BlockyModelNode node, int nodeIndex, int slotIndex)
		{
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(node.ProceduralRotation.Yaw, node.ProceduralRotation.Pitch, node.ProceduralRotation.Roll);
			refNodeTransform.Position = node.Position;
			refNodeTransform.Orientation = quaternion * node.Orientation;
			this._animShapeStretch = Vector3.One;
			this._animUvOffset.X = 0;
			this._animUvOffset.Y = 0;
			this._animVisible = node.Visible;
			AnimatedRenderer.AnimationSlot animationSlot = this._animationSlots[slotIndex];
			bool isBlending = animationSlot.IsBlending;
			if (isBlending)
			{
				this._targetAnimPosition = refNodeTransform.Position;
				this._targetAnimOrientation = refNodeTransform.Orientation;
				this._targetAnimShapeStretch = this._animShapeStretch;
				int num = -1;
				for (int i = slotIndex; i >= 0; i--)
				{
					int num2 = i * (int)this._nodeCount + nodeIndex;
					bool flag = this._hasLastFramesBeforeBlending[num2];
					if (flag)
					{
						num = num2;
						break;
					}
				}
				bool flag2 = num != -1;
				if (flag2)
				{
					ref AnimatedRenderer.LastFrame ptr = ref this._lastFramesBeforeBlending[num];
					refNodeTransform.Position = ptr.Position;
					refNodeTransform.Orientation = ptr.Orientation;
					this._animShapeStretch = ptr.ShapeStretch;
					this._animVisible = ptr.Visible;
					this._animUvOffset = ptr.UvOffset;
				}
				else
				{
					BlockyAnimation.BlockyAnimNodeAnim blockyAnimNodeAnim = null;
					float time = 0f;
					for (int j = slotIndex - 1; j >= 0; j--)
					{
						blockyAnimNodeAnim = this._targetNodeAnimsPerSlot[j * (int)this._nodeCount + nodeIndex];
						time = this._animationSlots[j].AnimationTime;
						bool flag3 = blockyAnimNodeAnim != null;
						if (flag3)
						{
							break;
						}
					}
					bool flag4 = blockyAnimNodeAnim != null;
					if (flag4)
					{
						BlockyAnimation.BlockyAnimNodeFrame blockyAnimNodeFrame;
						BlockyAnimation.BlockyAnimNodeFrame blockyAnimNodeFrame2;
						float amount;
						this.GetInterpolationData(blockyAnimNodeAnim.Frames, time, out blockyAnimNodeFrame, out blockyAnimNodeFrame2, out amount);
						bool hasPosition = blockyAnimNodeAnim.HasPosition;
						if (hasPosition)
						{
							refNodeTransform.Position += Vector3.Lerp(blockyAnimNodeFrame.Position, blockyAnimNodeFrame2.Position, amount);
						}
						bool hasOrientation = blockyAnimNodeAnim.HasOrientation;
						if (hasOrientation)
						{
							Quaternion quaternion2 = Quaternion.Lerp(blockyAnimNodeFrame.Orientation, blockyAnimNodeFrame2.Orientation, amount);
							Quaternion.Multiply(ref refNodeTransform.Orientation, ref quaternion2, out refNodeTransform.Orientation);
						}
						bool hasShapeStretch = blockyAnimNodeAnim.HasShapeStretch;
						if (hasShapeStretch)
						{
							this._animShapeStretch *= Vector3.Lerp(blockyAnimNodeFrame.ShapeStretch, blockyAnimNodeFrame2.ShapeStretch, amount);
						}
						bool hasShapeVisible = blockyAnimNodeAnim.HasShapeVisible;
						if (hasShapeVisible)
						{
							this._animVisible = blockyAnimNodeFrame.ShapeVisible;
						}
						bool hasShapeUvOffset = blockyAnimNodeAnim.HasShapeUvOffset;
						if (hasShapeUvOffset)
						{
							this._animUvOffset = blockyAnimNodeFrame.ShapeUvOffset;
						}
					}
				}
				float blendingProgress = animationSlot.BlendingProgress;
				float animationTime = animationSlot.AnimationTime;
				BlockyAnimation.BlockyAnimNodeAnim blockyAnimNodeAnim2 = this._targetNodeAnimsPerSlot[slotIndex * (int)this._nodeCount + nodeIndex];
				bool flag5 = blockyAnimNodeAnim2 == null;
				if (flag5)
				{
					for (int k = slotIndex - 1; k >= 0; k--)
					{
						blockyAnimNodeAnim2 = this._targetNodeAnimsPerSlot[k * (int)this._nodeCount + nodeIndex];
						animationTime = this._animationSlots[k].AnimationTime;
						bool flag6 = blockyAnimNodeAnim2 != null;
						if (flag6)
						{
							break;
						}
					}
				}
				bool flag7 = blockyAnimNodeAnim2 != null;
				if (flag7)
				{
					BlockyAnimation.BlockyAnimNodeFrame blockyAnimNodeFrame3;
					BlockyAnimation.BlockyAnimNodeFrame blockyAnimNodeFrame4;
					float amount2;
					this.GetInterpolationData(blockyAnimNodeAnim2.Frames, animationTime, out blockyAnimNodeFrame3, out blockyAnimNodeFrame4, out amount2);
					bool hasPosition2 = blockyAnimNodeAnim2.HasPosition;
					if (hasPosition2)
					{
						this._targetAnimPosition += Vector3.Lerp(blockyAnimNodeFrame3.Position, blockyAnimNodeFrame4.Position, amount2);
					}
					bool hasOrientation2 = blockyAnimNodeAnim2.HasOrientation;
					if (hasOrientation2)
					{
						Quaternion quaternion3 = Quaternion.Lerp(blockyAnimNodeFrame3.Orientation, blockyAnimNodeFrame4.Orientation, amount2);
						Quaternion.Multiply(ref this._targetAnimOrientation, ref quaternion3, out this._targetAnimOrientation);
					}
					bool hasShapeStretch2 = blockyAnimNodeAnim2.HasShapeStretch;
					if (hasShapeStretch2)
					{
						this._targetAnimShapeStretch *= Vector3.Lerp(blockyAnimNodeFrame3.ShapeStretch, blockyAnimNodeFrame4.ShapeStretch, amount2);
					}
					bool hasShapeVisible2 = blockyAnimNodeAnim2.HasShapeVisible;
					if (hasShapeVisible2)
					{
						this._animVisible = blockyAnimNodeFrame3.ShapeVisible;
					}
					bool hasShapeUvOffset2 = blockyAnimNodeAnim2.HasShapeUvOffset;
					if (hasShapeUvOffset2)
					{
						this._animUvOffset = blockyAnimNodeFrame3.ShapeUvOffset;
					}
				}
				Vector3.Lerp(ref refNodeTransform.Position, ref this._targetAnimPosition, blendingProgress, out refNodeTransform.Position);
				Quaternion.Lerp(ref refNodeTransform.Orientation, ref this._targetAnimOrientation, blendingProgress, out refNodeTransform.Orientation);
				Vector3.Lerp(ref this._animShapeStretch, ref this._targetAnimShapeStretch, blendingProgress, out this._animShapeStretch);
			}
			else
			{
				float animationTime2 = animationSlot.AnimationTime;
				BlockyAnimation.BlockyAnimNodeAnim blockyAnimNodeAnim3 = this._targetNodeAnimsPerSlot[slotIndex * (int)this._nodeCount + nodeIndex];
				bool flag8 = blockyAnimNodeAnim3 == null;
				if (flag8)
				{
					for (int l = slotIndex - 1; l >= 0; l--)
					{
						blockyAnimNodeAnim3 = this._targetNodeAnimsPerSlot[l * (int)this._nodeCount + nodeIndex];
						animationTime2 = this._animationSlots[l].AnimationTime;
						bool flag9 = blockyAnimNodeAnim3 != null;
						if (flag9)
						{
							break;
						}
					}
				}
				bool flag10 = blockyAnimNodeAnim3 != null;
				if (flag10)
				{
					BlockyAnimation.BlockyAnimNodeFrame blockyAnimNodeFrame5;
					BlockyAnimation.BlockyAnimNodeFrame blockyAnimNodeFrame6;
					float amount3;
					this.GetInterpolationData(blockyAnimNodeAnim3.Frames, animationTime2, out blockyAnimNodeFrame5, out blockyAnimNodeFrame6, out amount3);
					bool hasPosition3 = blockyAnimNodeAnim3.HasPosition;
					if (hasPosition3)
					{
						refNodeTransform.Position += Vector3.Lerp(blockyAnimNodeFrame5.Position, blockyAnimNodeFrame6.Position, amount3);
					}
					bool hasOrientation3 = blockyAnimNodeAnim3.HasOrientation;
					if (hasOrientation3)
					{
						Quaternion quaternion4 = Quaternion.Lerp(blockyAnimNodeFrame5.Orientation, blockyAnimNodeFrame6.Orientation, amount3);
						Quaternion.Multiply(ref refNodeTransform.Orientation, ref quaternion4, out refNodeTransform.Orientation);
					}
					bool hasShapeStretch3 = blockyAnimNodeAnim3.HasShapeStretch;
					if (hasShapeStretch3)
					{
						this._animShapeStretch *= Vector3.Lerp(blockyAnimNodeFrame5.ShapeStretch, blockyAnimNodeFrame6.ShapeStretch, amount3);
					}
					bool hasShapeVisible3 = blockyAnimNodeAnim3.HasShapeVisible;
					if (hasShapeVisible3)
					{
						this._animVisible = blockyAnimNodeFrame5.ShapeVisible;
					}
					bool hasShapeUvOffset3 = blockyAnimNodeAnim3.HasShapeUvOffset;
					if (hasShapeUvOffset3)
					{
						this._animUvOffset = blockyAnimNodeFrame5.ShapeUvOffset;
					}
				}
			}
			bool flag11 = !this._animVisible;
			if (flag11)
			{
				this._animShapeStretch.X = (this._animShapeStretch.Y = (this._animShapeStretch.Z = 0f));
			}
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x0015DCE4 File Offset: 0x0015BEE4
		private void GetInterpolationData(BlockyAnimation.BlockyAnimNodeFrame[] frames, float time, out BlockyAnimation.BlockyAnimNodeFrame previousFrame, out BlockyAnimation.BlockyAnimNodeFrame nextFrame, out float delta)
		{
			previousFrame = frames[(int)time];
			nextFrame = frames[((int)time + 1) % frames.Length];
			delta = time - (float)((int)time);
		}

		// Token: 0x040029CD RID: 10701
		public uint NodeBufferOffset;

		// Token: 0x040029CE RID: 10702
		private readonly ushort _nodeCount;

		// Token: 0x040029CF RID: 10703
		private bool _selfManageNodeBuffer;

		// Token: 0x040029D0 RID: 10704
		private readonly BlockyModel _model;

		// Token: 0x040029D1 RID: 10705
		private readonly Point[] _atlasSizes;

		// Token: 0x040029D2 RID: 10706
		protected GraphicsDevice _graphics;

		// Token: 0x040029D3 RID: 10707
		private const int MaxAnimationSlots = 9;

		// Token: 0x040029D4 RID: 10708
		private AnimatedRenderer.AnimationSlot[] _animationSlots = new AnimatedRenderer.AnimationSlot[9];

		// Token: 0x040029D5 RID: 10709
		public readonly Matrix[] NodeMatrices;

		// Token: 0x040029D6 RID: 10710
		public readonly AnimatedRenderer.NodeTransform[] NodeTransforms;

		// Token: 0x040029D7 RID: 10711
		private readonly GCHandle _nodeMatricesHandle;

		// Token: 0x040029D8 RID: 10712
		protected readonly IntPtr _nodeMatricesAddr;

		// Token: 0x040029D9 RID: 10713
		private readonly AnimatedRenderer.NodeTransform[] _nodeLocalParentTransforms;

		// Token: 0x040029DA RID: 10714
		private readonly BlockyAnimation.BlockyAnimNodeAnim[] _targetNodeAnimsPerSlot;

		// Token: 0x040029DB RID: 10715
		private AnimatedRenderer.LastFrame[] _lastFramesBeforeBlending;

		// Token: 0x040029DC RID: 10716
		private BitArray _hasLastFramesBeforeBlending;

		// Token: 0x040029DD RID: 10717
		private readonly int[] _highestActiveSlotByNodes;

		// Token: 0x040029DE RID: 10718
		private bool _areAnimatedNodeSlotsDirty;

		// Token: 0x040029DF RID: 10719
		private Vector3 _animShapeStretch;

		// Token: 0x040029E0 RID: 10720
		private Point _animUvOffset;

		// Token: 0x040029E1 RID: 10721
		private bool _animVisible;

		// Token: 0x040029E2 RID: 10722
		private Vector3 _targetAnimPosition;

		// Token: 0x040029E3 RID: 10723
		private Quaternion _targetAnimOrientation;

		// Token: 0x040029E4 RID: 10724
		private Vector3 _targetAnimShapeStretch;

		// Token: 0x040029E5 RID: 10725
		private Quaternion _cameraOrientation = Quaternion.Identity;

		// Token: 0x040029E6 RID: 10726
		private AnimatedRenderer.CameraControlNode[] _cameraNodes = new AnimatedRenderer.CameraControlNode[Enum.GetNames(typeof(CameraNode)).Length];

		// Token: 0x02000E89 RID: 3721
		public class AnimationSlot
		{
			// Token: 0x1700146E RID: 5230
			// (get) Token: 0x060067C4 RID: 26564 RVA: 0x002189F2 File Offset: 0x00216BF2
			// (set) Token: 0x060067C5 RID: 26565 RVA: 0x002189FA File Offset: 0x00216BFA
			public BlockyAnimation Animation { get; private set; }

			// Token: 0x1700146F RID: 5231
			// (get) Token: 0x060067C6 RID: 26566 RVA: 0x00218A03 File Offset: 0x00216C03
			// (set) Token: 0x060067C7 RID: 26567 RVA: 0x00218A0B File Offset: 0x00216C0B
			public bool IsLooping { get; private set; }

			// Token: 0x17001470 RID: 5232
			// (get) Token: 0x060067C8 RID: 26568 RVA: 0x00218A14 File Offset: 0x00216C14
			// (set) Token: 0x060067C9 RID: 26569 RVA: 0x00218A1C File Offset: 0x00216C1C
			public ClientItemPullbackConfig PullbackConfig { get; private set; }

			// Token: 0x17001471 RID: 5233
			// (get) Token: 0x060067CA RID: 26570 RVA: 0x00218A25 File Offset: 0x00216C25
			// (set) Token: 0x060067CB RID: 26571 RVA: 0x00218A2D File Offset: 0x00216C2D
			public float AnimationTime { get; private set; }

			// Token: 0x17001472 RID: 5234
			// (get) Token: 0x060067CC RID: 26572 RVA: 0x00218A36 File Offset: 0x00216C36
			public float BlendingProgress
			{
				get
				{
					return Easing.CubicEaseInAndOut(this._blendingTime / this._blendingDuration);
				}
			}

			// Token: 0x17001473 RID: 5235
			// (get) Token: 0x060067CD RID: 26573 RVA: 0x00218A4A File Offset: 0x00216C4A
			public bool IsBlending
			{
				get
				{
					return this._blendingDuration != 0f;
				}
			}

			// Token: 0x060067CE RID: 26574 RVA: 0x00218A5C File Offset: 0x00216C5C
			public void Copy(AnimatedRenderer.AnimationSlot targetAnimationSlot)
			{
				this.Animation = targetAnimationSlot.Animation;
				this.IsLooping = targetAnimationSlot.IsLooping;
				this.SpeedMultiplier = targetAnimationSlot.SpeedMultiplier;
				this.AnimationTime = targetAnimationSlot.AnimationTime;
				this._blendingTime = targetAnimationSlot._blendingTime;
				this._blendingDuration = targetAnimationSlot._blendingDuration;
			}

			// Token: 0x060067CF RID: 26575 RVA: 0x00218AB8 File Offset: 0x00216CB8
			public void SetAnimation(BlockyAnimation animation, bool isLooping = true, float speedMultiplier = 1f, float startTime = 0f, float blendingDuration = 0f, ClientItemPullbackConfig pullbackConfig = null)
			{
				this.Animation = animation;
				this.IsLooping = isLooping;
				this.SpeedMultiplier = speedMultiplier;
				this.PullbackConfig = pullbackConfig;
				bool flag = this.Animation != null;
				if (flag)
				{
					bool flag2 = !isLooping && startTime >= (float)this.Animation.Duration;
					if (flag2)
					{
						this.AnimationTime = (float)this.Animation.Duration;
					}
					else
					{
						this.AnimationTime = startTime % (float)this.Animation.Duration;
					}
				}
				this._blendingDuration = blendingDuration;
				this._blendingTime = 0f;
			}

			// Token: 0x060067D0 RID: 26576 RVA: 0x00218B50 File Offset: 0x00216D50
			public void SetAnimationNoBlending(BlockyAnimation animation, bool isLooping = true, float speedMultiplier = 1f, float startTime = 0f)
			{
				this.Animation = animation;
				this.IsLooping = isLooping;
				this.SpeedMultiplier = speedMultiplier;
				bool flag = this.Animation != null;
				if (flag)
				{
					bool flag2 = !isLooping && startTime >= (float)this.Animation.Duration;
					if (flag2)
					{
						this.AnimationTime = (float)this.Animation.Duration;
					}
					else
					{
						this.AnimationTime = startTime % (float)this.Animation.Duration;
					}
				}
				this._blendingDuration = 0f;
			}

			// Token: 0x060067D1 RID: 26577 RVA: 0x00218BD7 File Offset: 0x00216DD7
			public void SetPullback(ClientItemPullbackConfig pullbackConfig)
			{
				this.PullbackConfig = pullbackConfig;
			}

			// Token: 0x060067D2 RID: 26578 RVA: 0x00218BE4 File Offset: 0x00216DE4
			public void AdvancePlayback(float elapsedTime, out bool finishedBlending)
			{
				finishedBlending = false;
				bool isBlending = this.IsBlending;
				if (isBlending)
				{
					this._blendingTime += elapsedTime;
					bool flag = this._blendingTime >= this._blendingDuration;
					if (flag)
					{
						finishedBlending = true;
						this._blendingDuration = 0f;
					}
				}
				bool flag2 = this.Animation != null;
				if (flag2)
				{
					this.AnimationTime += elapsedTime * this.SpeedMultiplier;
					bool flag3 = this.Animation != null && this.AnimationTime >= (float)this.Animation.Duration;
					if (flag3)
					{
						bool isLooping = this.IsLooping;
						if (isLooping)
						{
							this.AnimationTime %= (float)this.Animation.Duration;
						}
						else
						{
							this.AnimationTime = (float)this.Animation.Duration;
						}
					}
				}
			}

			// Token: 0x040046F4 RID: 18164
			public float SpeedMultiplier;

			// Token: 0x040046F7 RID: 18167
			private float _blendingTime;

			// Token: 0x040046F8 RID: 18168
			private float _blendingDuration;
		}

		// Token: 0x02000E8A RID: 3722
		public struct NodeTransform
		{
			// Token: 0x040046F9 RID: 18169
			public Vector3 Position;

			// Token: 0x040046FA RID: 18170
			public Quaternion Orientation;
		}

		// Token: 0x02000E8B RID: 3723
		private struct LastFrame
		{
			// Token: 0x040046FB RID: 18171
			public Vector3 Position;

			// Token: 0x040046FC RID: 18172
			public Quaternion Orientation;

			// Token: 0x040046FD RID: 18173
			public Vector3 ShapeStretch;

			// Token: 0x040046FE RID: 18174
			public bool Visible;

			// Token: 0x040046FF RID: 18175
			public Point UvOffset;
		}

		// Token: 0x02000E8C RID: 3724
		public enum CameraControlNode
		{
			// Token: 0x04004701 RID: 18177
			None,
			// Token: 0x04004702 RID: 18178
			LookYaw,
			// Token: 0x04004703 RID: 18179
			LookPitch,
			// Token: 0x04004704 RID: 18180
			Look
		}
	}
}
