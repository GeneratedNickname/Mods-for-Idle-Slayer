using System.Reflection;

namespace AutoJumpMod;

/// <summary>
/// Looks up another mod's component by type name and reads a bool field or property from it.
/// Several candidates can be given (e.g. the repo's local build and the released build of a mod,
/// which live in different assemblies); the first type that exists is used.
/// </summary>
internal class ModFlagChecker
{
    private readonly (string TypeName, string MemberName)[] _candidates;
    private Type _type;
    private FieldInfo _field;
    private PropertyInfo _property;
    private bool _initialized;

    public ModFlagChecker(string qualifiedTypeName, string memberName = "")
        : this((qualifiedTypeName, memberName)) { }

    public ModFlagChecker(params (string TypeName, string MemberName)[] candidates)
    {
        _candidates = candidates;
    }

    private void Initialize()
    {
        if (_initialized) return;
        _initialized = true;

        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (var (typeName, memberName) in _candidates)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Plugin.DLog($"Type not found: {typeName}");
                continue;
            }

            _type = type;
            if (string.IsNullOrEmpty(memberName)) return;

            _field = type.GetField(memberName, flags);
            if (_field == null)
                _property = type.GetProperty(memberName, flags);
            if (_field == null && _property == null)
                Plugin.DLog($"Member '{memberName}' not found on {type.FullName}");
            return;
        }
    }

    private UnityEngine.Object FindInstance()
    {
        Initialize();
        if (_type == null) return null;

        var findMethod = typeof(UnityEngine.Object)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
                m.Name == "FindObjectOfType"
                && m.IsGenericMethodDefinition
                && m.GetParameters().Length == 0
            );
        if (findMethod == null)
        {
            Plugin.DLog("Couldn't find generic FindObjectOfType<>()");
            return null;
        }

        var generic = findMethod.MakeGenericMethod(_type);
        return generic.Invoke(null, []) as UnityEngine.Object;
    }

    public bool IsLoaded() => FindInstance() != null;

    public bool GetBoolFlag() => ReadFlag(FindInstance(), whenNoMember: false);

    /// <summary>Loaded, and the flag is true (a candidate without a member counts as always true).</summary>
    public bool IsActive() => ReadFlag(FindInstance(), whenNoMember: true);

    private bool ReadFlag(UnityEngine.Object inst, bool whenNoMember)
    {
        if (inst == null) return false;
        if (_field == null && _property == null) return whenNoMember;
        var value = _field != null ? _field.GetValue(inst) : _property.GetValue(inst);
        return value is true;
    }
}
