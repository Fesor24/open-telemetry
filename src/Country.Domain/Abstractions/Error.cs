namespace Country.Domain.Abstractions
{
    public record Error (string Code, string Message)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static BadRequest BadRequest(string Code, string Message) => new(Code, Message);
    }

    public record BadRequest(string Code, string Message) : Error(Code, Message);
    public record NotFound(string Code, string Message) : Error(Code, Message);
}
