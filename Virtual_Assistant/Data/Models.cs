using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Virtual_Assistant.Data;

public class Models
{
    private readonly ILogger<Models> _logger;

    public Models(ILogger<Models> logger)
    {
        _logger = logger;
    }
    
}