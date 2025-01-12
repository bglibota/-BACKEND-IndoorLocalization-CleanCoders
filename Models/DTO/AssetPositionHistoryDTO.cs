namespace IndoorLocalization_API.Models.DTO
{
    public class AssetPositionHistoryDTO:AssetPositionHistory
    {
        public AssetPositionHistoryDTO()
        {
        }
        public string AssetName { get; set; }
        public string FloorMapName { get; set; }  

    }
}
