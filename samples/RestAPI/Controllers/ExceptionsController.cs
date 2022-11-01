namespace RestAPI.Controllers;

using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestAPI.CustomExceptions;
using System;
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

    [HttpGet("aggregate-exception")]
    public IActionResult GetAggregateException()
    {
        _logger.LogInformation("Throwing AggregateException");

        throw new AggregateException(new CustomBaseException(), new TimeoutException());
    }

    [HttpGet("already-exists")]
    public IActionResult GetAlreadyExists()
    {
        _logger.LogInformation("Throwing AlreadyExistsException");

        ThrowIf.ConditionFails(false, new AlreadyExistsException());
        return Ok();
    }

    [HttpGet("custom-base-exception")]
    public IActionResult GetCustomBaseException()
    {
        _logger.LogInformation("Throwing CustomBaseException");

        throw new CustomBaseException();
    }

    [HttpGet("enhance-your-calm")]
    public IActionResult GetEnhanceYourCalm()
    {
        _logger.LogInformation("Throwing EnhanceYourCalmException");

        throw new EnhanceYourCalmException();
    }

    [HttpGet("external-component")]
    public IActionResult GetExternalException()
    {
        _logger.LogInformation("Throwing ExternalComponentException");

        ThrowIf.ConditionFails(false, new ExternalComponentException("component name", "component description"));
        return Ok();
    }

    [HttpGet("inner-exception")]
    public IActionResult GetInnerException()
    {
        _logger.LogInformation("Throwing exception with inner exception");

        throw new CustomBaseException(new UnauthorizedAccessException());
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        _logger.LogInformation("Throwing NotFoundException");

        ThrowIf.ArgumentNullThrowNotFound<string>(null, "argument identifier");
        return Ok();
    }

    [HttpGet("tea-pot")]
    public IActionResult GetTeaPot()
    {
        _logger.LogInformation("Throwing TeaPotException");

        throw new TeaPotException();
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
