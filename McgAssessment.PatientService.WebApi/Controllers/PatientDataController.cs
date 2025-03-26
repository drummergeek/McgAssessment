using McgAssessment.Data;
using McgAssessment.UserService;
using Microsoft.AspNetCore.Mvc;

namespace McgAssessment.PatientService.WebApi.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientDataController : ControllerBase
{
    private readonly IUserTokenService _userTokenService;
    private readonly IPatientService _patientService;

    public PatientDataController(IUserTokenService userTokenService, IPatientService patientService)
    {
        _userTokenService = userTokenService;
        _patientService = patientService;
    }

    private bool ValidateRequestHasPermissions(UserPermissions userPermissions, out string? failureReason)
    {
        // This should be handled by a proper authorization middleware, only doing it this way to simplify the
        // prototype implementation
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            failureReason = "Invalid Authorization Token!";
            return false;
        }

        if (!_userTokenService.TryValidateUserToken(authorizationHeader.ToString(), out User user))
        {
            failureReason = "Invalid User!";
            return false;
        }

        if ((user.Permissions & userPermissions) != userPermissions)
        {
            failureReason = "Not authorized to perform this action!";
            return false;
        }
        
        failureReason = null;
        return true;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!ValidateRequestHasPermissions(UserPermissions.ViewPatientGeneral, out var failureReason))
        {
            return Unauthorized(failureReason);
        }
        
        var response = await _patientService.GetPatientByIdAsync(id, cancellationToken);

        if (!response.Success)
        {
            return Problem(response.ErrorMessage);
        }

        return Ok(response.Result);
    }

    [HttpGet("byName")]
    public async Task<IActionResult> SearchByNameAsync(string lastName, string? firstName = null, bool includePartial = true,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateRequestHasPermissions(UserPermissions.ViewPatientGeneral, out var failureReason))
        {
            return Unauthorized(failureReason);
        }
        
        var response = await _patientService.GetPatientsByNameAsync(lastName, firstName, includePartial, cancellationToken);

        if (!response.Success)
        {
            return Problem(response.ErrorMessage);
        }

        return Ok(response.Result);
    }

    [HttpGet("byCondition")]
    public async Task<IActionResult> SearchByMedicalConditionAsync(string condition,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateRequestHasPermissions(UserPermissions.ViewPatientGeneral, out var failureReason))
        {
            return Unauthorized(failureReason);
        }
        
        var response = await _patientService.GetPatientsByMedicalConditionAsync(condition, cancellationToken);

        if (!response.Success)
        {
            return Problem(response.ErrorMessage);
        }

        return Ok(response.Result);
    }
    
    [HttpGet("byDocumentType")]
    public async Task<IActionResult> SearchByDocumentTypeAsync(PatientDocumentType type,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateRequestHasPermissions(UserPermissions.ViewPatientGeneral, out var failureReason))
        {
            return Unauthorized(failureReason);
        }
        
        var response = await _patientService.GetPatientsByDocumentTypeAsync(type, cancellationToken);

        if (!response.Success)
        {
            return Problem(response.ErrorMessage);
        }

        return Ok(response.Result);
    }

    [HttpPost("update/{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Patient patient,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateRequestHasPermissions(UserPermissions.ModifyPatientGeneral, out var failureReason))
        {
            return Unauthorized(failureReason);
        }

        patient.Id = id;
        var response = await _patientService.UpdatePatientAsync(patient, cancellationToken);

        if (!response.Success)
        {
            return Problem(response.ErrorMessage);
        }

        return Ok();        
    }
}
