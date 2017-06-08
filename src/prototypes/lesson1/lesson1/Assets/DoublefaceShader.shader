Shader "DoubleFaceShader" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{

		Pass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }
		Color[_PPLAmbient]
		SetTexture[_BumpMap]{
		constantColor(.5,.5,.5)
		combine constant lerp(texture) previous
	}
		SetTexture[_MainTex]{
		constantColor[_Color]
		Combine texture * previous DOUBLE, texture*constant
	}
	}

		Pass{
		Name "BASE"
		Tags{ "LightMode" = "Vertex" }
		Material{
		Diffuse[_Color]
		Emission[_PPLAmbient]
		Shininess[_Shininess]
		Specular[_SpecColor]
	}
		SeparateSpecular On
		Lighting Off //Sin que sea afectado por la luz
		Cull Off
		SetTexture[_BumpMap]{
		constantColor(.5,.5,.5)
		combine constant lerp(texture) previous
	}
		SetTexture[_MainTex]{
		Combine texture * previous DOUBLE, texture*primary
	}
	}
	}
		FallBack "Diffuse", 1
}