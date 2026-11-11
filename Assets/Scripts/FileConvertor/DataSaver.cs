using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
public class DataSaver : MonoBehaviour
{
    private static readonly string encryptionKey = "N8q7Vf3Xz1KjP9LsD0BaT6MwC4RhU2eQ";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static T LoadJson<T>(string name, string path = "jsonSaves/")
    {
        string json = File.ReadAllText(path + name + ".json");

        return JsonUtility.FromJson<T>(json);

    }
    public static T LoadJsonCompressed<T>(string name, string path = "jsonSaves/")
    {
        string compressedPath = Path.Combine(path, name + ".json.gz");

        if (!File.Exists(compressedPath))
        {
            Debug.LogWarning("Compressed file not found: " + compressedPath);
            return default;
        }

        try
        {
            byte[] compressedBytes = File.ReadAllBytes(compressedPath);

            using (var ms = new MemoryStream(compressedBytes))
            using (var gzip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip, Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                return JsonUtility.FromJson<T>(json);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load compressed JSON: " + ex.Message);
            return default;
        }
    }


    public static void SaveJson(object jsonData, string name, bool cleanCode = true, string path = "jsonSaves/", bool overrideIt = false)
    {
        string json = JsonUtility.ToJson(jsonData, cleanCode);
        if (File.Exists(path + name + ".json"))
        {
            if (!overrideIt)
            {
                Debug.LogWarning("there is already file called: " + name);
                return;
            }
            else
            {
                File.Delete(path + name + ".json");
            }
        }


        Directory.CreateDirectory(path);
        File.WriteAllText(path + name + ".json", json);

    }
    public static void SaveJsonCompressed(object jsonData, string name, bool cleanCode = true, string path = "jsonSaves/", bool overrideIt = false)
    {
        Directory.CreateDirectory(path); // make sure folder exists

        string json = JsonUtility.ToJson(jsonData, cleanCode);

        string compressedPath = Path.Combine(path, name + ".json.gz");
        string normalPath = Path.Combine(path, name + ".json");

        // Optionally delete existing files
        if (File.Exists(compressedPath) && !overrideIt)
        {
            Debug.LogWarning("Compressed file already exists: " + compressedPath);
            return;
        }

        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        using (var ms = new MemoryStream())
        {
            using (var gzip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionLevel.Fastest))
            {
                gzip.Write(jsonBytes, 0, jsonBytes.Length);
            }
            File.WriteAllBytes(compressedPath, ms.ToArray());
        }

        // Optional: save normal JSON too
        File.WriteAllText(normalPath, json);

        Debug.Log("Compressed JSON saved to: " + compressedPath);
    }

    public static void SaveJsonEncrypted(object jsonData, string name, string path = "jsonSaves/", bool overrideIt = false)
    {
        string json = JsonUtility.ToJson(jsonData, true);

        byte[] encryptedBytes = EncryptStringToBytes_Aes(json, encryptionKey);

        Directory.CreateDirectory(path);

        string fullPath = Path.Combine(path, name + ".dat");

        if (File.Exists(fullPath))
        {
            if (!overrideIt)
            {
                Debug.LogWarning("Encrypted file already exists: " + fullPath);
                return;
            }
            else
            {
                File.Delete(fullPath);
            }
        }

        File.WriteAllBytes(fullPath, encryptedBytes);

        Debug.Log("Encrypted save file written to: " + fullPath);
    }

    public static T LoadJsonEncrypted<T>(string name, string path = "jsonSaves/")
    {
        string fullPath = Path.Combine(path, name + ".dat");

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Encrypted file not found: " + fullPath);
            return default;
        }

        try
        {
            byte[] encryptedBytes = File.ReadAllBytes(fullPath);
            string json = DecryptStringFromBytes_Aes(encryptedBytes, encryptionKey);
            return JsonUtility.FromJson<T>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load encrypted JSON: " + ex.Message);
            return default;
        }
    }

    // ------------------- AES HELPER METHODS -------------------

    private static byte[] EncryptStringToBytes_Aes(string plainText, string keyString)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(keyString.PadRight(32));
        byte[] iv = new byte[16]; // Initialization vector = 0

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (StreamWriter sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            return ms.ToArray();
            }
        }
    }

    private static string DecryptStringFromBytes_Aes(byte[] cipherText, string keyString)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(keyString.PadRight(32));
        byte[] iv = new byte[16];

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(cipherText))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }


    public static bool JsonFileExist(string name, string path = "jsonSaves/")
    {
        return File.Exists(path + name + ".json");
    }
}
