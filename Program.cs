// See https://aka.ms/new-console-template for more information
// I HATE THIS!!
using BfresLibrary;
using BfresLibrary.Switch;
using SELib;
using BfresLibrary.Helpers;
namespace SEAnimToBFSka
{
    struct FreakyFrame
    {
        public float Frame;
        public float Value;
    }
    class Program
    {
        static AnimCurve MakeCurve(List<FreakyFrame> frames)
        {
            var curve = new AnimCurve();
            curve.CurveType = AnimCurveType.BakedFloat;
            var size = frames.Count;
            var freakyKeys = new float[size, 1];
            var freakyFrames = new float[size];
            int i = 0;
            foreach (FreakyFrame frame in frames)
            {
                freakyKeys[i, 0] = frame.Value;
                freakyFrames[i] = frame.Frame;
                i++;
            }
            curve.Keys = freakyKeys;
            curve.Frames = freakyFrames;
            return curve;
        }
        static void Main(string[] args)
        {
            if (args.Length < 3) return;
            var referenceModel = new ResFile(args[0]);

            var skelington = referenceModel.Models[0].Skeleton;


            var anim = new SkeletalAnim();
            anim.BindSkeleton = skelington;

            var seanim = SEAnim.Read(args[1]);
            anim.FrameCount = seanim.FrameCount;
            anim.Loop = seanim.Looping;

            foreach (var bone in seanim.Bones)
            {
                if (skelington.Bones.TryGetValue(bone, out var freakyBone))
                {
                    freakyBone.FlagsRotation = BoneFlagsRotation.EulerXYZ;
                    var boneAnim = new BoneAnim();

                    boneAnim.Name = bone;

                    anim.BoneAnims.Add(boneAnim);


                    float PositionX = 0;
                    float PositionY = 0;
                    float PositionZ = 0;

                    float RotationX = 0;
                    float RotationY = 0;
                    float RotationZ = 0;

                    float ScaleX = 0;
                    float ScaleY = 0;
                    float ScaleZ = 0;

                    if (seanim.AnimType == AnimationType.Relative)
                    {
                        PositionX = freakyBone.Position.X;
                        PositionY = freakyBone.Position.Y;
                        PositionZ = freakyBone.Position.Z;

                        RotationX = freakyBone.Rotation.X;
                        RotationY = freakyBone.Rotation.Y;
                        RotationZ = freakyBone.Rotation.Z;

                        ScaleX = freakyBone.Scale.X;
                        ScaleY = freakyBone.Scale.Y;
                        ScaleZ = freakyBone.Scale.Z;

                    }

                    if (seanim.AnimationPositionKeys.TryGetValue(bone, out var translationKeys))
                    {
                        var posCurveX = new List<FreakyFrame>();
                        var posCurveY = new List<FreakyFrame>();
                        var posCurveZ = new List<FreakyFrame>();
                        foreach (SEAnimFrame animFrame in translationKeys)
                        {
                            // I HATE MONDAYS
                            SELib.Utilities.Vector3 data = (SELib.Utilities.Vector3)animFrame.Data;
                            posCurveX.Add(new FreakyFrame()
                            {
                                Value = (float)data.X + PositionX,
                                Frame = animFrame.Frame
                            });
                            posCurveY.Add(new FreakyFrame()
                            {
                                Value = (float)data.Y + PositionY,
                                Frame = animFrame.Frame
                            });
                            posCurveZ.Add(new FreakyFrame()
                            {
                                Value = (float)data.Z + PositionZ,
                                Frame = animFrame.Frame
                            });
                        }
                        if (posCurveX.Any())
                        {
                            var freakyPosX = MakeCurve(posCurveX);
                            freakyPosX.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.PositionX;
                            var freakyPosY = MakeCurve(posCurveY);
                            freakyPosY.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.PositionY;
                            var freakyPosZ = MakeCurve(posCurveZ);
                            freakyPosZ.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.PositionZ;
                            boneAnim.Curves.Add(freakyPosX);
                            boneAnim.Curves.Add(freakyPosY);
                            boneAnim.Curves.Add(freakyPosZ);
                        }
                    }
                    if (seanim.AnimationRotationKeys.TryGetValue(bone, out var rotationKeys))
                    {
                        var rotCurveX = new List<FreakyFrame>();
                        var rotCurveY = new List<FreakyFrame>();
                        var rotCurveZ = new List<FreakyFrame>();
                        foreach (SEAnimFrame animFrame in rotationKeys)
                        {
                            // I HATE MONDAYS
                            SELib.Utilities.Vector3 data = (SELib.Utilities.Vector3)animFrame.Data;
                            rotCurveX.Add(new FreakyFrame()
                            {
                                Value = (float)data.X + RotationX,
                                Frame = animFrame.Frame
                            });
                            rotCurveY.Add(new FreakyFrame()
                            {
                                Value = (float)data.Y + RotationY,
                                Frame = animFrame.Frame
                            });
                            rotCurveZ.Add(new FreakyFrame()
                            {
                                Value = (float)data.Z + RotationZ,
                                Frame = animFrame.Frame
                            });
                        }
                        if (rotCurveX.Any())
                        {
                            var freakyRotX = MakeCurve(rotCurveX);
                            freakyRotX.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.RotateX;
                            var freakyRotY = MakeCurve(rotCurveY);
                            freakyRotY.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.RotateY;
                            var freakyRotZ = MakeCurve(rotCurveZ);
                            freakyRotZ.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.RotateZ;
                            boneAnim.Curves.Add(freakyRotX);
                            boneAnim.Curves.Add(freakyRotY);
                            boneAnim.Curves.Add(freakyRotZ);
                        }
                    }
                    if (seanim.AnimationScaleKeys.TryGetValue(bone, out var scaleKeys))
                    {
                        var scaleCurveX = new List<FreakyFrame>();
                        var scaleCurveY = new List<FreakyFrame>();
                        var scaleCurveZ = new List<FreakyFrame>();
                        foreach (SEAnimFrame animFrame in scaleKeys)
                        {
                            // I HATE MONDAYS
                            SELib.Utilities.Vector3 data = (SELib.Utilities.Vector3)animFrame.Data;
                            scaleCurveX.Add(new FreakyFrame()
                            {
                                Value = (float)data.X + ScaleX,
                                Frame = animFrame.Frame
                            });
                            scaleCurveY.Add(new FreakyFrame()
                            {
                                Value = (float)data.Y + ScaleY,
                                Frame = animFrame.Frame
                            });
                            scaleCurveZ.Add(new FreakyFrame()
                            {
                                Value = (float)data.Z + ScaleZ,
                                Frame = animFrame.Frame
                            });
                        }
                        if (scaleCurveX.Any())
                        {
                            var freakyScaleX = MakeCurve(scaleCurveX);
                            freakyScaleX.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.ScaleX;
                            var freakyScaleY = MakeCurve(scaleCurveY);
                            freakyScaleY.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.ScaleY;
                            var freakyScaleZ = MakeCurve(scaleCurveZ);
                            freakyScaleZ.AnimDataOffset = (uint)SkeletalAnimHelper.AnimTarget.ScaleZ;
                            boneAnim.Curves.Add(freakyScaleX);
                            boneAnim.Curves.Add(freakyScaleY);
                            boneAnim.Curves.Add(freakyScaleZ);
                        }
                    }


                }
            }
            anim.Export(args[2], referenceModel);
            Console.WriteLine("Hello, World!");

        }
    }
}
