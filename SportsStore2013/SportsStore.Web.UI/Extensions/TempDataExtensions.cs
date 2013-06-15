using System.Web.Mvc;
using SportsStore.Web.UI.Models;

namespace SportsStore.Web.UI.Extensions
{
    public static class TempDataExtensions
    {
        public static void AddMessage(this TempDataDictionary tempData, MessageType type, string message)
        {
            tempData["message"] = new Message { MessageType = type, Value = message };
        }
    }
}