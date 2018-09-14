Shader "Custom/Unlit/TextureCullOff" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Off
		LOD 100

		Pass{
		Lighting Off
		SetTexture[_MainTex]{ combine texture }
	}
	}
}