using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Reflection.Cache.Properties;
using Soenneker.Reflection.Cache.Types;

namespace Soenneker.Reflection.Cache.Tests.Properties;

public class PropertyAccessorTests
{
    private readonly ReflectionCache _cache;

    public PropertyAccessorTests()
    {
        _cache = new ReflectionCache();
    }

    [Test]
    public void String_property_getter_and_setter_should_use_cached_delegates()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Name));
        var target = new AccessorTarget();

        property.CanGetValue.Should().BeTrue();
        property.CanSetValue.Should().BeTrue();

        property.TrySetValue(target, "Jane").Should().BeTrue();
        property.TryGetValue(target, out object? value).Should().BeTrue();
        value.Should().Be("Jane");

        property.GetValue(target).Should().Be("Jane");
    }

    [Test]
    public void Int_property_getter_and_setter_should_use_cached_delegates()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Age));
        var target = new AccessorTarget();

        property.SetValue(target, 42);

        property.GetValue(target).Should().Be(42);
        target.Age.Should().Be(42);
    }

    [Test]
    public void Nullable_int_property_setter_should_accept_null_and_value()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Score));
        var target = new AccessorTarget();

        property.TrySetValue(target, 7).Should().BeTrue();
        target.Score.Should().Be(7);

        property.TrySetValue(target, null).Should().BeTrue();
        target.Score.Should().BeNull();
    }

    [Test]
    public void Enum_property_setter_should_accept_enum_value_without_string_conversion()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Status));
        var target = new AccessorTarget();

        property.TrySetValue(target, AccessorStatus.Active).Should().BeTrue();
        target.Status.Should().Be(AccessorStatus.Active);

        property.TrySetValue(target, "Active").Should().BeFalse();
    }

    [Test]
    public void Read_only_property_should_not_have_setter()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.ReadOnly));
        var target = new AccessorTarget();

        property.GetSetter().Should().BeNull();
        property.CanSetValue.Should().BeFalse();
        property.TrySetValue(target, "changed").Should().BeFalse();
        Action act = () => property.SetValue(target, "changed");
        act.Should().Throw<NotSupportedException>();
    }

    [Test]
    public void Write_only_property_should_not_have_getter()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.WriteOnly));
        var target = new AccessorTarget();

        property.GetGetter().Should().BeNull();
        property.CanGetValue.Should().BeFalse();
        property.TryGetValue(target, out object? value).Should().BeFalse();
        value.Should().BeNull();

        property.TrySetValue(target, "secret").Should().BeTrue();
        target.WriteOnlyValue.Should().Be("secret");
    }

    [Test]
    public void Indexer_property_should_be_unsupported()
    {
        CachedProperty property = GetProperty("Item");
        var target = new AccessorTarget();

        property.IsIndexer.Should().BeTrue();
        property.GetGetter().Should().BeNull();
        property.GetSetter().Should().BeNull();
        property.TryGetValue(target, out _).Should().BeFalse();
        property.TrySetValue(target, "value").Should().BeFalse();
    }

    [Test]
    public void Wrong_value_type_should_throw_from_SetValue_and_return_false_from_TrySetValue()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Age));
        var target = new AccessorTarget();

        property.TrySetValue(target, "42").Should().BeFalse();

        Action act = () => property.SetValue(target, "42");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Repeated_GetSetter_calls_should_return_same_delegate()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Name));

        Action<object, object?>? setter1 = property.GetSetter();
        Action<object, object?>? setter2 = property.GetSetter();

        setter1.Should().NotBeNull();
        ReferenceEquals(setter1, setter2).Should().BeTrue();
    }

    [Test]
    public void Repeated_GetGetter_calls_should_return_same_delegate()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Name));

        Func<object, object?>? getter1 = property.GetGetter();
        Func<object, object?>? getter2 = property.GetGetter();

        getter1.Should().NotBeNull();
        ReferenceEquals(getter1, getter2).Should().BeTrue();
    }

    [Test]
    public void Thread_safe_accessor_initialization_should_not_throw()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Name));
        var getters = new Func<object, object?>?[128];
        var setters = new Action<object, object?>?[128];

        Parallel.For(0, getters.Length, i =>
        {
            var target = new AccessorTarget();
            getters[i] = property.GetGetter();
            setters[i] = property.GetSetter();
            property.TrySetValue(target, "Jane").Should().BeTrue();
            property.TryGetValue(target, out object? value).Should().BeTrue();
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
        CachedProperty property = GetProperty(nameof(AccessorTarget.Name));

        property.PropertyInfo.Name.Should().Be(nameof(AccessorTarget.Name));
        property.IsPublic.Should().BeTrue();
        property.IsStatic.Should().BeFalse();
        property.IsIndexer.Should().BeFalse();
        property.IsReadOnly.Should().BeFalse();
    }

    [Test]
    public void Static_property_should_be_unsupported_for_object_accessors()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.StaticName));

        property.IsStatic.Should().BeTrue();
        property.GetGetter().Should().BeNull();
        property.GetSetter().Should().BeNull();
    }

    [Test]
    public void Private_setter_should_not_be_treated_as_public_setter()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.PrivateSet));
        var target = new AccessorTarget();

        property.GetGetter().Should().NotBeNull();
        property.GetSetter().Should().BeNull();
        property.TrySetValue(target, "changed").Should().BeFalse();
    }

    [Test]
    public void Init_only_setter_should_not_be_treated_as_normal_setter()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.InitOnly));
        var target = new AccessorTarget();

        property.GetGetter().Should().NotBeNull();
        property.GetSetter().Should().BeNull();
        property.TrySetValue(target, "changed").Should().BeFalse();
    }

    [Test]
    public void Value_type_declaring_type_should_support_getter_but_not_setter()
    {
        CachedType cachedType = _cache.GetCachedType(typeof(AccessorStruct));
        CachedProperty property = cachedType.GetCachedProperty(nameof(AccessorStruct.Number))!;
        object target = new AccessorStruct { Number = 12 };

        property.TryGetValue(target, out object? value).Should().BeTrue();
        value.Should().Be(12);

        property.GetSetter().Should().BeNull();
        property.TrySetValue(target, 20).Should().BeFalse();
        Action act = () => property.SetValue(target, 20);
        act.Should().Throw<NotSupportedException>();
    }

    [Test]
    public void Null_or_wrong_instance_should_fail_Try_methods()
    {
        CachedProperty property = GetProperty(nameof(AccessorTarget.Name));

        property.TryGetValue(null!, out _).Should().BeFalse();
        property.TrySetValue(null!, "Jane").Should().BeFalse();
        property.TryGetValue(new object(), out _).Should().BeFalse();
        property.TrySetValue(new object(), "Jane").Should().BeFalse();

        Action getAct = () => property.GetValue(null!);
        getAct.Should().Throw<ArgumentNullException>();

        Action setAct = () => property.SetValue(new object(), "Jane");
        setAct.Should().Throw<ArgumentException>();
    }

    private CachedProperty GetProperty(string name)
    {
        CachedType cachedType = _cache.GetCachedType(typeof(AccessorTarget));
        return cachedType.GetCachedProperty(name)!;
    }

    private sealed class AccessorTarget
    {
        private string? _writeOnly;

        public string? Name { get; set; }

        public int Age { get; set; }

        public int? Score { get; set; }

        public AccessorStatus Status { get; set; }

        public string ReadOnly => "read";

        public string? WriteOnly
        {
            set => _writeOnly = value;
        }

        public string? WriteOnlyValue => _writeOnly;

        public string? PrivateSet { get; private set; }

        public string? InitOnly { get; init; }

        public static string? StaticName { get; set; }

        public string this[int index]
        {
            get => index.ToString();
            set => _writeOnly = value;
        }
    }

    private struct AccessorStruct
    {
        public int Number { get; set; }
    }

    private enum AccessorStatus
    {
        Unknown,
        Active
    }
}
