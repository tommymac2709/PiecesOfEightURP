using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace GameDevTV.Saving
{
    [CreateAssetMenu(menuName = "SavingStrategies/XorText", fileName ="XorTextStrategy")]
    public class XorTextStrategy : SavingStrategy
    {
        [Header("Use Context Menu to generate a random key.")]
        [SerializeField] string key = "TheQuickBrownFoxJumpedOverTheLazyDog"; //This is a TERRIBLE KEY
       
        
        public string EncryptDecrypt(string szPlainText, string szEncryptionKey)  
        {  
            StringBuilder szInputStringBuild = new StringBuilder(szPlainText);  
            StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);  
            char Textch;  
            for (int iCount = 0; iCount < szPlainText.Length; iCount++)
            {
                int stringIndex = iCount % szEncryptionKey.Length;
                Textch = szInputStringBuild[iCount];  
                Textch = (char)(Textch ^ szEncryptionKey[stringIndex]);  
                szOutStringBuild.Append(Textch);  
            }  
            return szOutStringBuild.ToString();  
        } 

        string EncodeAsBase64String(string source)
        {
            byte[] sourceArray = new byte[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                sourceArray[i] = (byte)source[i];
            }
            return Convert.ToBase64String(sourceArray);
        }

        string DecodeFromBase64String(string source)
        {
            byte[] sourceArray = Convert.FromBase64String(source);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < sourceArray.Length; i++)
            {
                builder.Append((char) sourceArray[i]);
            }

            return builder.ToString();
        }
        
        public override void SaveToFile(string saveFile, JObject state)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path} ");
            using (var textWriter = File.CreateText(path))
            {
                string json = state.ToString();
                string encoded = EncryptDecrypt(json, key);
                string base64 = EncodeAsBase64String(encoded);
                textWriter.Write(base64);
            }
        }

        public override JObject LoadFromFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new JObject();
            }
            
            using (var textReader = File.OpenText(path))
            {
                string encoded = textReader.ReadToEnd();
                string decoded = DecodeFromBase64String(encoded);
                string json = EncryptDecrypt(decoded, key);
                return (JObject)JToken.Parse(json);
            }
        }
        public override string GetExtension()
        {
            return ".xortext";
        }
    #if UNITY_EDITOR

        [ContextMenu("Generate Random Key")]
        void GenerateKey()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("key");
            property.stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }
        
    #endif
    }
}

