using Framework.Core.Data;
using Framework.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
        _context = context;
    }
    
    
    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
        return "secret stuff";
    }

    [HttpGet("notfound")]
    public ActionResult<string> GetNotFoundRequest()
    {
        var thing = _context.Users!.Find(42L);
    
        if (thing == null)
            return NotFound(new ApiResponse(404));

        return Ok();
    }

    [HttpGet("servererror")]
    public ActionResult<string> GetServerError()
    {
        var thing = _context.Users!.Find(42L);
    
        var thingRoReturn = thing!.ToString();

        return Ok();
    }

    [HttpGet("badrequest")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult<string> GetBadRequest(long id)
    {
        return Ok(new ApiResponse(400));
    }
}