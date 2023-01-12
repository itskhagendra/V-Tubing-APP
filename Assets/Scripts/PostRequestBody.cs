using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRequestBody 
{
    public class RegisterUser
    {
        public string email;
    }
    public class LoginUser
    {
        public string email;
        public string password;
    }
    public class VerifyOTP
    {
        public string email;
        public string otp;
    }

    public class UpdatePassword
    {
        public string email;
        public string password;
        public string otp;
        public int type;
    }
    
    public class Get2DAvatar
    {
        public class Root
        {
            public string model;
            public string scene;
            public string armature;
            public string blendShapes;
        }
        public class BlendShapes
        {
            public Wolf3DAvatar Wolf3D_Avatar;
        }


        public class Wolf3DAvatar
        {
            public double mouthSmile = 0.2f;
        }
    }
}
