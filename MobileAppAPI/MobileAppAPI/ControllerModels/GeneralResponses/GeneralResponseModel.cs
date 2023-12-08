namespace MobileAppAPI.ControllerModels.GeneralResponses
{
    public class GeneralResponseModel<T>
    {
        public ResponseCode Code { get; set; }
        public string? CodeDescription { get; set; }
        public DateTime time = DateTime.Now;
        public T ?Data { get; set; }

        public GeneralResponseModel(ResponseCode code, string codeDescription,T data)
        {
            Code = code;
            CodeDescription = codeDescription;
            Data = data;
        }
        public GeneralResponseModel(ResponseCode code, string codeDescription)
        {
            Code = code;
            CodeDescription = codeDescription;
        }
        public GeneralResponseModel(ResponseCode code)
        {
            Code = code;
        }
        public GeneralResponseModel(T data)
        {
            Data = data;
        }
        public static GeneralResponseModel<T> SuccessResponse(T data) =>
       new GeneralResponseModel<T>(ResponseCode.Success, string.Empty, data);
        public static GeneralResponseModel<T> SuccessResponse() =>
      new GeneralResponseModel<T>(ResponseCode.Success, string.Empty);

        // Static method for error response
        public static GeneralResponseModel<T> ErrorResponse(string message) =>
            new GeneralResponseModel<T>(ResponseCode.Failure, message, default(T));
        public static GeneralResponseModel<T> ErrorResponse(ResponseCode code,string message) =>
           new GeneralResponseModel<T>(code, message, default(T));
    }
}
