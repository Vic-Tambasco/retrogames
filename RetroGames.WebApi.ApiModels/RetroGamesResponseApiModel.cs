namespace RetroGames.WebApi.ApiModels;

public class RetroGamesResponseApiModel
{
    public int ResponseCode { get; set; }
    public string? ErrorMessage { get; set; }

    public RetroGamesResponseApiModel(int responseCode, string? errorMessage = null)
    {
        ResponseCode = responseCode;
        ErrorMessage = errorMessage;
    }
}

public class RetroGamesResponseApiModel<T> : RetroGamesResponseApiModel
{
    public T? Data { get; set; }

    public RetroGamesResponseApiModel(T data) : base(ResponseCodes.SUCESSO.Code)
    {
        Data = data;
    }
    
    public RetroGamesResponseApiModel(int responseCode, string errorMessage) : base(responseCode, errorMessage)
    {
    }
}