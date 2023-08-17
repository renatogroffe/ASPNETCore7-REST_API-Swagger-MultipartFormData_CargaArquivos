using Microsoft.AspNetCore.Mvc;
using APICargaArquivos.Models;

namespace APICargaArquivos.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;

    public FilesController(ILogger<FilesController> logger)
    {
        _logger = logger;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFormData([FromForm]FormData data)
    {
        var filesDirectory = Path.Combine(
            new DirectoryInfo(Environment.CurrentDirectory).Parent!.FullName, "Files");
        if (!Directory.Exists(filesDirectory))
            Directory.CreateDirectory(filesDirectory);
        _logger.LogInformation($"Diretório em que os arquivos serão gravados: {filesDirectory}");
        _logger.LogInformation($"Usuário: {data.User}");
        _logger.LogInformation($"Observação: {data.Note}");
        if (data.Files != null && data.Files.Count > 0)
        {
            _logger.LogInformation($"Quantidade de arquivos enviados: {data.Files.Count}");
            foreach (var file in data.Files)
            {
                _logger.LogInformation($"Arquivo: {file.FileName} - Content-Type = {file.ContentType}");
                var newFile = Path.Combine(filesDirectory,
                    $"{Guid.NewGuid()}{new FileInfo(file.FileName).Extension}");
                using var stream = file.OpenReadStream();
                using var fileContentStream = new MemoryStream();
                await stream.CopyToAsync(fileContentStream);
                await System.IO.File.WriteAllBytesAsync(newFile, fileContentStream.ToArray());
                if (System.IO.File.Exists(newFile))
                    _logger.LogInformation($"Novo arquivo: {newFile} | Nome original: {file.FileName}");
            }
            _logger.LogInformation("Concluído o processamento dos arquivos!");
        }
        return Ok();
    }
}