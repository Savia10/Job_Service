using Job_Service.Models;
using Job_Service.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Job_Service.Common
{
    public class Validation
    {
        //public async Task<User> IsAuthenticated(string authHeader)
        //{
        //    if (authHeader.ToLower().StartsWith("basic ") && authHeader.Length > 6)
        //    {

        //        string encodedUsernamePassword = authHeader.Substring("basic ".Length).Trim();
        //        if (IsBase64String(encodedUsernamePassword))
        //        {
        //            string userCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
        //            string consumerName = userCredentials;
        //            string consumerPass = string.Empty;
        //            string consumerPassHash = string.Empty;
        //            if (userCredentials.IndexOf(":") != -1)
        //            {
        //                consumerName = userCredentials.Split(':')[0];
        //                consumerPass = userCredentials.Split(':')[1];
        //            }
        //            if (!string.IsNullOrEmpty(consumerName))
        //            {
        //                string userName = consumerName;

        //                User user = await _userRepository.GetUserById(userName);
        //                if (user != null)
        //                {
        //                    if (!string.IsNullOrEmpty(user.PasswordHash)) //No need to hashing if there is no Password to user
        //                    {
        //                        using (SHA256 sha256Hash = SHA256.Create())
        //                        {
        //                            // ComputeHash - returns byte array  
        //                            byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(consumerPass));

        //                            // Convert byte array to a string   
        //                            StringBuilder hashPassword = new StringBuilder();
        //                            for (int i = 0; i < hashBytes.Length; i++)
        //                            {
        //                                hashPassword.Append(hashBytes[i].ToString("X2"));
        //                            }
        //                            consumerPassHash = hashPassword.ToString();
        //                        }
        //                    }
        //                    if (string.IsNullOrEmpty(user.PasswordHash) || consumerPassHash.ToUpper().Equals(Convert.ToString(user.PasswordHash.ToUpper())))
        //                    {
        //                        return user;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return new User();

        //}



    }
}
