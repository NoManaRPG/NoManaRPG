// This file is part of the WafclastRPG project.

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
