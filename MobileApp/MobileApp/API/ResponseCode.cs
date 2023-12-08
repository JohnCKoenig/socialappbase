namespace MobileApp.API.GeneralResponses
{
    public enum ResponseCode
    {
        /// <summary>
        /// General Codes
        /// </summary>
        Success, //Request was succesful
        Failure, //Request failed

        /// <summary>
        /// Account Codes
        /// </summary>
        UserAlreadyExists, //a user with that email already exists
        InvalidCredentials, //no user exists with those credentials

        ///<summary>
        ///Chat Codes
        ///</summary>
        RecipientDoesNotExist,
        ReflectiveChat,
    }
}
