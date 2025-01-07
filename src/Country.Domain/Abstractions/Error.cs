namespace Country.Domain.Abstractions
{
    public record Error (string Code, string Message)
    {
        public static Error None = new(string.Empty, string.Empty);
    }
}
