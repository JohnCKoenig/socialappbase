namespace MobileAppAPI.ControllerModels.GeneralResponses
{
    public class GeneralResponseModel
    {
        public ResponseCode Code { get; set; }
        public string? CodeDescription { get; set; }
        public DateTime time = DateTime.Now;

        public GeneralResponseModel(ResponseCode code, string codeDescription)
        {
            Code = code;
            CodeDescription = codeDescription;
        }
        public GeneralResponseModel(ResponseCode code)
        {
            Code = code;
        }
    }
}
