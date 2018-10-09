using Microsoft.WindowsAzure.Storage.Table;

namespace TripitSyncFunctions.TableServices
{
    public class TokenEntity : TableEntity
    {
        public TokenEntity(string tenantId, string objectid)
        {
            PartitionKey = tenantId;
            RowKey = objectid;
        }
        public TokenEntity()
        {

        }
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
        public string ADObjectId { get; set; }
        public string ADTenantId { get; set; }
    }
}
