namespace AppCore.ValueObjects;

public sealed class EmailAddress : IEquatable<EmailAddress>
{
    public string Value { get; }
    public string User { get; }       // część przed @
    public string Domain { get; }     // część po @

    private EmailAddress(string value, string user, string domain)
    {
        Value = value;
        User = user;
        Domain = domain;
    }

    /// <summary>Tworzy EmailAddress z ciągu znaków. Rzuca wyjątek jeśli niepoprawny.</summary>
    public static EmailAddress Parse(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Adres email nie może być pusty.");

        email = email.Trim().ToLowerInvariant();

        var atIndex = email.IndexOf('@');

        if (atIndex <= 0)
            throw new ArgumentException($"Brak znaku '@' w adresie: {email}");

        if (atIndex == email.Length - 1)
            throw new ArgumentException($"Brak domeny w adresie: {email}");

        var user = email[..atIndex];
        var domain = email[(atIndex + 1)..];

        if (!domain.Contains('.'))
            throw new ArgumentException($"Niepoprawna domena: {domain}");

        return new EmailAddress(email, user, domain);
    }

    /// <summary>Próbuje sparsować email — zwraca null jeśli niepoprawny.</summary>
    public static EmailAddress? TryParse(string email)
    {
        try { return Parse(email); }
        catch { return null; }
    }

    /// <summary>Zwraca domenę główną np. 'gmail.com' → 'gmail'</summary>
    public string DomainName => Domain.Split('.')[0];

    /// <summary>Zwraca rozszerzenie domeny np. 'gmail.com' → 'com'</summary>
    public string TopLevelDomain => Domain.Split('.')[^1];

    /// <summary>Sprawdza czy email należy do podanej domeny.</summary>
    public bool IsFromDomain(string domain) =>
        Domain.Equals(domain.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase);

    /// <summary>Formatuje email z wielką literą użytkownika np. 'Jan@gmail.com'</summary>
    public string ToDisplayFormat() =>
        $"{char.ToUpper(User[0])}{User[1..]}@{Domain}";

    // Równość — dwa EmailAddress są równe jeśli mają tę samą wartość
    public bool Equals(EmailAddress? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is EmailAddress other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static bool operator ==(EmailAddress? left, EmailAddress? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(EmailAddress? left, EmailAddress? right) =>
        !(left == right);

    // Niejawna konwersja string → EmailAddress
    public static implicit operator string(EmailAddress email) => email.Value;
}