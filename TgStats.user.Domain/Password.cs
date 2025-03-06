using System.Diagnostics.CodeAnalysis;

namespace TgStats.user.Domain;

public readonly struct Password : IComparable, IComparable<Password>, IEquatable<Password>, IFormattable, IParsable<Password>, ISpanFormattable, ISpanParsable<Password>, IUtf8SpanFormattable
{
    public string Value { get; }
    public Password(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Password cannot be empty");
        }

        if(value.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long");
        }

        Value = value;
    }

    public static bool TryCreate(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out Password result)
    {
        result = default;
        if(s.IsEmpty || s.IsWhiteSpace() || s.Length < 8)
        {
            return false;
        }

        result = new Password(s.ToString());
        return true;
    }

    public override bool Equals(object? obj) 
        => obj is Password password && Equals(password);

    public bool Equals(Password other) => Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    
    public override int GetHashCode() => HashCode.Combine(Value);

    public static Password Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Password result)
        => TryParse(s.AsSpan(),provider,out result);

    public int CompareTo(object? obj) 
        => obj is Password password
            ? CompareTo(password)
            : throw new ArgumentException("Object is not a Password");

    public int CompareTo(Password other) 
        => string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public string ToString(string? format, IFormatProvider? formatProvider) 
        => Value.ToString(formatProvider);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if(Value.AsSpan().TryCopyTo(destination))
        {
            charsWritten = Value.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static Password Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(s.ToString());

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Password result) 
        => TryCreate(s, out result);

    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(Value);
        if(utf8Bytes.AsSpan().TryCopyTo(utf8Destination))
        {
            bytesWritten = utf8Bytes.Length;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    public static bool operator ==(Password left, Password right) => left.Equals(right);
    public static bool operator !=(Password left, Password right) => !( left  ==  right );

    public override string ToString() => Value;
}