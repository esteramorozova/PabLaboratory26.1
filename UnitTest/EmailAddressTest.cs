using AppCore.ValueObjects;

namespace UnitTest;

public class EmailAddressTest
{
    // ── Parse — poprawne adresy ──────────────────────────

    [Fact]
    public void Parse_ValidEmail_ReturnsEmailAddress()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("jan.kowalski@crm.pl", email.Value);
    }

    [Fact]
    public void Parse_ExtractsUserCorrectly()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("jan.kowalski", email.User);
    }

    [Fact]
    public void Parse_ExtractsDomainCorrectly()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("crm.pl", email.Domain);
    }

    [Fact]
    public void Parse_ExtractsDomainNameCorrectly()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("crm", email.DomainName);
    }

    [Fact]
    public void Parse_ExtractsTopLevelDomainCorrectly()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("pl", email.TopLevelDomain);
    }

    [Fact]
    public void Parse_ConvertsToLowercase()
    {
        var email = EmailAddress.Parse("JAN.KOWALSKI@CRM.PL");
        Assert.Equal("jan.kowalski@crm.pl", email.Value);
    }

    [Fact]
    public void Parse_TrimmsWhitespace()
    {
        var email = EmailAddress.Parse("  jan.kowalski@crm.pl  ");
        Assert.Equal("jan.kowalski@crm.pl", email.Value);
    }

    // ── Parse — niepoprawne adresy ───────────────────────

    [Fact]
    public void Parse_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Parse(""));
    }

    [Fact]
    public void Parse_NullString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Parse(null!));
    }

    [Fact]
    public void Parse_MissingAtSign_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Parse("jankowalski.crm.pl"));
    }

    [Fact]
    public void Parse_MissingDomain_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Parse("jan.kowalski@"));
    }

    [Fact]
    public void Parse_DomainWithoutDot_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Parse("jan.kowalski@crm"));
    }

    // ── TryParse ─────────────────────────────────────────

    [Fact]
    public void TryParse_ValidEmail_ReturnsEmailAddress()
    {
        var email = EmailAddress.TryParse("jan.kowalski@crm.pl");
        Assert.NotNull(email);
    }

    [Fact]
    public void TryParse_InvalidEmail_ReturnsNull()
    {
        var email = EmailAddress.TryParse("niepoprawny-email");
        Assert.Null(email);
    }

    // ── IsFromDomain ─────────────────────────────────────

    [Fact]
    public void IsFromDomain_CorrectDomain_ReturnsTrue()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.True(email.IsFromDomain("crm.pl"));
    }

    [Fact]
    public void IsFromDomain_WrongDomain_ReturnsFalse()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.False(email.IsFromDomain("gmail.com"));
    }

    [Fact]
    public void IsFromDomain_CaseInsensitive_ReturnsTrue()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.True(email.IsFromDomain("CRM.PL"));
    }

    // ── ToDisplayFormat ───────────────────────────────────

    [Fact]
    public void ToDisplayFormat_CapitalizesFirstLetter()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("Jan.kowalski@crm.pl", email.ToDisplayFormat());
    }

    // ── Równość ───────────────────────────────────────────

    [Fact]
    public void Equals_SameEmail_ReturnsTrue()
    {
        var email1 = EmailAddress.Parse("jan.kowalski@crm.pl");
        var email2 = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal(email1, email2);
    }

    [Fact]
    public void Equals_DifferentEmail_ReturnsFalse()
    {
        var email1 = EmailAddress.Parse("jan.kowalski@crm.pl");
        var email2 = EmailAddress.Parse("anna.nowak@crm.pl");
        Assert.NotEqual(email1, email2);
    }

    [Fact]
    public void Equals_SameEmailDifferentCase_ReturnsTrue()
    {
        var email1 = EmailAddress.Parse("JAN.KOWALSKI@CRM.PL");
        var email2 = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal(email1, email2);
    }

    // ── Konwersja ─────────────────────────────────────────

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        string value = email;
        Assert.Equal("jan.kowalski@crm.pl", value);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var email = EmailAddress.Parse("jan.kowalski@crm.pl");
        Assert.Equal("jan.kowalski@crm.pl", email.ToString());
    }
}