using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text.Json;

namespace LCMRCommon
{

    /// <summary>
    /// ConfigurationCommon class
    /// </summary>
    /// <remarks>
    ///創建人員:JC
    ///串建日期:20250410
    ///更新日期:20250410
    ///備註:
    ///configurationCommon
    ///加解密功能
    ///</remarks>

    public static class ConfigurationCommon
    {
        public static string EncryptedConnectionString(this IConfigurationBuilder bulider, string filePath)
        {
            var config = bulider.Build();
            // 讀取 AES Key 和 IV PS.未來應將 AES Key 和 IV 存在環境變數
            var key = config["AES:Key"]?? "bA3Y1fx9QhV+W9yBfLz2C0fFvU4ncnZ6GVovryUtNHI=";
            var iv = config["AES:IV"]?? "AxY8DCtDaUoJGp+zDQhuwQ==";

            var encryptedData = config["encryptedData"];
            // 判斷是否加密
            if (!string.IsNullOrEmpty(encryptedData))
            {
                //解密 ConnectionStrings
                var decryptedJson = AesHelper.Decrypt(encryptedData, key, iv);
                var connectionSection = JsonSerializer.Deserialize<JsonElement>(decryptedJson);
                var connStr = connectionSection.GetProperty("DefaultConnection").GetString();

                return connStr;
            }
            else
            {
                // 若未加密，加密ConnectionStrings重建檔案確保ConnectionStrings加密
                var connectionSection = config.GetSection("ConnectionStrings");
                var connectionJson = JsonSerializer.Serialize(connectionSection.GetChildren().ToDictionary(x => x.Key, x => x.Value));

                var encrypted = AesHelper.Encrypt(connectionJson, key, iv);

                // 讀取原始 appsettings.json
                var json = File.ReadAllText(filePath);

                using var doc = JsonDocument.Parse(json);
                var jsonObj = doc.RootElement.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);

                // 移除 ConnectionStrings，新增 encryptedData
                jsonObj.Remove("ConnectionStrings");
                jsonObj["encryptedData"] = JsonDocument.Parse($"\"{encrypted}\"").RootElement;

                // 序列化回 JSON
                var options = new JsonSerializerOptions { WriteIndented = true };
                var updatedJson = JsonSerializer.Serialize(jsonObj, options);

                File.WriteAllText(filePath, updatedJson);

                return config.GetConnectionString("DefaultConnection");
            }
        }

        public static bool IsBase64String(string s)
        {
            Span<byte> buffer = new Span<byte>(new byte[s.Length]);
            return Convert.TryFromBase64String(s, buffer, out _);
        }

        private static void chkAesKey(this IConfigurationBuilder bulider)
        {

        }
    }
}
