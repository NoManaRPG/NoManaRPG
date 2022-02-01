// This file is part of WafclastRPG project.

namespace WafclastRPG.Database.Response
{
    public class StringResponse : IResponse
    {
        public string Response { get; }

        public StringResponse(string response)
        {
            this.Response = response;
        }
    }
}
