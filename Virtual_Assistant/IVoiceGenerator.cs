using System.IO;

namespace Virtual_Assistant;

public interface IVoiceGenerator
{
    Task<IEnumerable<byte>?> GenerateVoiceAsync(string text, string voice);
}