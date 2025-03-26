namespace McgAssessment.PatientService;

public class PatientServiceResponse<T> : PatientServiceResponse
{
    public PatientServiceResponse(T result)
    {
        Result = result;
    }

    public PatientServiceResponse() { }

    public PatientServiceResponse(string errorMessage) : base(errorMessage) { }

    public T? Result { get; set; }
}

public class PatientServiceResponse
{
    public PatientServiceResponse()
    {
        Success = true;
    }
    
    public PatientServiceResponse(string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
    }

    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}