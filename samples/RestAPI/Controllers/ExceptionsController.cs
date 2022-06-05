namespace RestAPI.Controllers;

using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ToolPack.Exceptions.Base.Entities;
using ToolPack.Exceptions.Base.Guard;

[ApiController]
[Route("exceptions")]
public class ExceptionsController : ControllerBase
{
    private readonly ILogger<ExceptionsController> _logger;

    public ExceptionsController(ILogger<ExceptionsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("already-exists")]
    public IActionResult GetAlreadyExists()
    {
        _logger.LogInformation("Throwing AlreadyExistsException");

        ThrowWhen.ConditionFails(false, new AlreadyExistsException());
        return Ok();
    }

    [HttpGet("custom-base-exception")]
    public IActionResult GetCustomBaseException()
    {
        _logger.LogInformation("Throwing CustomBaseException");

        throw new CustomBaseException();
    }

    [HttpGet("external-component")]
    public IActionResult GetExternalException()
    {
        _logger.LogInformation("Throwing ExternalComponentException");

        ThrowWhen.ConditionFails(false, new ExternalComponentException("component name", "component description"));
        return Ok();
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        _logger.LogInformation("Throwing NotFoundException");

        ThrowWhen.ArgumentNullThrowNotFound<string>(null, "argument description", "argument identifier");
        return Ok();
    }

    [HttpGet("validation-failed-exception")]
    public IActionResult GetValidationFailedException()
    {
        _logger.LogInformation("Throwing ValidationFailedException");

        throw new ValidationFailedException(
            new List<ValidationFailure>()
            {
                new("Prop1 Name", "Prop1 error message"),
                new("Prop2 Name", "Prop2 error message")
            });
    }
}
