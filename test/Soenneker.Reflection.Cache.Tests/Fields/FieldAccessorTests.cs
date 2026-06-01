using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Fields;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Fields;

public class FieldAccessorTests
{
    private readonly ReflectionCache _cache;

    public FieldAccessorTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void String_field_getter_and_setter_should_use_cached_delegates()
    {
        CachedField field = GetField(nameof(AccessorTarget.Name));
        var target = new AccessorTarget();

        field.CanGetValue.Should().BeTrue();
        field.CanSetValue.Should().BeTrue();

        field.TrySetValue(target, "Jane").Should().BeTrue();
        field.TryGetValue(target, out object? value).Should().BeTrue();
        value.Should().Be("Jane");

        field.GetValue(target).Should().Be("Jane");
    }

    [Test]
    public void Int_field_getter_and_setter_should_use_cached_delegates()
    {
        CachedField field = GetField(nameof(AccessorTarget.Age));
        var target = new AccessorTarget();

        field.SetValue(target, 42);

        field.GetValue(target).Should().Be(42);
        target.Age.Should().Be(42);
    }

    [Test]
    public void Nullable_int_field_setter_should_accept_null_and_value()
    {
        CachedField field = GetField(nameof(AccessorTarget.Score));
        var target = new AccessorTarget();

        field.TrySetValue(target, 7).Should().BeTrue();
        target.Score.Should().Be(7);

        field.TrySetValue(target, null).Should().BeTrue();
        target.Score.Should().BeNull();
    }

    [Test]
    public void Enum_field_setter_should_accept_enum_value_without_string_conversion()
    {
        CachedField field = GetField(nameof(AccessorTarget.Status));
        var target = new AccessorTarget();

        field.TrySetValue(target, AccessorStatus.Active).Should().BeTrue();
        target.Status.Should().Be(AccessorStatus.Active);

        field.TrySetValue(target, "Active").Should().BeFalse();
    }

    [Test]
    public void Readonly_field_should_not_have_setter()
    {
        CachedField field = GetField(nameof(AccessorTarget.ReadOnly));
        var target = new AccessorTarget();

        field.GetGetter().Should().NotBeNull();
        field.GetSetter().Should().BeNull();
        field.TryGetValue(target, out object? value).Should().BeTrue();
        value.Should().Be("read");
        field.TrySetValue(target, "changed").Should().BeFalse();

        Action act = () => field.SetValue(target, "changed");
        act.Should().Throw<NotSupportedException>();
    }

    [Test]
    public void Private_field_should_be_unsupported()
    {
        CachedField field = GetField("_private");
        var target = new AccessorTarget();

        field.GetGetter().Should().BeNull();
        field.GetSetter().Should().BeNull();
        field.TryGetValue(target, out _).Should().BeFalse();
        field.TrySetValue(target, "changed").Should().BeFalse();
    }

    [Test]
    public void Static_field_should_be_unsupported_for_object_accessors()
    {
        CachedField field = GetField(nameof(AccessorTarget.StaticName));

        field.GetGetter().Should().BeNull();
        field.GetSetter().Should().BeNull();
    }

    [Test]
    public void Const_field_should_be_unsupported()
    {
        CachedField field = GetField(nameof(AccessorTarget.ConstantName));
        var target = new AccessorTarget();

        field.GetGetter().Should().BeNull();
        field.GetSetter().Should().BeNull();
        field.TryGetValue(target, out _).Should().BeFalse();
        field.TrySetValue(target, "changed").Should().BeFalse();
    }

    [Test]
    public void Wrong_value_type_should_throw_from_SetValue_and_return_false_from_TrySetValue()
    {
        CachedField field = GetField(nameof(AccessorTarget.Age));
        var target = new AccessorTarget();

        field.TrySetValue(target, "42").Should().BeFalse();

        Action act = () => field.SetValue(target, "42");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Repeated_GetSetter_calls_should_return_same_delegate()
    {
        CachedField field = GetField(nameof(AccessorTarget.Name));

        Action<object, object?>? setter1 = field.GetSetter();
        Action<object, object?>? setter2 = field.GetSetter();

        setter1.Should().NotBeNull();
        ReferenceEquals(setter1, setter2).Should().BeTrue();
    }

    [Test]
    public void Repeated_GetGetter_calls_should_return_same_delegate()
    {
        CachedField field = GetField(nameof(AccessorTarget.Name));

        Func<object, object?>? getter1 = field.GetGetter();
        Func<object, object?>? getter2 = field.GetGetter();

        getter1.Should().NotBeNull();
        ReferenceEquals(getter1, getter2).Should().BeTrue();
    }

    [Test]
    public void Thread_safe_accessor_initialization_should_not_throw()
    {
        CachedField field = GetField(nameof(AccessorTarget.Name));
        var getters = new Func<object, object?>?[128];
        var setters = new Action<object, object?>?[128];

        Parallel.For(0, getters.Length, i =>
        {
            var target = new AccessorTarget();
            getters[i] = field.GetGetter();
            setters[i] = field.GetSetter();
            field.TrySetValue(target, "Jane").Should().BeTrue();
            field.TryGetValue(target, out object? value).Should().BeTrue();
            value.Should().Be("Jane");
        });

        for (var i = 1; i < getters.Length; i++)
        {
            ReferenceEquals(getters[0], getters[i]).Should().BeTrue();
            ReferenceEquals(setters[0], setters[i]).Should().BeTrue();
        }
    }

    [Test]
    public void Existing_metadata_should_remain_available()
    {
        CachedField field = GetField(nameof(AccessorTarget.Name));

        field.FieldInfo.Name.Should().Be(nameof(AccessorTarget.Name));
        field.FieldInfo.IsPublic.Should().BeTrue();
        field.FieldInfo.IsStatic.Should().BeFalse();
        field.FieldInfo.IsInitOnly.Should().BeFalse();
    }

    [Test]
    public void Value_type_declaring_type_should_support_getter_but_not_setter()
    {
        CachedType cachedType = _cache.GetCachedType(typeof(AccessorStruct));
        CachedField field = cachedType.GetCachedField(nameof(AccessorStruct.Number))!;
        object target = new AccessorStruct { Number = 12 };

        field.TryGetValue(target, out object? value).Should().BeTrue();
        value.Should().Be(12);

        field.GetSetter().Should().BeNull();
        field.TrySetValue(target, 20).Should().BeFalse();

        Action act = () => field.SetValue(target, 20);
        act.Should().Throw<NotSupportedException>();
    }

    [Test]
    public void Null_or_wrong_instance_should_fail_Try_methods()
    {
        CachedField field = GetField(nameof(AccessorTarget.Name));

        field.TryGetValue(null!, out _).Should().BeFalse();
        field.TrySetValue(null!, "Jane").Should().BeFalse();
        field.TryGetValue(new object(), out _).Should().BeFalse();
        field.TrySetValue(new object(), "Jane").Should().BeFalse();

        Action getAct = () => field.GetValue(null!);
        getAct.Should().Throw<ArgumentNullException>();

        Action setAct = () => field.SetValue(new object(), "Jane");
        setAct.Should().Throw<ArgumentException>();
    }

    private CachedField GetField(string name)
    {
        CachedType cachedType = _cache.GetCachedType(typeof(AccessorTarget));
        return cachedType.GetCachedField(name)!;
    }

    private sealed class AccessorTarget
    {
        private string? _private = null;

        public string? Name = null;

        public int Age = 0;

        public int? Score = null;

        public AccessorStatus Status = AccessorStatus.Unknown;

        public readonly string ReadOnly = "read";

        public static string? StaticName = null;

        public const string ConstantName = "constant";

        public string? PrivateValue => _private;
    }

    private struct AccessorStruct
    {
        public int Number;
    }

    private enum AccessorStatus
    {
        Unknown,
        Active
    }
}
