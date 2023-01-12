using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegisterUserResponse 
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [Serializable]
    public class Data
    {
        public string _id;
        public ImageUrl imageUrl;
        public bool isEmailVerify;
        public bool isBlocked;
        public bool isDeleted;
        public string email;
        public string username;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int __v;
        public bool isRegister;
        public string deviceId;
        public string accessToken;
        public bool isPasswordAvailable;
    }

    [Serializable]
    public class ImageUrl
    {
        public string original;
        public string thumbnail;
    }

    [Serializable]
    public class Root
    {
        public int statusCode;
        public string message;
        public Data data;
    }


}

[Serializable]
public class OTPVerifyResponse
{
    [Serializable]
    public class Data
    {
        public string _id;
        public ImageUrl imageUrl;
        public bool isEmailVerify;
        public string email;
        public string emailOtp;
        public string username;
        public int __v;
        public string deviceId;
        public string accessToken;
        public bool isRegister;
    }

    [Serializable]
    public class ImageUrl
    {
        public string original;
        public string thumbnail;
    }

    [Serializable]
    public class Root
    {
        public int statusCode;
        public string message;
        public Data data;
    }
}

[Serializable]
public class Avatars2D
{
    [Serializable]
    public class Root
    {
        public List<string> renders;
    }
}