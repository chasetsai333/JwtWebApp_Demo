namespace JwtWebApp.Models
{
    public class JwtSettingsOptions
    {
        /// <summary>
        /// 發行人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 加密的key，拿來與jwt-token比對
        /// </summary>
        public string SignKey { get; set; }
    }
}
