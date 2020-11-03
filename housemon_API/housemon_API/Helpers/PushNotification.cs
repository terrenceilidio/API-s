using System;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using housemon_API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace housemon_API.Helpers
{
    public class PushNotification
    {
        public async Task SendPushNotification(string title, string body, string token)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
            });

            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body

                },
                Token = token
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
        }
    }
}
