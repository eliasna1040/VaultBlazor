namespace Services;

public interface IEncryptFileService
{
    Task<MemoryStream> EncryptFile(string? password, byte[] fileInBytes);
    Task<MemoryStream> DecryptFile(string? password, byte[] fileInBytes);
}