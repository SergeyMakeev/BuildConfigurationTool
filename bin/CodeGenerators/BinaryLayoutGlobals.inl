//=============================================================================================================================
//
//Глобальный файл общего назначения для генератора BinaryLayout.
//
//=============================================================================================================================


$LayoutGlobalNamespaces(DefaultGenerateNamespace = LayoutGenerated; SerializeCodeNamespace = BinaryLayoutSerialize;);


//=============================================================================================================================
//Basic types

$LayoutDeclareNative(typein = byte; kind = compiler; size = 8);
$LayoutDeclareNative(typein = int8; kind = compiler; size = 8);
$LayoutDeclareNative(typein = uint8; kind = compiler; size = 8);
$LayoutDeclareNative(typein = int16; kind = compiler; size = 16);
$LayoutDeclareNative(typein = uint16; kind = compiler; size = 16);
$LayoutDeclareNative(typein = int32; kind = compiler; size = 32);
$LayoutDeclareNative(typein = uint32; kind = compiler; size = 32);
$LayoutDeclareNative(typein = int64; kind = compiler; size = 64);
$LayoutDeclareNative(typein = uint64; kind = compiler; size = 64);

$LayoutDeclareNative(typein = float; kind = compiler; size = 32);
$LayoutDeclareNative(typein = double; kind = compiler; size = 64);

$LayoutDeclareNative(typein = bool; kind = compiler; size = 8);


//=============================================================================================================================

using namespace std;

namespace std
{
	$LayoutDeclareNative(typein = wstring; typeout = layout_wstring; kind = usertype; size = 64; typealing = 32);
	$LayoutDeclareNative(typein = string; typeout = layout_string; kind = usertype; size = 64; typealing = 32);
	$LayoutDeclareNative(typein = vector<T>; typeout = layout_vector; kind = vector; size = 64; typealing = 32);
	$LayoutDeclareNative(typein = array<T, N>; typeout = layout_array; kind = array; size = 32; typealing = 32);
}
$LayoutDeclareNative(typein = Strong<T>; typeout = layout_ptr; kind = pointer; size = 32);


//=============================================================================================================================

$LayoutAttribute(Ignore)
$LayoutStructSerializable()
$LayoutStructMacros(name = LAYOUT_SERIALIZABLE; group = Unique)
struct LayoutSerializable
{
};


//=============================================================================================================================
//2 components

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct vec2
{
	float x;
	float y;
};

typedef vec2 float2;

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct int2
{
	int32 x;
	int32 y;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct short2
{
	int16 x;
	int16 y;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct byte2
{
	byte x;
	byte y;
};


//=============================================================================================================================
//3 components

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct vec3
{
	float x;
	float y;
	float z;
};

typedef vec3 float3;

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct int3
{
	int32 x;
	int32 y;
	int32 z;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct short3
{
	int16 x;
	int16 y;
	int16 z;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct byte3
{
	byte x;
	byte y;
	byte z;
};


//=============================================================================================================================
//3 components

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct vec4
{
	float x;
	float y;
	float z;
	float w;
};

typedef vec4 float4;

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct int4
{
	int32 x;
	int32 y;
	int32 z;
	int32 w;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct short4
{
	int16 x;
	int16 y;
	int16 z;
	int16 w;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct byte4
{
	byte x;
	byte y;
	byte z;
	byte w;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct AABB
{
	vec3 center;
	vec3 extents;
};

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct pos3
{
	vec3	local;
	int3	global;
};

typedef pos3 Position;

$LayoutStructCompatible()
$LayoutAttribute(Assign = Manual; DisablePack)
struct quat
{
	float x;
	float y;
	float z;
	float w;
};


