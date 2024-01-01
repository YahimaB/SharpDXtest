﻿using System;

using LinearAlgebra;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Engine.BaseAssets.Components
{
    public class SpotLight : Light
    {
        private const float Near = 0.001f;

        [SerializedField]
        private float radius = 1.0f;
        [SerializedField]
        private float intensity = 0.4f;
        [SerializedField]
        private float angularIntensity = 0.4f;
        [SerializedField]
        private float angle = MathF.PI / 3.0f;
        [SerializedField]
        private int shadowSize = 1024;

        public Ranged<float> Radius => new Ranged<float>(ref radius, 0.0f);
        public Ranged<float> Intensity => new Ranged<float>(ref intensity, 0.0f, 1.0f);
        public Ranged<float> AngularIntensity => new Ranged<float>(ref angularIntensity, 0.0f, 1.0f);
        public Ranged<float> Angle => new Ranged<float>(ref angle, 0.0f, MathF.PI);
        public Ranged<int> ShadowSize => new Ranged<int>(ref shadowSize, 0);

        public Matrix4x4f LightSpace
        {
            get
            {
                float ctg = 1f / (float)Math.Tan(angle / 2f);

                Matrix4x4f proj = new Matrix4x4f(ctg, 0, 0, 0,
                                                 0, 0, ctg, 0,
                                                 0, radius / (radius - Near), 0, -radius * Near / (radius - Near),
                                                 0, 1, 0, 0);

                return proj * (Matrix4x4f)GameObject.Transform.View;
            }
        }

        public Texture ShadowTexture { get; private set; }

        public SpotLight()
        {
            ShadowTexture = new Texture(shadowSize, shadowSize, null, Format.R32_Typeless, BindFlags.ShaderResource | BindFlags.DepthStencil);
        }
    }
}