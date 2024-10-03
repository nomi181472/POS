using Grpc.Core;

namespace BS.ExternalServices
{
    public static class ApiConfigs
    {
        public static class StoreApiConfig
        {
            public static string Name { get; set; } = "Store";
            public static int Port { get; set; } = 6082;
            public static string Host { get; set; } = "store";

        }
        public static class HubApiConfig
        {
            public static string Name { get; set; } = "Hub";
            public static int Port { get; set; } = 9082;
            public static string Host { get; set; } = "hub";

        }

    }

}
